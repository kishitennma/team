using UnityEngine;



public class Bullet_Action : MonoBehaviour
{
    [SerializeField] float life_time;//弾丸の生存時間
    public GameObject explosive_obj;//弾丸爆発エフェクト
    private void Start()
    {
        Destroy(gameObject,life_time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            GameObject explosive = Instantiate(explosive_obj, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}