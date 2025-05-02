using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum WeaponType { Pistol, AssaultRifle }

public class WeaponSystem : MonoBehaviour
{
    [Header("武器生成")]
    [SerializeField] Transform weaponParent;
    public WeaponType type;

    [Header("武器パラメータ範囲")]
    [SerializeField] float min_shoot_force = 0f, max_shoot_force = 100f;
    [SerializeField] float min_reload_time = 0.1f, max_reload_time = 1.5f;
    [SerializeField] float min_time_between_shooting = 0.01f, max_time_between_shooting = 0.30f;
    [SerializeField] int min_magazinesize = 1, max_magazinesize = 90;
    [SerializeField] float spread_amount = 0.01f;
    [SerializeField] bool allow_bullet_hold = false;

    [Header("弾丸プレハブ")]
    [SerializeField] GameObject bullet_prefab;
    [Header("弾丸情報テキスト")]
    [SerializeField] Text ammo_text;
    [Header("入力キー設定")]
    [SerializeField] KeyCode reload_key = KeyCode.R;

    [Header("マテリアル設定")]
    [SerializeField] private string materialFolder = "Materials";
    [SerializeField] private bool useRandomMaterial = true;  // ランダムでマテリアルを適用するか
    [SerializeField] private Material defaultMaterial;       // 既定のマテリアル（エディタ上で設定）
    [SerializeField] private bool useRandomColor = true;      // ランダムカラーを使用するか
    [SerializeField] private bool useEmissionBlink = false;
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private float emissionIntensity = 1f;

    private List<Material> loadedMaterials = new();
    private List<Material> instanceMaterials = new(); // 明滅制御用
    private List<(Material mat, Color baseEmission)> blinkingMaterials = new();
    private List<GameObject> handles = new();
    private List<GameObject> bodies = new();
    private List<GameObject> nozzles = new();

    private Transform nozzle_transform;

    private float shoot_force, reload_time, time_between_shooting, spread;
    private int magazine_size, bullets_left, bullets_shot;
    private bool ready_to_shoot = true, reloading = false, allow_invoke = true, shooting = false;

    void Start() => BuildWeapon(type);

    void Update()
    {
        HandleInput();
        if (ammo_text) ammo_text.text = $"弾数: {bullets_left} / {magazine_size}";

        if (useEmissionBlink)
        {
            float intensity = Mathf.PingPong(Time.time * blinkSpeed, emissionIntensity);
            foreach (var (mat, baseColor) in blinkingMaterials)
            {
                mat.SetColor("_EmissionColor", baseColor.linear * intensity);
            }
        }
    }
    void HandleInput()
    {
        shooting = allow_bullet_hold ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        if (ready_to_shoot && shooting && !reloading && bullets_left > 0) { bullets_shot = 0; Shoot(); }
        if (Input.GetKeyDown(reload_key) && bullets_left < magazine_size && !reloading) Reload();
    }
    void Shoot()
    {
        ready_to_shoot = false;
        Vector3 spread_vec = nozzle_transform.TransformDirection(new Vector3(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread),
            0));
        Vector3 direction = nozzle_transform.forward + spread_vec;

        GameObject bullet = Instantiate(bullet_prefab, nozzle_transform.position, Quaternion.identity);
        bullet.transform.forward = direction.normalized;
        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shoot_force, ForceMode.Impulse);

        bullets_left--;
        bullets_shot++;

        if (allow_invoke)
        {
            Invoke(nameof(ResetShot), time_between_shooting);
            allow_invoke = false;
        }

        if (bullets_shot < 1 && bullets_left > 0) Invoke(nameof(Shoot), time_between_shooting);
    }

    void ResetShot() { ready_to_shoot = true; allow_invoke = true; }

    void Reload()
    {
        reloading = true;
        Debug.Log("リロード中");
        Invoke(nameof(ReloadFinished), reload_time);
    }

    void ReloadFinished()
    {
        bullets_left = magazine_size;
        reloading = false;
    }

    public void BuildWeapon(WeaponType weapon_type)
    {
        ClearWeapon();
        LoadParts(weapon_type);

        if (handles.Count == 0 || bodies.Count == 0 || nozzles.Count == 0)
        {
            Debug.LogError("パーツロード失敗！");
            return;
        }

        GameObject handle = Instantiate(GetRandomPart(handles), weaponParent);
        GameObject body = Instantiate(GetRandomPart(bodies), weaponParent);
        GameObject nozzle = Instantiate(GetRandomPart(nozzles), weaponParent);
        // マテリアル適用
        ApplyRandomMaterial(handle);
        ApplyRandomMaterial(body);
        ApplyRandomMaterial(nozzle);
        ConnectParts(
            handle.transform.Find("ConnectPoint_Body"),
            body.transform.Find("ConnectPoint_Handle"));

        ConnectParts(
            body.transform.Find("ConnectPoint_Nozzle"),
            nozzle.transform.Find("ConnectPoint_Body"));

        nozzle_transform = nozzle.transform;

        shoot_force = Random.Range(min_shoot_force, max_shoot_force);
        reload_time = Random.Range(min_reload_time, max_reload_time);
        time_between_shooting = Random.Range(min_time_between_shooting, max_time_between_shooting);
        magazine_size = Random.Range(min_magazinesize, max_magazinesize);
        bullets_left = magazine_size;
        spread = spread_amount;
        ready_to_shoot = true;
    }

    void ConnectParts(Transform base_point, Transform attach_point)
    {
        if (base_point == null || attach_point == null)
        {
            Debug.LogError("接続ポイントが見つかりません");
            return;
        }

        Transform part = attach_point.parent;
        part.position = base_point.position;
        part.rotation = base_point.rotation;
    }

    void LoadParts(WeaponType type)
    {
        handles.Clear(); bodies.Clear(); nozzles.Clear();
        string basePath = $"{type}";
        handles.AddRange(Resources.LoadAll<GameObject>($"{basePath}/Handles"));
        bodies.AddRange(Resources.LoadAll<GameObject>($"{basePath}/Bodies"));
        nozzles.AddRange(Resources.LoadAll<GameObject>($"{basePath}/Nozzles"));
    }

    void ClearWeapon()
    {
        foreach (Transform child in weaponParent) Destroy(child.gameObject);
    }

    GameObject GetRandomPart(List<GameObject> parts) => parts.Count > 0 ? parts[Random.Range(0, parts.Count)] : null;
    void ApplyRandomMaterial(GameObject obj)
    {
        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer == null) return;

        // 既定のマテリアル＆色もそのまま使う → 処理しない
        if (!useRandomMaterial && !useRandomColor) return;

        Material matInstance;

        if (useRandomMaterial)
        {
            if (loadedMaterials.Count == 0)
                loadedMaterials.AddRange(Resources.LoadAll<Material>(materialFolder));
            if (loadedMaterials.Count == 0)
            {
                Debug.LogWarning("マテリアルが見つかりません");
                return;
            }
            Material baseMat = loadedMaterials[Random.Range(0, loadedMaterials.Count)];
            matInstance = new Material(baseMat);
            renderer.material = matInstance;
        }
        else
        {
            // インスペクター上のマテリアルをそのまま使いつつインスタンス化して色だけ変更
            matInstance = renderer.material; // 自動的にインスタンス化される
        }
        // 色の設定
        if (useRandomColor)
        {
            Color emissionColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
            matInstance.EnableKeyword("_EMISSION");
            matInstance.SetColor("_EmissionColor", emissionColor * emissionIntensity);

            if (useEmissionBlink)
            {
                blinkingMaterials.Add((matInstance, emissionColor));
            }
        }
    }
}