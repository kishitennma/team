using UnityEngine;

public class EnemyBulletAction : MonoBehaviour
{
    [SerializeField] float life_time;//弾丸の生存時間

    public int attack_damage;//弾丸のダメージ
    private Damage_Calclate cal;

    private void Start()
    {
        Destroy(gameObject, life_time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            cal.Damage_Cal((int)attack_damage, Player_Status.Player_HP);
            Debug.Log("プレイヤーに当たった");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("当たった");
            Destroy(gameObject);
        }
    }
}