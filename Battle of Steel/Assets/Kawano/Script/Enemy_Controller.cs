using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Enemy_Ai_Style
{
    //敵のAIリスト
    Idle,//停止
    Boss_Idle,//ボス（停止）
    Boss_Fast,//(散弾弾発射ボス)
    Boss_Second,
}
public enum Enemy_ID
{
    //敵のIDリスト
    Idle_Robot,//ロボット(停止)
    Idle_Fast_Robot,//ロボット(弾丸高速発射)
    Boss_01,
    Boss_02,
    Boss_03,
}
public class Enemy_Status
{
    public int max_hp;//最大体力
    public int attack_damage;//攻撃力
    public float bullet_per_shot;//発射間隔
    public  Enemy_Ai_Style style;//AIスタイル
    public Enemy_Status(int set_hp,int set_damage,Enemy_Ai_Style set_style,float per_shot)
    {
        //各ステータスを入力
        max_hp = set_hp;
        attack_damage = set_damage;
        bullet_per_shot = per_shot;
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
    [SerializeField] GameObject Explosive_unit;//爆発エフェクト
    public Dictionary<Enemy_ID, Enemy_Status> enemy_index = new()
    {
        //ここに敵のステータスを入力(体力、攻撃力、AI,発射レート)
        {Enemy_ID.Idle_Robot,      new Enemy_Status( 30, 5,Enemy_Ai_Style.Idle,      350f) },
        {Enemy_ID.Idle_Fast_Robot ,new Enemy_Status( 45, 5,Enemy_Ai_Style.Idle,      150f) },
        {Enemy_ID.Boss_01,         new Enemy_Status(100,15,Enemy_Ai_Style.Boss_Idle, 300f) },
        {Enemy_ID.Boss_02,         new Enemy_Status(250, 7,Enemy_Ai_Style.Boss_Fast, 260f) },
        {Enemy_ID.Boss_03,         new Enemy_Status(300, 8,Enemy_Ai_Style.Boss_Second,200f) },
    };

    //変数
    public int count_game_state = 0;//ゲーム回数
    private Enemy_Status e_status;//エネミーステータス
    private Animator animator;//アニメーター
    private int hp = 0;//現在の体力
    private int b_time = 0;//弾丸発射時間
    private int damage = 0;//攻撃力
    private int add_count = 10;//加算値
    private float bullet_per_shot;//発射間隔
    private bool act_shot = false;//弾丸発射許可値
    private Vector3 e_vec;//ベクトル
    private Enemy_Ai_Style ai_style;//AIスタイル
    
    //ボス用
    private int boss_act_count;//ボスのアクションカウント
    private Quaternion tpr_rotate_bullets;//弾の初期位置を保存
    private int act_time;//Boss＿Second用時間計測
    private int shot_count;//Boss_Second用発射カウント
    void Start()
    {
        //エネミーのインデックスを取得
        e_status = enemy_index[id];
        act_shot = false;
        animator = GetComponent<Animator>();//Animator取得
        bullet_per_shot = e_status.bullet_per_shot;//弾丸の発射間隔を設定
        hp = e_status.max_hp + (add_count * count_game_state);//体力を設定
        damage = e_status.attack_damage + (add_count * count_game_state);//攻撃力設定
        ai_style = e_status.style;//AIスタイルを設定
    }
    void Update()
    {
        Enmey_State(ai_style);//エネミーの行動管理
        b_time++;
        //体力が1以下ならアニメーション更新
        if (hp < 1)
        {
            GameObject explosive = Instantiate(Explosive_unit, gameObject.transform.position, Quaternion.identity);
            if (animator != null)
            {
                animator.SetBool("Death", true);//アニメーションを設定
            }
            hp = 0;//体力が0以下にならないようにする
            act_shot = false;
            Destroy(bullet_point);//銃弾発射位置削除
            DestroyObject();//オブジェクト
        }
    }
    //エネミーの行動処理
    private void Enmey_State(Enemy_Ai_Style style)
    {
        //停止状態(何もしない
        if (style == Enemy_Ai_Style.Idle)
        {
            //弾丸発射が許可されている、かつ、体力が１以上、b_timeが間隔時間より大きくなったら
            if(act_shot == true && b_time > bullet_per_shot && hp > 0)
            {
                //弾を発射
                Shot();
                b_time = 0;//時間初期化
            }
        }
        //ボス（停止するボス）
        if(style == Enemy_Ai_Style.Boss_Idle)
        {    
            if (act_shot == true && b_time > bullet_per_shot && hp > 0 && boss_act_count > 1)
            {

                Way_Shot(1,20);//-1,0,1の三回、20度ずつ
                boss_act_count = 0;
                b_time = 0;
            }
            else if (act_shot == true && b_time > bullet_per_shot && hp > 0)
            {
                //弾丸発射が許可されている、かつ、体力が１以上、b_timeが間隔時間より大きくなったら

                //弾を発射
                Shot();
                boss_act_count++;
                b_time = 0;//時間初期化
            }
        }
        //弾のパターンが変わるボス
        if(style == Enemy_Ai_Style.Boss_Fast)
        {
            if(act_shot == true && bullet_per_shot < b_time && hp > 0)
            {
                switch (boss_act_count)
                {
                    case 0: Way_Shot(2,2); break;
                    case 1: Way_Shot(3, 9); break;
                    case 2: Way_Shot(5, 14); break;
                }
                switch(boss_act_count)
                {
                    case 0:boss_act_count++;break;
                    case 1:boss_act_count++;break;
                    case 2:boss_act_count-=2;break;
                }

                //発射カウントで放つ弾の数を変更する
                b_time = 0;
            }
        }
        if(style == Enemy_Ai_Style.Boss_Second)
        {
            act_time++;
            if (act_shot == true && bullet_per_shot < b_time && hp > 0)
            {
                switch (boss_act_count)
                {
                    case 0: Mul_Shot(8, 10);break;
                    case 1: Way_Shot(3, 4); boss_act_count++; break;
                    case 2: Way_Shot_Left(12, 87); boss_act_count-= 2; break;
                }
                //発射カウントで放つ弾の数を変更する
                b_time = 0;
            }
        }
    }
    //Playerが範囲内に入ったらその方向を向く
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //敵からプレイヤーまでのベクトル作成
            e_vec = gameObject.transform.position - collider.gameObject.transform.position;
            transform.rotation = Quaternion.LookRotation(e_vec);//角度をdirectionまで変更
            act_shot = true;//弾丸発射
        }
    }
    //Bulletタグに当たったら体力を減らす
    private void OnCollisionEnter(Collision collision)
    {
        //Bulletとの当たり判定
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp = Damage_Cal(Player_Status.Player_Attack_Damage, hp);
            collision.gameObject.IsDestroyed();
            Debug.Log("当たった  体力" + hp);//デバッグ用
        }
    }
    //アニメーション中でこの関数を呼んでオブジェクトを消す
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    //通常の弾丸発射スクリプト
    private void Shot()
    {
        //弾のプレハブを生成
        GameObject bullet = Instantiate(bullet_prefab, gameObject.transform.position, Quaternion.identity);
        //弾丸に攻撃力の情報を渡しておく
        EnemyBulletAction e_bullet_act = bullet.GetComponent<EnemyBulletAction>();
        e_bullet_act.attack_damage = damage;//攻撃力を渡す
        bullet.transform.position = bullet_point.transform.position;//ポジションをポイントへ移動
        bullet.transform.rotation = Quaternion.LookRotation(e_vec);//角度をdirectionまで変更

        //RigidBodyにbullet_force分の力を加える
        bullet.GetComponent<Rigidbody>().AddForce(-e_vec.normalized * bullet_force, ForceMode.Impulse);
    }
    //弾丸を扇状に決められた回数分左右2方向に発射
    private void Way_Shot(int counts,int radius)
    {

        for(int i = 0; i <= counts; i++)
        {
            Debug.Log(i + "回目");
            if(i== 0)
            {
                //弾のプレハブを生成
                GameObject bullet = Instantiate(bullet_prefab, gameObject.transform.position, Quaternion.identity);
                bullet.transform.position = bullet_point.transform.position;//ポジションをポイントへ移動
                bullet.transform.rotation = Quaternion.LookRotation(e_vec);//角度をdirectionまで変更
                //弾丸に攻撃力の情報を渡しておく
                EnemyBulletAction e_bulet_act = bullet.GetComponent<EnemyBulletAction>();
                e_bulet_act.attack_damage = damage;//攻撃力を渡す

                bullet.GetComponent<Rigidbody>().AddForce(-e_vec.normalized * bullet_force, ForceMode.Impulse);
                tpr_rotate_bullets = bullet.transform.rotation;
            }
            if(i>0)
            {
                
                GameObject bullet_r = Instantiate(bullet_prefab, gameObject.transform.position, Quaternion.identity);
                bullet_r.transform.position = bullet_point.transform.position;
                //弾丸の初期角度を入れる
                Quaternion qua_r = tpr_rotate_bullets;
                //角度をi*rad分変更
                qua_r.y = tpr_rotate_bullets.y - (i * radius);
                //弾丸の角度を変更
                bullet_r.transform.rotation = qua_r;
                //角度を計算
                Vector3 e_vec_r = Quaternion.AngleAxis(-radius*i, Vector3.up) * e_vec;
                //弾丸の発射角度を変更
                bullet_r.GetComponent<Rigidbody>().AddForce(-e_vec_r.normalized * bullet_force, ForceMode.Impulse);
                //弾丸に攻撃力の情報を渡しておく
                EnemyBulletAction e_bulet_act_r = bullet_r.GetComponent<EnemyBulletAction>();
                e_bulet_act_r.attack_damage = damage;//攻撃力を渡す

                //右側
                GameObject bullet_l = Instantiate(bullet_prefab, gameObject.transform.position, Quaternion.identity);
                bullet_l.transform.position = bullet_point.transform.position;
                Quaternion qua_l = tpr_rotate_bullets;
                qua_l.y = tpr_rotate_bullets.y + (i * radius);
                bullet_l.transform.rotation = qua_l;
                Vector3 e_vec_l = Quaternion.AngleAxis(radius * i, Vector3.up) * e_vec;
                bullet_l.GetComponent<Rigidbody>().AddForce(-e_vec_l.normalized * bullet_force, ForceMode.Impulse);
                //弾丸に攻撃力の情報を渡しておく
                EnemyBulletAction e_bulet_act_l = bullet_l.GetComponent<EnemyBulletAction>();
                e_bulet_act_l.attack_damage = damage;//攻撃力を渡す
            }
        }
    }
    //左周りで角度を変更して発射
    private void Way_Shot_Left(int counts,int radius)
    {
        for (int i = 0; i <= counts; i++)
        {
            Debug.Log(i + "回目");
            if (i == 0)
            {
                //弾のプレハブを生成
                GameObject bullet = Instantiate(bullet_prefab, gameObject.transform.position, Quaternion.identity);
                bullet.transform.position = bullet_point.transform.position;//ポジションをポイントへ移動
                bullet.transform.rotation = Quaternion.LookRotation(e_vec);//角度をdirectionまで変更
                //弾丸に攻撃力の情報を渡しておく
                EnemyBulletAction e_bulet_act = bullet.GetComponent<EnemyBulletAction>();
                e_bulet_act.attack_damage = damage;//攻撃力を渡す

                bullet.GetComponent<Rigidbody>().AddForce(-e_vec.normalized * bullet_force, ForceMode.Impulse);
                tpr_rotate_bullets = bullet.transform.rotation;
            }
            if (i > 0)
            {

                GameObject bullet_r = Instantiate(bullet_prefab, gameObject.transform.position, Quaternion.identity);
                bullet_r.transform.position = bullet_point.transform.position;
                Quaternion qua_r = Quaternion.AngleAxis(-radius * i, Vector3.up);
                bullet_r.transform.rotation = qua_r;                
                //角度を計算
                Vector3 e_vec_r = Quaternion.AngleAxis(-radius * i, Vector3.up) * e_vec;
                //弾丸の発射角度を変更
                bullet_r.GetComponent<Rigidbody>().AddForce(-e_vec_r.normalized * bullet_force, ForceMode.Impulse);
                //弾丸に攻撃力の情報を渡しておく
                EnemyBulletAction e_bulet_act_r = bullet_r.GetComponent<EnemyBulletAction>();
                e_bulet_act_r.attack_damage = damage;//攻撃力を渡す
            }
        }
    }
    //弾丸を指定回数分指定時間間隔で発射
    private void Mul_Shot(int counts,int time)
    {
        if (shot_count < counts && act_time > time)
        {
            bullet_per_shot = time;
            Shot();
            shot_count++;
            act_time = 0;
            if (shot_count == counts)
            {
                bullet_per_shot = e_status.bullet_per_shot;
                shot_count = 0;
                boss_act_count++;
            }
        }
    }
}