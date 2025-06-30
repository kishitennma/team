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
    public GameObject Weapon_image;
    private void Start()
    {
        Player_HP = 100;
        Weapon_image.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(Player_Put_Attack_Damage < Player_Attack_Damage)
        {
            Weapon_image.SetActive(true);
        }
        else
        {
            Weapon_image?.SetActive(false);
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