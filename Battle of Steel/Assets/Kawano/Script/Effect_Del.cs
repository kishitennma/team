using UnityEngine;

public class Effect_Del : MonoBehaviour
{
    public ParticleSystem Effects;//パーティクルオブジェクト

    // Update is called once per frame
    void Update()
    {
        //パーティクルが空、
        if(Effects != null && !Effects.IsAlive())
        {
            Destroy(gameObject);//ゲームオブジェクト終了
        }
    }
}
