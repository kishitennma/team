using UnityEngine;

public class Player_Status : MonoBehaviour
{
    [Header("プレイヤーのステータス")]
    public static int Player_Attack_Damage;//プレイヤーの攻撃力を保持
    public static int Player_HP;//プレイヤーの体力

    private void Start()
    {
        Player_HP = 100;
    }
}