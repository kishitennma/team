using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Status : MonoBehaviour
{
    [Header("プレイヤーのステータス")]
    public static int Player_Attack_Damage;//プレイヤーの攻撃力を保持
    public static int Player_HP=100;//プレイヤーの体力

    private void Start()
    {
        Player_HP = 100;
    }

    private void FixedUpdate()
    {
        if (Player_HP < 1)
        {
            SceneManager.LoadScene("GameTitle");//仮でいったんタイトルに戻る//ゲームオーバー画面
        }
    }
}