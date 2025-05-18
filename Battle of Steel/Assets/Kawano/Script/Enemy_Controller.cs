using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Enemy_Ai_Style
{
    //敵のAIリスト
    Idle,//停止

}
public enum Enemy_ID
{
    //敵のIDリスト
    Idle_Robot,//ロボット(停止)

}
public class Enemy_Status
{
    public static int max_hp;//最大体力
    public static int attack_damage;//攻撃力
    public Enemy_Ai_Style style;//AIスタイル

    public Enemy_Status(int set_hp,int set_damage,Enemy_Ai_Style set_style)
    {
        //各ステータスを入力
        max_hp = set_hp;
        attack_damage = set_damage;
        style = set_style;
    }
}

public class Enemy_Controller : Damage_Calclate
{
    [Header("敵のID")]
    [SerializeField] Enemy_ID id;
    //ここに敵のステータスを入力(体力、攻撃力、AI)
    public Dictionary<Enemy_ID, Enemy_Status> enemy_index = new()
    {
        {Enemy_ID.Idle_Robot,new Enemy_Status(30,5,Enemy_Ai_Style.Idle)},
    };

    //変数
    Animator animator;
    private int hp;//現在の体力
    private int damage;
    private bool act_shot;
    private Enemy_Ai_Style ai_style;//AIスタイル
    void Start()
    {
        animator = GetComponent<Animator>();
        hp = Enemy_Status.max_hp;//体力を設定
        damage = Enemy_Status.attack_damage;//攻撃力設定
    }
    void Update()
    {
        Enmey_State(ai_style);//エネミーの行動管理

        //体力が1以下ならアニメーション更新
        if(hp < 1)
        {
            hp = 0;
            animator.SetBool("Death", true);
        }
    }
    //エネミーの行動処理
    private void Enmey_State(Enemy_Ai_Style style)
    {
        if (style == Enemy_Ai_Style.Idle)
        {
            //停止状態
            if(act_shot == true)
            {
                //弾を発射
            }
        }
        
    }
    //Bulletタグに当たったら体力を減らす
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("見つけた");
            //敵からプレイヤーまでのベクトル作成
            Vector3 direction = gameObject.transform.position - collider.gameObject.transform.position; direction.y = 0;//yを0に設定
            transform.rotation = Quaternion.LookRotation(direction);//角度をdirectionまで変更
            act_shot = true;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp = Damage_Cal(damage, hp);
            collision.gameObject.IsDestroyed();
            Debug.Log("当たった  体力" + hp);
        }
    }

    //アニメーション中でこの関数を呼んでオブジェクトを消す
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}