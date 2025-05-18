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
    [SerializeField] bool isEquipped = false;
    [SerializeField] float min_shoot_force = 90f, max_shoot_force = 100f;
    [SerializeField] float min_reload_time = 0.1f, max_reload_time = 1.5f;
    [SerializeField] float min_time_between_shooting = 0.01f, max_time_between_shooting = 0.30f;
    [SerializeField] int min_magazinesize = 1, max_magazinesize = 90;
    [SerializeField] float spread_amount = 0.01f;
    [SerializeField] bool allow_bullet_hold = false;

    [Header("弾丸プレハブ")]
    [SerializeField] GameObject bullet_prefab;
    [Header("弾丸情報テキスト")]
    [SerializeField] Text ammo_text;
    [Header("マズルフラッシュを読み込む")]
    public ParticleSystem flash;//フラッシュパーティクル
    public GameObject flash_light;//フラッシュライト
    [Header("銃SEを読み込む")]
    public AudioSource flash_sound;

    [Header("マテリアル設定")]
    [SerializeField] private string materialFolder = "Materials";
    [SerializeField] private bool useRandomMaterial = true;
    [SerializeField] private bool useRandomColor = true;
    [Header("カスタムカラー設定")]
    [SerializeField] private bool useCustomColor = false;
    [SerializeField] private Color customColor = Color.white; [SerializeField] private bool useAlbedoColor = true;
    [SerializeField] private bool useEmissionColor = true;
    [SerializeField] private bool useEmissionBlink = false;
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private float emissionIntensity = 1f;

    [Header("パーツ設定（nullならランダム）")]
    [SerializeField] private GameObject overrideHandle;
    [SerializeField] private GameObject overrideBody;
    [SerializeField] private GameObject overrideNozzle;

    private List<Material> loadedMaterials = new();
    private List<(Material mat, Color baseEmission)> blinkingMaterials = new();
    private List<GameObject> handles = new(), bodies = new(), nozzles = new();

    private Transform muzzle_transform;
 
    private float shoot_force, reload_time, time_between_shooting, spread;
    private int magazine_size, bullets_left, bullets_shot;
    private int flash_light_time = 0;
    private bool ready_to_shoot = true, reloading = false, allow_invoke = true, shooting = false;
    void Start()
    {
        BuildWeapon(type);
        flash_light.SetActive(false);

    }


    void Update()
    {
        //フラッシュライトが有効にされたら時間経過で消去
        if (flash_light.activeInHierarchy == true)
            flash_light_time++;
        if (flash_light_time > 60)
        {
            flash_light.SetActive(false);
            flash_light_time = 0;
        }
            

        if (!isEquipped) return;
        HandleInput();
        if (ammo_text) ammo_text.text = $"弾数: {bullets_left} / {magazine_size}";

        if (useEmissionBlink)
        {
            float intensity = Mathf.PingPong(Time.time * blinkSpeed, emissionIntensity);
            foreach (var (mat, baseColor) in blinkingMaterials)
                mat.SetColor("_EmissionColor", baseColor.linear * intensity);
        }
    }

    void HandleInput()
    {
        shooting = allow_bullet_hold ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        if (ready_to_shoot && shooting && !reloading && bullets_left > 0)
        {
            bullets_shot = 0; Shoot();
            flash.Play();
            flash_sound.Play();
            flash_light.SetActive(true);
        }
    }

    void Shoot()
    {
        ready_to_shoot = false;
        Vector3 spread_vec = muzzle_transform.TransformDirection(new Vector3(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread),
            0));
        Vector3 direction = muzzle_transform.forward + spread_vec;
        flash_light.transform.position = muzzle_transform.position;
        flash.transform.position = muzzle_transform.position;
        GameObject bullet = Instantiate(bullet_prefab, muzzle_transform.position, Quaternion.identity);
        bullet.transform.forward = direction.normalized;
        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shoot_force, ForceMode.Impulse);
        bullets_left--;
        bullets_shot++;

        if(bullets_left == 0)
        {
            Reload();
        }

        if (allow_invoke)
        {
            Invoke(nameof(ResetShot), time_between_shooting);
            allow_invoke = false;
        }

        if (bullets_shot < 1 && bullets_left > 0)
        {
            Invoke(nameof(Shoot), time_between_shooting);
        }
        
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

        GameObject handle = overrideHandle ? Instantiate(overrideHandle, weaponParent) : Instantiate(GetRandomPart(handles), weaponParent);
        GameObject body = overrideBody ? Instantiate(overrideBody, weaponParent) : Instantiate(GetRandomPart(bodies), weaponParent);
        GameObject nozzle = overrideNozzle ? Instantiate(overrideNozzle, weaponParent) : Instantiate(GetRandomPart(nozzles), weaponParent);

        ApplyMaterial(handle.GetComponentInChildren<Renderer>());
        ApplyMaterial(body.GetComponentInChildren<Renderer>());
        ApplyMaterial(nozzle.GetComponentInChildren<Renderer>());

        ConnectParts(handle.transform.Find("ConnectPoint_Body"), body.transform.Find("ConnectPoint_Handle"));
        ConnectParts(body.transform.Find("ConnectPoint_Nozzle"), nozzle.transform.Find("ConnectPoint_Body"));

        muzzle_transform = nozzle.transform;

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


    void ApplyMaterial(Renderer renderer)
    {
        if (renderer == null) return;

        Material mat;

        // マテリアル選択
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
            mat = new Material(baseMat);
            renderer.material = mat;
        }
        else
        {
            mat = renderer.material;
        }

        // カラー適用
        if (useRandomColor)
        {
            Color color = useCustomColor ? customColor : Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);

            if (useAlbedoColor)
                mat.color = color;

            if (useEmissionColor)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", color * emissionIntensity);

                if (useEmissionBlink)
                    blinkingMaterials.Add((mat, color));
            }
        }
    }
}