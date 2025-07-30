using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Cnt : Damage_Calclate
{
    [Header("敵のID")]
    [SerializeField] Enemy_ID id;//敵のID
    [Header("弾のプレハブ")]
    [SerializeField] GameObject[] bullet_prefab;//弾のプレハブ
    [SerializeField] GameObject bullet_point;//弾の発射位置
    [SerializeField] GameObject Explosive_unit;//爆発エフェクト
    public Dictionary<Enemy_ID, Enemy_Status> enemy_index = new()
    {
        //ここに敵のステータスを入力(体力、攻撃力、AI,発射レート)
        {Enemy_ID.Last_Boss,new Enemy_Status(800, 3,Enemy_Ai_Style.Last_Boss,210f) },//ラストボス
    };

    //変数
    public AudioSource a_source;
    public Material boss_mat;//ボスのマテリアル
    public int count_game_state = 0;//ゲーム回数
    private Enemy_Status e_status;//エネミーステータス
    private Animator animator;//アニメーター
    private int hp = 0;//現在の体力
    private int b_time = 0;//弾丸発射時間
    private int damage = 0;//攻撃力
    private int add_count = 10;//加算値
    private float bullet_per_shot;//発射間隔
    private bool act_shot = false;//弾丸発射許可値
    private float bullet_force;//弾丸の発射速度
    private Vector3 e_vec;//ベクトル
    public AudioSource Enemy_Die;

    //ボス用
    private int boss_act_count;//ボスのアクションカウント
    private Quaternion tpr_rotate_bullets;//弾の初期位置を保存
    private int act_time;//Boss＿Second用時間計測
    private int shot_count;//Boss_Second用発射カウント
    private int shot_count_second;//Boss_Second用発射カウント
    private int shot_count_vertex;
    private bool stop_rand = false;
    void Start()
    {
        if (boss_mat == null)
            boss_mat = gameObject.GetComponent<Material>();
        boss_mat.color = Color.black;
        //エネミーのインデックスを取得
        e_status = enemy_index[id];
        act_shot = false;
        animator = GetComponent<Animator>();//Animator取得
        bullet_per_shot = e_status.bullet_per_shot;//弾丸の発射間隔を設定
        hp = e_status.max_hp + (add_count * count_game_state);//体力を設定
        damage = e_status.attack_damage + (add_count * count_game_state);//攻撃力設定
    }
    void Update()
    {
        Enmey_State();//エネミーの行動管理
        b_time++;
        //体力が1以下ならアニメーション更新
        if (hp < 1)
        {
            GameObject explosive = Instantiate(Explosive_unit, bullet_point.transform.position, Quaternion.identity);
            if (animator != null)
            {
                animator.SetBool("Death", true);//アニメーションを設定
            }
            hp = 0;//体力が0以下にならないようにする

            Enemy_Die.Play();
            act_shot = false;
            Enemy_Manager.last_boss_spawnwd = true;
            Destroy(bullet_point);//銃弾発射位置削除
            DestroyObject();//オブジェクト
        }
    }
    //エネミーの行動処理
    private void Enmey_State()
    {
        act_time++;
        if (act_shot == true && bullet_per_shot < b_time && hp > 0)
        {
            if(!stop_rand)
            {
                boss_act_count = Random.Range(1, 4);
                C_Color(boss_act_count, boss_mat);//カラーをカウント事にマテリアルを変える
            }
            switch (boss_act_count)
            {
                case 1:
                    {
                        bullet_force = 60.0f;
                        Way_Shot(18, 10, false,3,40f);
                        b_time = 0;
                    }
                    break;
                case 2:
                    {
                        bullet_force = 20.0f;
                        Homing_Shot(5, 5);
                        b_time = 0;
                    }
                    break;
                case 3:
                    {
                        bullet_force = 180f;
                        Mul_Shot(10, 7);
                        b_time = 0;
                    }
                    break;
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
    private void OnTriggerExit(Collider other)
    {
        act_shot = false;//弾丸を発射しなくする
        shot_count = 0;
        shot_count_second = 0;//数値初期化
        Debug.Log("対象を見失った");
    }
    //Bulletタグに当たったら体力を減らす
    private void OnCollisionEnter(Collision collision)
    {
        //Bulletとの当たり判定
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp = Damage_Cal(Player_Status.Player_Attack_Damage, hp);
            collision.gameObject.IsDestroyed();
        }
    }
    //アニメーション中でこの関数を呼んでオブジェクトを消す
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    //弾丸を扇状に決められた回数分左右2方向に発射
    private void Way_Shot(int counts, int radius, bool derct, int r_count, float time)
    {
        if (shot_count_vertex <= r_count)
        {
            shot_count_vertex++;//行動回数を増加
            bullet_per_shot = time;
            a_source.Play();//効果音をつける
            for (int i = 0; i <= counts; i++)
            {
                //初回発射を基準として左右に扇状に展開
                if (i == 0)
                {
                    //弾のプレハブを生成
                    GameObject bullet = Instantiate(bullet_prefab[0], gameObject.transform.position, Quaternion.identity);
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
                    //横軸
                    if (!derct)
                    {
                        GameObject bullet_r = Instantiate(bullet_prefab[0], gameObject.transform.position, Quaternion.identity);
                        bullet_r.transform.position = bullet_point.transform.position;
                        //弾丸の初期角度を入れる
                        Quaternion qua_r = tpr_rotate_bullets;
                        //角度をi*rad分変更
                        qua_r.y = tpr_rotate_bullets.y - (i * radius);
                        //弾丸の角度を変更
                        bullet_r.transform.rotation = qua_r;
                        //角度を計算
                        Vector3 e_vec_r = Quaternion.AngleAxis(-radius * i, Vector3.up) * e_vec;
                        //弾丸の発射角度を変更
                        bullet_r.GetComponent<Rigidbody>().AddForce(-e_vec_r.normalized * bullet_force, ForceMode.Impulse);
                        //弾丸に攻撃力の情報を渡しておく
                        EnemyBulletAction e_bulet_act_r = bullet_r.GetComponent<EnemyBulletAction>();
                        e_bulet_act_r.attack_damage = damage;//攻撃力を渡す
                                                             //右側
                        GameObject bullet_l = Instantiate(bullet_prefab[0], gameObject.transform.position, Quaternion.identity);
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
                    else
                    {
                        GameObject bullet_r = Instantiate(bullet_prefab[0], gameObject.transform.position, Quaternion.identity);
                        bullet_r.transform.position = bullet_point.transform.position;
                        //弾丸の初期角度を入れる
                        Quaternion qua_r = tpr_rotate_bullets;
                        //角度をi*rad分変更
                        qua_r.y = tpr_rotate_bullets.x - (i * radius);
                        //弾丸の角度を変更
                        bullet_r.transform.rotation = qua_r;
                        //角度を計算
                        Vector3 e_vec_r = Quaternion.AngleAxis(-radius * i, Vector3.left) * e_vec;
                        //弾丸の発射角度を変更
                        bullet_r.GetComponent<Rigidbody>().AddForce(-e_vec_r.normalized * bullet_force, ForceMode.Impulse);
                        //弾丸に攻撃力の情報を渡しておく
                        EnemyBulletAction e_bulet_act_r = bullet_r.GetComponent<EnemyBulletAction>();
                        e_bulet_act_r.attack_damage = damage;//攻撃力を渡す
                                                             //右側
                        GameObject bullet_l = Instantiate(bullet_prefab[0], gameObject.transform.position, Quaternion.identity);
                        bullet_l.transform.position = bullet_point.transform.position;
                        Quaternion qua_l = tpr_rotate_bullets;
                        qua_l.y = tpr_rotate_bullets.x + (i * radius);
                        bullet_l.transform.rotation = qua_l;
                        Vector3 e_vec_l = Quaternion.AngleAxis(radius * i, Vector3.left) * e_vec;
                        bullet_l.GetComponent<Rigidbody>().AddForce(-e_vec_l.normalized * bullet_force, ForceMode.Impulse);
                        //弾丸に攻撃力の情報を渡しておく
                        EnemyBulletAction e_bulet_act_l = bullet_l.GetComponent<EnemyBulletAction>();
                        e_bulet_act_l.attack_damage = damage;//攻撃力を渡す

                    }
                }
            }
            if (shot_count_vertex >= r_count)
            {
                bullet_per_shot = e_status.bullet_per_shot;
                shot_count_vertex = 0;

                switch (boss_act_count)
                {
                    case 0:
                    case 1: boss_act_count++; break;
                    case 2: boss_act_count = 0; break;
                }
            }

        }

    }
    //弾丸を指定回数分指定時間間隔で発射
    private void Mul_Shot(int counts, int time)
    {
        stop_rand = true;
        if (shot_count_second <= counts && act_time > time)
        {
            a_source.Play();
            bullet_per_shot = time;

            a_source.Play();
            //弾のプレハブを生成
            GameObject bullet = Instantiate(bullet_prefab[2], bullet_point.transform.position, Quaternion.identity);
            //弾丸に攻撃力の情報を渡しておく
            EnemyBulletAction e_bullet_act = bullet.GetComponent<EnemyBulletAction>();
            e_bullet_act.attack_damage = damage;//攻撃力を渡す
            bullet.transform.position = bullet_point.transform.position;//ポジションをポイントへ移動
            bullet.transform.rotation = Quaternion.LookRotation(e_vec);//角度をdirectionまで変更

            //RigidBodyにbullet_force分の力を加える
            bullet.GetComponent<Rigidbody>().AddForce(-e_vec.normalized * bullet_force, ForceMode.Impulse);

            shot_count_second++;
            act_time = 0;
        }
        if (shot_count_second >= counts)
        {
            stop_rand = false;
            bullet_per_shot = e_status.bullet_per_shot;
            shot_count_second = 0; 
        }

    }
    private void Homing_Shot(int counts, int time)
    {
        stop_rand = true;
        if (shot_count <= counts && act_time > time)
        {
            a_source.Play();
            bullet_per_shot = time;
            //弾のプレハブを生成
            GameObject bullet = Instantiate(bullet_prefab[1], bullet_point.transform.position, Quaternion.identity);
            //弾丸に攻撃力の情報を渡しておく
            EnemyBulletAction e_bullet_act = bullet.GetComponent<EnemyBulletAction>();
            e_bullet_act.attack_damage = damage;//攻撃力を渡す
            bullet.transform.position = bullet_point.transform.position;//ポジションをポイントへ移動
            bullet.transform.rotation = Quaternion.LookRotation(-e_vec);//角度をdirectionまで変更

            //RigidBodyにbullet_force分の力を加える
            bullet.GetComponent<Rigidbody>().AddForce(-e_vec.normalized * bullet_force, ForceMode.Impulse);
            shot_count++;
            act_time = 0;
        }
        if (shot_count >= counts)
        {
            stop_rand = false;
            bullet_per_shot = e_status.bullet_per_shot;
            shot_count = 0; 
        }
    }

    private void C_Color(int c,Material mat)
    {
        switch (c)
        {
            case 0:
                mat.color = Color.black;
                break;
            case 1:
                mat.color = Color.green;
                break;
            case 2:
                mat.color = Color.yellow;
                break;
            case 3:
                mat.color = Color.blue;
                break;
        }
    }

}