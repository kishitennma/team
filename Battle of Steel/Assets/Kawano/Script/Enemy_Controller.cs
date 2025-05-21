using System.Collections.Generic;
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
    [SerializeField] Enemy_ID id;//敵のID
    [SerializeField] Collision find_player_collision;//プレイヤーを探すためのあたり判定
    [Header("インスペクタ確認用")]
    [SerializeField] int set_hp;//インスペクタ確認用
    [Header("弾丸プレハブ")]
    [SerializeField] GameObject bullet_prefab;//弾丸のプレハブ
    //ここに敵のステータスを入力(体力、攻撃力、AI)
    public Dictionary<Enemy_ID, Enemy_Status> enemy_index = new()
    {
        {Enemy_ID.Idle_Robot,new Enemy_Status(30,5,Enemy_Ai_Style.Idle)},
    };

    //変数
    Animator animator;//アニメーター
    private int hp;//現在の体力
    private Enemy_Ai_Style ai_style;//AIスタイル
    void Start()
    {
        animator = GetComponent<Animator>();//アニメーターを取得
        
        hp = Enemy_Status.max_hp;//体力を設定
    }
    void Update()
    {
        Enmey_State(ai_style);//エネミーの行動管理
        set_hp = hp;
        //体力が1以下ならアニメーションを変更
        if(hp < 1)
        {
            Debug.Log("敵を倒した");
            animator.SetBool("Death", true);//アニメーターの条件を更新
        }
    }
    //エネミーの行動処理
    private void Enmey_State(Enemy_Ai_Style style)
    {
        //styleが各AIスタイルの場合その挙動を設定
        if (style == Enemy_Ai_Style.Idle)
        {
            //停止状態
        }
        else { }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Tag.Playerを持っていた場合そのオブジェクトの方向を向く
        if(collision.gameObject.CompareTag("Player"))
        {
            transform.LookAt(collision.gameObject.transform);//方向を設定
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Object.BulletがCollisionに衝突した場合、体力を現象させる
        if(other.CompareTag("Bullet"))
        {
            hp -= 5;
            Debug.Log("当たった");

        }
    }
    public void DestroyObject()
    {
        Destroy(gameObject);//このスクリプトがアタッチされているオブジェクト破棄
    }
}