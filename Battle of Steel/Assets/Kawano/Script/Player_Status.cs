using UnityEngine;

public class Player_Status : MonoBehaviour
{
    [Header("�v���C���[�̃X�e�[�^�X")]
    public static int Player_Attack_Damage;//�v���C���[�̍U���͂�ێ�
    public static int Player_HP;//�v���C���[�̗̑�

    private void Start()
    {
        Player_HP = 100;
    }
}