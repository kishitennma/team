using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum WeaponType 
{ 
    Pistol,
    AssaultRifle,
    ShotGun,
}
public class WeaponSystem : MonoBehaviour
{
    [Header("武器生成")]
    [SerializeField] Transform weaponParent;
    [SerializeField] public bool isMainWeapon;
    public WeaponType type;
    [Header("弾丸プレハブ")]
    [SerializeField] GameObject bullet_prefab;
    [Header("弾丸情報テキスト")]
    public Text ammo_text;
    public GameObject not_ammo_text;
    [Header("マズルフラッシュを読み込む")]
    public ParticleSystem flash;//フラッシュパーティクル
    public GameObject flash_light;//フラッシュライト
    [Header("銃SEを読み込む")]
    public AudioSource flash_sound;

    [Header("マテリアル設定")]
    [SerializeField] private bool useRandomMaterial = true;
    [SerializeField] private bool useRandomColor = true;
    [Header("カスタムカラー設定")]
    [SerializeField] private bool useCustomColor = false;
    [SerializeField] private Color customColor = Color.white; [SerializeField] private bool useAlbedoColor = true;
    [SerializeField] private bool useEmissionColor = true;
    [SerializeField] private bool useEmissionBlink = false;

    [Header("パーツ設定（nullならランダム）")]
    [SerializeField] private GameObject overrideHandle;
    [SerializeField] private GameObject overrideBody;
    [SerializeField] private GameObject overrideNozzle;

    [Header("武器の性能（確認用）")]
    public float shoot_force, reload_time, time_between_shooting, spread;
    public int magazine_size, bullets_left, bullets_shot;
    public int setting_attack_dmg = 0;//他のスクリプトに渡すようの変数
    public PlayerController player;
    //武器情報リスト
    public Dictionary<int, Weapon_Date> weapon_index = new()
    {
        //辞書番号　             武器タイプ、弾速、リロード時間、発射間隔、マガジン容量、発散、連射(true)か単発、攻撃力
        {-1,new Weapon_Date(WeaponType.Pistol,0,   0f,           0f,        0,            0f,    false,           0)},

        //武器データ(ステータスのみ)
        {0,new Weapon_Date(WeaponType.Pistol,      20, 2f, 0.1f,  12, 0.005f,  false,22)},//ピストル
        {1,new Weapon_Date(WeaponType.AssaultRifle,20, 24f, 0.15f, 48,  0.01f,  true,  6)},//アサルト
        {2,new Weapon_Date(WeaponType.ShotGun,     20, 2.5f, 0.7f,  6,  0.06f,  false, 9)},//ショットガン
    };

    private List<Material> loadedMaterials = new();//マテリアルリスト
    private List<(Material mat, Color baseEmission)> blinkingMaterials = new();//マテリアル色情報リスト
    private List<GameObject> handles = new(), bodies = new(), nozzles = new();//武器構成要素リスト

    private string materialFolder = "Materials";//マテリアルフォルダ
    private float blinkSpeed = 2f;//明滅速度
    private float emissionIntensity = 1f;//発光強度
    private int allow_per_shots = 5;//同時発射数
    public int index; Weapon_Date weapon;//武器保存用

    public Transform muzzle_transform;//Muzzleの位置
    private bool allow_bullet_hold;//連射
    private int flash_light_time = 0;//フラッシュライトの発射時間
    public bool ready_to_shoot = true, reloading = false, allow_invoke = true, shooting = false;

    public bool isEquiped;//現在所持している武器

    public  bool on_reload;//リロード中
    private float set_rel_time;
    private float set_timer;
    void Start()
    {
        Application.targetFrameRate = 120;//60FPS（仮）
        not_ammo_text.SetActive(false);
        //PlayerPrefsにセーブされた二つの数字を読み込む
        index = PlayerPrefs.GetInt(isMainWeapon ? "Select_f" : "Select_s", -1);
        
        //nullなら-1
        if (!weapon_index.ContainsKey(index))
        {
            Debug.LogError($"武器インデックス {index} が見つかりません");
            return;
        }

        weapon = weapon_index[index];//武器情報を持たせる
        BuildWeapon(weapon.type); // 見た目生成

        // 武器ステータス適用
        shoot_force = weapon.shot_force * 10;//弾丸の発射速度
        reload_time = weapon.relode_time;//リロード時間
        time_between_shooting = weapon.time_between_shooting;//
        magazine_size = weapon.magazine_size;
        bullets_left = magazine_size;
        spread = weapon.spread_amount;
        allow_bullet_hold = weapon.allow_bullet_hold;
        flash_light.SetActive(false);

        if (isMainWeapon)
            isEquiped = true;
    }

    //ここで、武器のステータス、情報を設定
    void Update()
    {
        //qが押されたら、テキストを終了
        if (Input.GetKeyDown(KeyCode.Q))
        {
            not_ammo_text.SetActive(false);

        }
        if (bullets_left <= 0)
            on_reload = true;

        //常にこの武器のSetActiveがtrueの時、攻撃力を更新させる
        setting_attack_dmg = weapon.attack_damage;
        //フラッシュライトが有効にされたら時間経過で消去
        if (flash_light.activeInHierarchy == true)
            flash_light_time++;
        if (flash_light_time > 60)
        {
            flash_light.SetActive(false);
            flash_light_time = 0;
        }

        //この武器を所持していた時
        if (isEquiped)
        {
            //if (on_reload)
            //    on_reload = false;
            if(!on_reload)
            HandleInput();
            //弾丸の残段数/最大数を表示
            if (ammo_text) ammo_text.text = $"{bullets_left} / {magazine_size}";
            if (on_reload)
            {
                set_timer++;
                set_rel_time = Reload_Set_Time(magazine_size, reload_time);
                //所持弾数が最大弾数より小さく、リロード時間を超えたら弾丸を１増加
                if (bullets_left < magazine_size && set_timer > set_rel_time * 12)
                {
                    bullets_left++;
                    set_timer = 0;
                    if (bullets_left >= magazine_size)
                    {
                        not_ammo_text.SetActive(false);
                        on_reload = false;
                    }

                }
            }

        }
        else
        {
            if (on_reload)
            {
                set_timer++;
                set_rel_time = Reload_Set_Time(magazine_size, reload_time);
                //所持弾数が最大弾数より小さく、リロード時間を超えたら弾丸を１増加
                if (bullets_left < magazine_size && set_timer > set_rel_time*12)
                {
                    bullets_left++;
                    set_timer = 0;
                    if (bullets_left > magazine_size)
                    {
                        on_reload = false;
                    }

                }
            }

        }
        if (useEmissionBlink)
        {
            float intensity = Mathf.PingPong(Time.time * blinkSpeed, emissionIntensity);
            foreach (var (mat, baseColor) in blinkingMaterials)
                mat.SetColor("_EmissionColor", baseColor.linear * intensity);
        }

    }
    //武器情報設定、最終組み立て関数
    void HandleInput()
    {
        //武器の基礎を作成
        shooting = allow_bullet_hold ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        if (ready_to_shoot && shooting && !reloading && bullets_left > 0)
        {
            bullets_shot = 0;
            //ショットガンの場合、複数の弾を同時に出す
            if(weapon.type == WeaponType.ShotGun)
            {
                Bullet_Action.life_time = 0.25f;
                int bullet_per_tap = allow_per_shots;//５発
                for (int i = 0; i < bullet_per_tap;i++)
                {
                    Shoot();
                }
            }
            else
            {
                Bullet_Action.life_time = 0.6f;
                Shoot();
            }
                bullets_left--;

            if(bullets_left <= 0)
            {
                not_ammo_text.SetActive(true);
            }

            flash.Play();
            flash_sound.Play();
            flash_light.SetActive(true);
        }
    }
    //弾丸発射関数
    void Shoot()
    {
        //弾丸を発射する
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
        bullets_shot++;


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
    //リロード開始関数
    public void Reload()
    {
        on_reload = true;
    }
    //武器組み立て関数
    public void BuildWeapon(WeaponType weapon_type)
    {
        //武器を組み立てる
        ClearWeapon();
        LoadParts(weapon_type);

        //パーツ生成
        GameObject handle = overrideHandle ? Instantiate(overrideHandle, weaponParent) : Instantiate(GetRandomPart(handles), weaponParent);
        GameObject body = overrideBody ? Instantiate(overrideBody, weaponParent) : Instantiate(GetRandomPart(bodies), weaponParent);
        GameObject nozzle = overrideNozzle ? Instantiate(overrideNozzle, weaponParent) : Instantiate(GetRandomPart(nozzles), weaponParent);
        //マテリアル適用
        ApplyMaterial(handle.GetComponentInChildren<Renderer>());
        ApplyMaterial(body.GetComponentInChildren<Renderer>());
        ApplyMaterial(nozzle.GetComponentInChildren<Renderer>());
        //パーツ接続
        ConnectParts(handle.transform.Find("ConnectPoint_Body"), body.transform.Find("ConnectPoint_Handle"));
        ConnectParts(body.transform.Find("ConnectPoint_Nozzle"), nozzle.transform.Find("ConnectPoint_Body"));
        //発射可能にする
        ready_to_shoot = true;
    }
    //パーツ接続関数
    void ConnectParts(Transform base_point, Transform attach_point)
    {
        //武器のパーツどうしをくっつける
        if (base_point == null || attach_point == null)
        {
            Debug.LogError("接続ポイントが見つかりません");
            return;
        }

        Transform part = attach_point.parent;
        part.position = base_point.position;
        part.rotation = base_point.rotation;
    }
    //パーツ読み込み関数
    void LoadParts(WeaponType type)
    {
        //武器のパーツをロード
        handles.Clear(); bodies.Clear(); nozzles.Clear();
        string basePath = $"{type}";
        handles.AddRange(Resources.LoadAll<GameObject>($"{basePath}/Handles"));
        bodies.AddRange(Resources.LoadAll<GameObject>($"{basePath}/Bodies"));
        nozzles.AddRange(Resources.LoadAll<GameObject>($"{basePath}/Nozzles"));
    }
    //武器消去関数
    void ClearWeapon()
    {
        foreach (Transform child in weaponParent) Destroy(child.gameObject);
    }
    //パーツランダム取得関数
    GameObject GetRandomPart(List<GameObject> parts) => parts.Count > 0 ? parts[Random.Range(0, parts.Count)] : null;
    //マテリアル適用関数
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
    //リロード時間計算関数
    private float Reload_Set_Time(int mag_size, float reload_time)
    {
        //最大弾数をリロード時間で割って、一発ごとにかかる時間を算出
        if (mag_size == 0 || reload_time == 0)
            return 0;
        float set_time = mag_size / reload_time;

        return set_time;
    }
}

//武器情報ベースクラス
// Weapon_Date.cs
public class Weapon_Date
{
    public WeaponType type;//武器の種類
    public int shot_force;//発射速度
    public float relode_time;//リロードにかかる時間
    public float time_between_shooting;//発射間隔
    public int  magazine_size;//マガジンの容量
    public float spread_amount;//発散の強度
    public bool allow_bullet_hold;//フルオートかどうか
    public int attack_damage;//攻撃力
    public Weapon_Date(WeaponType w_type, int force, float r_time, float bet_shot, int mag_size, float spr_amount, bool bullet_hold, int attack)
    {
        type = w_type;
        shot_force = force;
        relode_time = r_time;
        time_between_shooting = bet_shot;
        magazine_size = mag_size;
        spread_amount = spr_amount;
        allow_bullet_hold = bullet_hold;
        attack_damage = attack;
    }
}