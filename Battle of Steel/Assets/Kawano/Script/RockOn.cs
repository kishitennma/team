using UnityEngine;
//ロックオンシステム

public class RockOn : MonoBehaviour
{
    [Header("プレイヤーオブジェクト")]
    [SerializeField] GameObject player_object;//プレイヤー

    private GameObject target;//敵

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //範囲内に入ってエネミーだったらtargetに入れる
        if(other.gameObject.CompareTag("Enemy"))
        {
            target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //範囲外に出たらtargetを初期化
        if(other.gameObject.CompareTag("Enemy"))
        {
            target = null;
        }
    }
    //ターゲット情報を返す
    public GameObject GetTarget()
    {
        return this.target;
    }

    private GameObject RockOnPlayerToEnemy()
    {
        float search_radius = 10f;
        //var hits = Physics.SphereCastAll(
        //    player_object.transform.position,
        //    search_radius,
        //    player_object.transform.forward,
        //    0.01f
        //    )


        return null;
    }
}
