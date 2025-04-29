using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public enum WeaponType
{
    Pistol,
    AssaultRifle
}

public class WeaponSystem : MonoBehaviour
{
    [Header("���퐶��")]
    [SerializeField] Transform weaponParent;
    public WeaponType type;
    [Header("����p�����[�^�͈�")]
    [SerializeField] float minShootForce = 0f;
    [SerializeField] float maxShootForce = 100f;
    [SerializeField] float minReloadTime = 0.1f;
    [SerializeField] float maxReloadTime = 1.5f;
    [SerializeField] float minTimeBetweenShooting = 0.01f;
    [SerializeField] float maxTimeBetweenShooting = 0.30f;
    [SerializeField] int minMagazineSize = 1;
    [SerializeField] int maxMagazineSize = 90;
    [SerializeField] float spreadAmount = 0.01f;
    [SerializeField] bool allowBulletHold = false;
    [Header("�e�ۃv���n�u")]
    [SerializeField] GameObject bulletPrefab;
    [Header("�e�ۏ��e�L�X�g")]
    [SerializeField] private Text ammoText;
    [Header("���̓L�[�ݒ�")]
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    private List<GameObject> handles = new List<GameObject>();
    private List<GameObject> bodies = new List<GameObject>();
    private List<GameObject> nozzles = new List<GameObject>();

    private Transform nozzleTransform;
    
    private float shootForce;
    private float reloadTime;
    private float timeBetweenShooting;
    private int magazineSize;

    private int bulletsLeft;
    private bool readyToShoot = true;
    private bool reloading = false;
    private bool allowInvoke = true;

    private bool shooting = false;

    private int bulletsShot;
    private float spread;

    void Start()
    {
        BuildWeapon(WeaponType.Pistol); // ������
    }

    void Update()
    {
        HandleInput();
        UpdateAmmoDisplay();
    }
    //�e�ۏ��\��
    private void UpdateAmmoDisplay()
    {
        if (ammoText != null)
        {
            ammoText.text = $"�e��: {bulletsLeft} / {magazineSize}";
        }
    }
    //���͏���
    private void HandleInput()
    {
        shooting = allowBulletHold ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }

        if (Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
    }

    //���ˏ���
    private void Shoot()
    {
        readyToShoot = false;

        Vector3 directionWithoutSpread = nozzleTransform.forward;
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 directionWithSpread = directionWithoutSpread + nozzleTransform.TransformDirection(new Vector3(x, y, 0));

        GameObject bullet = Instantiate(bulletPrefab, nozzleTransform.position, Quaternion.identity);
        bullet.transform.forward = directionWithSpread.normalized;
        bullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < 1 && bulletsLeft > 0)
        {
            Invoke(nameof(Shoot), timeBetweenShooting);
        }
    }
    //���˃��Z�b�g
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    //�����[�h
    private void Reload()
    {
        reloading = true;
        Debug.Log("�����[�h��");
        Invoke(nameof(ReloadFinished), reloadTime);
    }
    //�����[�h����
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    //�����g�ݗ��Ă�
    public void BuildWeapon(WeaponType weaponType)
    {
        ClearWeapon();
        LoadParts(weaponType);

        if (handles.Count == 0 || bodies.Count == 0 || nozzles.Count == 0)
        {
            Debug.LogError("�p�[�c���[�h���s�I");
            return;
        }
        GameObject handle = Instantiate(GetRandomPart(handles), weaponParent);
        GameObject body = Instantiate(GetRandomPart(bodies), weaponParent);
        GameObject nozzle = Instantiate(GetRandomPart(nozzles), weaponParent);
        Transform handleConnectPoint = handle.transform.Find("ConnectPoint_Body");
        AlignParts(handleConnectPoint, body.transform);
        Transform bodyConnectPoint = body.transform.Find("ConnectPoint_Nozzle");
        AlignParts(bodyConnectPoint, nozzle.transform);
        nozzleTransform = nozzle.transform;
        // �p�����[�^�������_���ݒ�
        shootForce = Random.Range(minShootForce, maxShootForce);
        reloadTime = Random.Range(minReloadTime, maxReloadTime);
        timeBetweenShooting = Random.Range(minTimeBetweenShooting, maxTimeBetweenShooting);
        magazineSize = Random.Range(minMagazineSize, maxMagazineSize);
        spread = spreadAmount;
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    //���i��t�֐�
    private void AlignParts(Transform basePoint, Transform attachPart)
    {
        if (basePoint == null)
        {
            Debug.LogError("���t���|�C���g�Ȃ��I");
            return;
        }
        attachPart.position = basePoint.position;
        attachPart.rotation = basePoint.rotation;
    }
    //���i��Resources���烍�[�h
    private void LoadParts(WeaponType type)
    {
        handles.Clear();
        bodies.Clear();
        nozzles.Clear();

        string basePath = "Parts/" + type.ToString();
        handles.AddRange(Resources.LoadAll<GameObject>(basePath + "/Handles"));
        bodies.AddRange(Resources.LoadAll<GameObject>(basePath + "/Bodies"));
        nozzles.AddRange(Resources.LoadAll<GameObject>(basePath + "/Nozzles"));
    }
    //������폜����֐�
    private void ClearWeapon()
    {
        foreach (Transform child in weaponParent)
        {
            Destroy(child.gameObject);
        }
    }
    //���i�������_���擾
    private GameObject GetRandomPart(List<GameObject> parts)
    {
        if (parts == null || parts.Count == 0) return null;
        return parts[Random.Range(0, parts.Count)];
    }

}