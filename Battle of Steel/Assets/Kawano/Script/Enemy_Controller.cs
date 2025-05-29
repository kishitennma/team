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
    Boss_Normal,//通常のボス(停止)
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
    [Header("弾のプレハブ")]
    [SerializeField] GameObject bullet_prefab;//弾のプレハブ
    [SerializeField] GameObject bullet_point;//弾の発射位置
    [SerializeField] int bullet_force;//弾丸の発射速度
    [SerializeField] int bullet_per_shot;//発射間隔
    public Dictionary<Enemy_ID, Enemy_Status> enemy_index = new()
    {
        //ここに敵のステータスを入力(体力、攻撃力、AI)
        {Enemy_ID.Idle_Robot ,new Enemy_Status( 30, 5,Enemy_Ai_Style.Idle) },
        {Enemy_ID.Boss_Normal,new Enemy_Status(100,15,Enemy_Ai_Style.Idle) },
    };

    //変数
    public int count_game_state = 0;
    Animator animator;
    PlayerController p_cnt;
    private int hp;//現在の体力
    private int b_time;//弾丸発射時間
    private int damage;//攻撃力
    private int add_count = 5;//加算値
    private bool act_shot = false;//弾丸発射許可値
    private Vector3 e_vec;//ベクトル
    private Enemy_Ai_Style ai_style;//AIスタイル
    void Start()
    {
        act_shot = false;
        animator = GetComponent<Animator>();//Animator取得
        hp = Enemy_Status.max_hp + (add_count * count_game_state);//体力を設定
        damage = Enemy_Status.attack_damage + (add_count * count_game_state);//攻撃力設定
    }
    void Update()
    {
        Enmey_State(ai_style);//エネミーの行動管理
        b_time++;
        //体力が1以下ならアニメーション更新
        if(hp < 1)
        {
            hp = 0;//体力が0以下にならないようにする
            act_shot = false;
            Destroy(bullet_point);//銃弾発射位置削除
            animator.SetBool("Death", true);//アニメーションを設定
        }
    }
    //エネミーの行動処理
    private void Enmey_State(Enemy_Ai_Style style)
    {
        if (style == Enemy_Ai_Style.Idle)
        {
            //停止状態(何もしない


            //弾丸発射が許可されている、かつ、体力が１以上、b_timeが間隔時間より大きくなったら
            if(act_shot == true && bullet_per_shot < b_time && hp > 0)
            {
                //弾を発射
                Shot();
                b_time = 0;//時間初期化
            }
        }
    }
    //Playerが範囲内に入ったらその方向を向く
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player")==true)
        {
            //敵からプレイヤーまでのベクトル作成
            e_vec = gameObject.transform.position - collider.gameObject.transform.position;
            transform.rotation = Quaternion.LookRotation(e_vec);//角度をdirectionまで変更
            act_shot = true;//弾丸発射を許可
        }
    }
    //Bulletタグに当たったら体力を減らす
    private void OnCollisionEnter(Collision collision)
    {
        //Bulletとの当たり判定
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameObject player = GameObject.Find("Animate_Player");
            p_cnt = player.GetComponent<PlayerController>();
            hp = Damage_Cal(p_cnt.attack_power, hp);
            collision.gameObject.IsDestroyed();
            Debug.Log("当たった  体力" + hp);//デバッグ用
        }
    }
    //アニメーション中でこの関数を呼んでオブジェクトを消す
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    private void Shot()
    {
        //弾のプレハブを生成
        GameObject bullet = Instantiate(bullet_prefab, gameObject.transform.position, Quaternion.identity);
        bullet.transform.position = bullet_point.transform.position;//ポジションをポイントへ移動
        bullet.transform.rotation = Quaternion.LookRotation(e_vec);//角度をdirectionまで変更
        //RigidBodyにbullet_force分の力を加える
        bullet.GetComponent<Rigidbody>().AddForce(-e_vec.normalized * bullet_force, ForceMode.Impulse);
    }
}