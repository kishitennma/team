using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Status : MonoBehaviour
{
    [Header("プレイヤーのステータス")]
    public static int Player_Attack_Damage;//プレイヤーの攻撃力を保持(変動あり)
    public static int Player_Put_Attack_Damage;//プレイヤーの攻撃力を保持(変動無し)
    public static int Player_HP=100;//プレイヤーの体力
    public GameObject weapon_image;//攻撃力増加イメージ
    public GameObject guide_image;//操作方法画像

    private bool guide_flag;//ガイド表示フラグ
    private void Start()
    {
        Player_HP = 100;
        weapon_image.SetActive(false);//攻撃力が上昇中に表示させる画像
        guide_image.SetActive(false);//操作方法のオブジェクト
    }

    private void Update()
    {
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

        //リロード中
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
}