using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Status : MonoBehaviour
{
    [Header("プレイヤーのステータス")]
    public static int Player_Attack_Damage;//プレイヤーの攻撃力を保持(変動あり)
    public static int Player_Put_Attack_Damage;//プレイヤーの攻撃力を保持(変動無し)
    public static int Player_HP=100;//プレイヤーの体力
    public static int Player_Max_HP = 100;
    [Header("イメージ")]
    public GameObject weapon_image;//攻撃力増加イメージ
    public GameObject guide_image;//操作方法画像
    public Text heal_text;
    [Header("エフェクト")]
    public GameObject Player_Exp;//プレイヤーの死亡時の爆発エフェクト
    private bool guide_flag = true;//ガイド表示フラグ
    public static int weapons_f;//メイン武器
    public static int weapons_s;//サブ武器
    public static bool damaged;//ダメージフラグ
    private float death_timer = 0;//死ぬまでの時間
    public static float hitstop = 1.0f;//ヒットストップ
    private float hitstop_timer = 0f;//ヒットストップタイマー
    const float MAX_HIT_TIMER = 5f;//ヒットストップの最大時間
    private bool hitstop_flag = false;//ヒットストップフラグ
    private GameObject Player;//プレイヤーオブジェクト
    public GameObject Player＿Camera;//カメラオブジェクト(プレイヤーを入れる/区別化)

    [SerializeField] Image OnDamaged;
    //回復用変数
    public GameObject Heal_Effect;//回復エフェクト
    private int heal_timer = 0;//回復エフェクト発生時間
    private bool heal_f = false;//回復エフェクト表示フラグ
    private Vector3 C_Start_Pos;
    public AudioSource heal_sound;
    //定数
    const int HEAL_VALUE = 50;//回復力
    const float MAX_SHAKE = 0.7f;//最大振動幅
    public static int MAX_HEAL_COUNTS = 3;
    public static int heal_count = MAX_HEAL_COUNTS;//回復できる回数


    private void Start()
    {
        Player = GameObject.Find("Player");
        Heal_Effect.SetActive(false);
        Player_HP = Player_Max_HP;//体力を最大体力と同じにする
        weapon_image.SetActive(false);//攻撃力が上昇中に表示させる画像
        OnDamaged.color = Color.clear;
        death_timer = 0;
        heal_count = MAX_HEAL_COUNTS;
        EnemyBulletAction.hit_counts = 0;//被弾回数を初期化
        guide_flag = false;
    }

    private void Update()
    {
        heal_text.text = "回復 :" + heal_count + "/" + MAX_HEAL_COUNTS;
        if(heal_count == 0)
        {
            heal_text.color = Color.red;
        }
        else
        {
            heal_text.color = Color.green;
        }

            OnDamaged.color = Color.Lerp(OnDamaged.color, Color.clear, Time.deltaTime);
        if (damaged == true && hitstop_flag == false)
        {
            //ヒットストップを有効にする
            C_Start_Pos = Player＿Camera.transform.position;//カメラの位置を記録
            hitstop_flag = true;
            Damage_Reaction();
        }
        //ヒットストップ
        if(hitstop_flag == true)
        {
            hitstop_timer += 0.1f;

            //設定した時間中は移動できなくする
            if(hitstop_timer < MAX_HIT_TIMER)
            {

                //カメラのランダム座標を取得
                float C_offset_X = Random.Range(-MAX_SHAKE, MAX_SHAKE);
                float C_offset_Y = Random.Range(-MAX_SHAKE, MAX_SHAKE);

                Player＿Camera.transform.position = 
                    new Vector3(C_Start_Pos.x + C_offset_X,C_Start_Pos.y + C_offset_Y,C_Start_Pos.z);

                hitstop = 0f;//移動速度に0を掛けて強制的に移動しなくする
            }
            else
            {
                //値の初期化
                hitstop = 1f;
                Player＿Camera.transform.position = C_Start_Pos;
                hitstop_timer = 0f;
                hitstop_flag = false;
            }
        }

        //回復ボタン 
        if (Input.GetKeyDown(KeyCode.R) && heal_count > 0)
        {
            //体力が最大値と同等の時
            if(Player_HP >= Player_Max_HP)
            {
                Debug.Log("体力はマックスです");
            }
            else if(Player_HP < Player_Max_HP)
            {
                heal_count--;//使用回数を減らす
                Heal_Effect.SetActive(true);
                heal_sound.Play();
                heal_f = true;
                Heal();
            }
        }
        if(heal_f)
        {
            heal_timer++;

            if(heal_timer >= 300)
            {
                Heal_Effect.SetActive(false);
                heal_timer = 0;
                heal_f = false;
            }
        }


        //操作ガイド表示
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(guide_flag == true)
                guide_flag = false;
            else
                guide_flag = true;
        }

        if (guide_flag)
            guide_image.SetActive(true);
        else
            guide_image.SetActive(false);

    }

    private void FixedUpdate()
    {
        //攻撃力が基本値よりも大きくなったら画像を表示
        if(Player_Put_Attack_Damage < Player_Attack_Damage && Input.GetKey(KeyCode.Q) == false)
        {
            weapon_image.SetActive(true);
        }
        else
        {
            weapon_image.SetActive(false);
        }
        if (Player_HP < 1)
        {
            //爆発エフェクト発生
            GameObject explosive = Instantiate(Player_Exp, Player.transform.position, Quaternion.identity);
            death_timer += 0.1f;

            if(death_timer > 5f)
            {
                SceneManager.LoadScene("GameOver");//仮でいったんタイトルに戻る//ゲームオーバー画面
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        //自害デバッグ用
        if (Input.GetKeyDown(KeyCode.L))
        {
            Player_HP = 0;
        }
    }
    //体力回復関数
    private void Heal()
    {
        //回復を実行
        heal_timer++;
        Player_HP += HEAL_VALUE;//体力を数値分回復

        //最大体力をはみ出さないようにする
        if (Player_HP >= Player_Max_HP)
        {
            Player_HP = Player_Max_HP;
        }
    }

    void Damage_Reaction()
    {
        //ダメージを受けたら画面を赤くする
        OnDamaged.color = new Color(0.7f, 0, 0, 0.65f);

        damaged = false;
    }
}