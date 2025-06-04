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
    [Header("���퐶��")]
    [SerializeField] Transform weaponParent;
    [SerializeField] bool isMainWeapon;
    public WeaponType type;
    [Header("�e�ۃv���n�u")]
    [SerializeField] GameObject bullet_prefab;
    [Header("�e�ۏ��e�L�X�g")]
    public Text ammo_text;
    [Header("�}�Y���t���b�V����ǂݍ���")]
    public ParticleSystem flash;//�t���b�V���p�[�e�B�N��
    public GameObject flash_light;//�t���b�V�����C�g
    [Header("�eSE��ǂݍ���")]
    public AudioSource flash_sound;

    [Header("�}�e���A���ݒ�")]
    [SerializeField] private bool useRandomMaterial = true;
    [SerializeField] private bool useRandomColor = true;
    [Header("�J�X�^���J���[�ݒ�")]
    [SerializeField] private bool useCustomColor = false;
    [SerializeField] private Color customColor = Color.white; [SerializeField] private bool useAlbedoColor = true;
    [SerializeField] private bool useEmissionColor = true;
    [SerializeField] private bool useEmissionBlink = false;

    [Header("�p�[�c�ݒ�inull�Ȃ烉���_���j")]
    [SerializeField] private GameObject overrideHandle;
    [SerializeField] private GameObject overrideBody;
    [SerializeField] private GameObject overrideNozzle;

    [Header("����̐��\�i�m�F�p�j")]
    public float shoot_force, reload_time, time_between_shooting, spread;
    public int magazine_size, bullets_left, bullets_shot;


    public PlayerController player;
    public Dictionary<int, Weapon_Date> weapon_index = new()
    {
        //�����ԍ��@             ����^�C�v�A�e���A�����[�h���ԁA���ˊԊu�A�}�K�W���e�ʁA���U�A�A��(true)���P���A�U����
        {-1,new Weapon_Date(WeaponType.Pistol,0,   0f,           0f,        0,            0f,    false,           0)},

        //����f�[�^(�X�e�[�^�X�̂�)
        {0,new Weapon_Date(WeaponType.Pistol,      40,0.4f,0.6f,12,0.02f,false,25)},
        {1,new Weapon_Date(WeaponType.AssaultRifle,60,0.2f,0.3f,36,0.1f,true, 10)},
        {2,new Weapon_Date(WeaponType.ShotGun,     60,1.2f,0.7f,6,0.2f,  false, 6)},          //6*5�ō��v30
    };

    private List<Material> loadedMaterials = new();
    private List<(Material mat, Color baseEmission)> blinkingMaterials = new();
    private List<GameObject> handles = new(), bodies = new(), nozzles = new();

    private string materialFolder = "Materials";
    private float blinkSpeed = 2f;
    private float emissionIntensity = 1f;
    int index; Weapon_Date weapon;

    private Transform muzzle_transform;
    private bool allow_bullet_hold;
    private int flash_light_time = 0;
    private bool ready_to_shoot = true, reloading = false, allow_invoke = true, shooting = false;
    void Start()
    {

        //PlayerPrefs�ɃZ�[�u���ꂽ��̐�����ǂݍ���
        index = PlayerPrefs.GetInt(isMainWeapon ? "Select_f" : "Select_s", -1);
        //null�Ȃ�-1
        if (!weapon_index.ContainsKey(index))
        {
            Debug.LogError($"����C���f�b�N�X {index} ��������܂���");
            return;
        }

        weapon = weapon_index[index];//���������������
        Debug.Log(index);//�C���f�b�N�X�ԍ����擾
        BuildWeapon(weapon.type); // �����ڐ���

        // �X�e�[�^�X�K�p
        shoot_force = weapon.shot_force * 10;
        reload_time = weapon.relode_time;
        time_between_shooting = weapon.time_between_shooting;
        magazine_size = weapon.magazine_size;
        bullets_left = magazine_size;
        spread = weapon.spread_amount;
        allow_bullet_hold = weapon.allow_bullet_hold;
        player.attack_power = weapon.attack_damage;
        Debug.Log("�v���C���[�U����" + player.attack_power);
        flash_light.SetActive(false);
    }


    void Update()
    {
        //��ɂ��̕����SetActive��true�̎��A�U���͂��X�V������
        player.attack_power = weapon.attack_damage;
        //�t���b�V�����C�g���L���ɂ��ꂽ�玞�Ԍo�߂ŏ���
        if (flash_light.activeInHierarchy == true)
            flash_light_time++;
        if (flash_light_time > 60)
        {
            flash_light.SetActive(false);
            flash_light_time = 0;
        }
        HandleInput();
        //�e�ۖ��������AQ�L�[�������ꂽ��e�ە�[
        if (bullets_left == 0 && Input.GetKeyDown(KeyCode.Q))
        {
            Invoke(nameof(Reload), 5f);
        }


        if (ammo_text) ammo_text.text = $"{bullets_left} / {magazine_size}";

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
            bullets_shot = 0;

            if(weapon.type != WeaponType.ShotGun)
            {
                int bullet_per_tap = 5;
                for (int i = 0; i < bullet_per_tap;i++)
                {
                    Shoot();
                }
                
            }
            else
            {
                Shoot();
            }
                bullets_left--;

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

    public void Reload()
    {
        reloading = true;
       Invoke(nameof(ReloadFinished),reload_time);
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

        ready_to_shoot = true;
    }

    void ConnectParts(Transform base_point, Transform attach_point)
    {
        if (base_point == null || attach_point == null)
        {
            Debug.LogError("�ڑ��|�C���g��������܂���");
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

        // �}�e���A���I��
        if (useRandomMaterial)
        {
            if (loadedMaterials.Count == 0)
                loadedMaterials.AddRange(Resources.LoadAll<Material>(materialFolder));

            if (loadedMaterials.Count == 0)
            {
                Debug.LogWarning("�}�e���A����������܂���");
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

        // �J���[�K�p
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

//������x�[�X�N���X
// Weapon_Date.cs
public class Weapon_Date
{
    public WeaponType type;
    public int shot_force;
    public float relode_time;
    public float time_between_shooting;
    public int magazine_size;
    public float spread_amount;
    public bool allow_bullet_hold;
    public int attack_damage;
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