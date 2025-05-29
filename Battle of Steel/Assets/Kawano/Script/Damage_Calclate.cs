using UnityEngine;

public class Damage_Calclate : MonoBehaviour
{
    //Œ»İ‚ÍƒVƒ“ƒvƒ‹‚ÉUŒ‚—Í•ª‘Ì—Í‚ğŒ¸‚ç‚·‚Ì‚İ
    public int Damage_Cal(int damage, int hp)
    {
        hp = hp - damage;
        //‘Ì—Í‚ª0ˆÈ‰º‚É‚È‚ç‚È‚¢‚æ‚¤‚É‚·‚é
        if (hp < 0)
            hp = 0;
        return hp;
    }
}

