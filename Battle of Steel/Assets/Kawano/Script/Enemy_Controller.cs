using System.Collections.Generic;
using UnityEngine;

public enum Enemy_Ai_Style
{
    //�G��AI���X�g
    Idle,//��~

}
public enum Enemy_ID
{
    //�G��ID���X�g
    Idle_Robot,//���{�b�g(��~)

}
public class Enemy_Status
{
    public static int max_hp;//�ő�̗�
    public static int attack_damage;//�U����
    public Enemy_Ai_Style style;//AI�X�^�C��

    public Enemy_Status(int set_hp,int set_damage,Enemy_Ai_Style set_style)
    {
        //�e�X�e�[�^�X�����
        max_hp = set_hp;
        attack_damage = set_damage;
        style = set_style;
    }
}

public class Enemy_Controller : Damage_Calclate
{
    [Header("�G��ID")]
    [SerializeField] Enemy_ID id;

    //�����ɓG�̃X�e�[�^�X�����(�̗́A�U���́AAI)
    public Dictionary<Enemy_ID, Enemy_Status> enemy_index = new()
    {
        {Enemy_ID.Idle_Robot,new Enemy_Status(30,5,Enemy_Ai_Style.Idle)},
    };

    //�ϐ�
    Damage_Calclate calc;
    private int hp;//���݂̗̑�
    private Enemy_Ai_Style ai_style;//AI�X�^�C��
    void Start()
    {
        hp = Enemy_Status.max_hp;//�̗͂�ݒ�
    }
    void Update()
    {
        Enmey_State(ai_style);//�G�l�~�[�̍s���Ǘ�

        //�̗͂�0�Ȃ����
        if(hp < 0)
        {
            Destroy(this);
        }
    }
    //�G�l�~�[�̍s������
    private void Enmey_State(Enemy_Ai_Style style)
    {
        if (style == Enemy_Ai_Style.Idle)
        {

        }
        else { }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        Debug.Log("��������");
    }

}