using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Status : MonoBehaviour
{
    [Header("プレイヤーのステータス")]
    public static int Player_Attack_Damage;//プレイヤーの攻撃力を保持(変動あり)
    public static int Player_HP=100;//プレイヤーの体力

    private void Start()
    {
        Player_HP = 100;
        Cursor.visible = false;//カーソル非表示
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
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