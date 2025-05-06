using UnityEngine;

public class Player_Weapon_Manager : MonoBehaviour
{
    public Transform weaponHolder;
    public KeyCode switchKey = KeyCode.Q;

    private WeaponSystem currentWeapon;

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            TrySwitchWeapon();
        }
    }

    void TrySwitchWeapon()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hit in hits)
        {
            DroppedWeapon drop = hit.GetComponent<DroppedWeapon>();
            if (drop != null)
            {
                // ç°ÇÃïêäÌÇínñ Ç…óéÇ∆Ç∑
                if (currentWeapon == null)
                {
                    GameObject dropped = Instantiate(currentWeapon.gameObject);
                    dropped.transform.position = transform.position + Vector3.right; // è≠ÇµÇ∏ÇÁÇ∑
                    var dropScript = dropped.AddComponent<DroppedWeapon>();
                    dropScript.InitFromWeapon(currentWeapon);
                    Destroy(currentWeapon.gameObject);
                }

                // êVÇµÇ¢ïêäÌÇèEÇ§
                currentWeapon = Instantiate(drop.weaponPrefab, weaponHolder).GetComponent<WeaponSystem>();
                Destroy(drop.gameObject);
                break;
            }
        }
    }
}

public class DroppedWeapon : MonoBehaviour
{
    public GameObject weaponPrefab;

    public void InitFromWeapon(WeaponSystem weapon)
    {
        weaponPrefab = weapon.gameObject;
    }

    void Start()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}
