using UnityEngine;

public class Damage_Calclate : MonoBehaviour
{
    //現在はシンプルに攻撃力分体力を減らすのみ
    public int Damage_Cal(int damage, int hp)
    {
        hp = hp - damage;
        //体力が0以下にならないようにする
        if (hp < 0)
            hp = 0;
        return hp;
    }
}

