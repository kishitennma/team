using UnityEngine;

public class Damage_Calclate : MonoBehaviour
{
    //���݂̓V���v���ɍU���͕��̗͂����炷�̂�
    public int Damage_Cal(int damage, int hp)
    {
        hp = hp - damage;
        if (hp < 0)
            hp = 0;
        return hp;
    }
}