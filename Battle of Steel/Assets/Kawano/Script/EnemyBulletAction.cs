using UnityEngine;

public class EnemyBulletAction : MonoBehaviour
{
    [SerializeField] float life_time;//弾丸の生存時間
    public GameObject explosive_effect;//爆発エフェクト
    public int attack_damage = 0;//弾丸のダメージ
    private Damage_Calclate cal;//ダメージ計算

    private void Start()
    {
        GameObject calc = GameObject.Find("Game_Manager");
        cal = calc.GetComponent<Damage_Calclate>();
        Destroy(gameObject, life_time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //弾丸がプレイヤーに当たったら体力を減らす
            GameObject explosive = Instantiate(explosive_effect, gameObject.transform.position, Quaternion.identity);
            Debug.Log("現在の体力" +Player_Status.Player_HP );
            Player_Status.Player_HP = cal.Damage_Cal(attack_damage,Player_Status.Player_HP);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            //壁に当たったら消す
            Destroy(gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //弾丸がプレイヤーに当たったら体力を減らす
            GameObject explosive = Instantiate(explosive_effect, gameObject.transform.position, Quaternion.identity);
            Debug.Log("現在の体力" + Player_Status.Player_HP);
            Player_Status.Player_HP = cal.Damage_Cal(attack_damage, Player_Status.Player_HP);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            //壁に当たったら消す
            Destroy(gameObject);
        }

    }
}