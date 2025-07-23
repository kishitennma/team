using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool guide_flag;//ガイド表示フラグ
    public static int[] weapons;

    //回復用変数
    public int heal_count = 3;//回復できる回数
    public GameObject Heal_Effect;//回復エフェクト
    private int heal_timer = 0;//回復エフェクト発生時間
    private bool heal_f = false;//回復エフェクト表示フラグ
    //回復定数
    const int HEAL_VALUE = 50;//回復力


    private void Start()
    {
        Heal_Effect.SetActive(false);
        Player_HP = Player_Max_HP;//体力を最大体力と同じにする
        weapon_image.SetActive(false);//攻撃力が上昇中に表示させる画像
        guide_image.SetActive(false);//操作方法のオブジェクト
    }

    private void Update()
    {
        //回復ボタン 
        if(Input.GetKeyDown(KeyCode.R) && heal_count > 0)
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
            if(guide_flag)
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
            SceneManager.LoadScene("GameOver");//仮でいったんタイトルに戻る//ゲームオーバー画面
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Player_HP = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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
}