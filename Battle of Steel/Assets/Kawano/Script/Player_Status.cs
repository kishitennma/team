using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Status : MonoBehaviour
{
    [Header("�v���C���[�̃X�e�[�^�X")]
    public static int Player_Attack_Damage;//�v���C���[�̍U���͂�ێ�
    public static int Player_HP=100;//�v���C���[�̗̑�

    private void Start()
    {
        Player_HP = 100;
    }

    private void FixedUpdate()
    {
        if (Player_HP < 1)
        {
            SceneManager.LoadScene("GameTitle");//���ł�������^�C�g���ɖ߂�//�Q�[���I�[�o�[���
        }
    }
}