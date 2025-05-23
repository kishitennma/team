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
                // 今の武器を地面に落とす
                if (currentWeapon == null)
                {
                    GameObject dropped = Instantiate(currentWeapon.gameObject);
                    dropped.transform.position = transform.position + Vector3.right; // 少しずらす
                    var dropScript = dropped.AddComponent<DroppedWeapon>();
                    dropScript.InitFromWeapon(currentWeapon);
                    Destroy(currentWeapon.gameObject);
                }

                // 新しい武器を拾う
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
