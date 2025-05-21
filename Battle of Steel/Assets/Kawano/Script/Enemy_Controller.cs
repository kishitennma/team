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
    [SerializeField] Enemy_ID id;//�G��ID
    [SerializeField] Collision find_player_collision;//�v���C���[��T�����߂̂����蔻��
    [Header("�C���X�y�N�^�m�F�p")]
    [SerializeField] int set_hp;//�C���X�y�N�^�m�F�p
    [Header("�e�ۃv���n�u")]
    [SerializeField] GameObject bullet_prefab;//�e�ۂ̃v���n�u
    //�����ɓG�̃X�e�[�^�X�����(�̗́A�U���́AAI)
    public Dictionary<Enemy_ID, Enemy_Status> enemy_index = new()
    {
        {Enemy_ID.Idle_Robot,new Enemy_Status(30,5,Enemy_Ai_Style.Idle)},
    };

    //�ϐ�
    Animator animator;//�A�j���[�^�[
    private int hp;//���݂̗̑�
    private Enemy_Ai_Style ai_style;//AI�X�^�C��
    void Start()
    {
        animator = GetComponent<Animator>();//�A�j���[�^�[���擾
        
        hp = Enemy_Status.max_hp;//�̗͂�ݒ�
    }
    void Update()
    {
        Enmey_State(ai_style);//�G�l�~�[�̍s���Ǘ�
        set_hp = hp;
        //�̗͂�1�ȉ��Ȃ�A�j���[�V������ύX
        if(hp < 1)
        {
            Debug.Log("�G��|����");
            animator.SetBool("Death", true);//�A�j���[�^�[�̏������X�V
        }
    }
    //�G�l�~�[�̍s������
    private void Enmey_State(Enemy_Ai_Style style)
    {
        //style���eAI�X�^�C���̏ꍇ���̋�����ݒ�
        if (style == Enemy_Ai_Style.Idle)
        {
            //��~���
        }
        else { }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Tag.Player�������Ă����ꍇ���̃I�u�W�F�N�g�̕���������
        if(collision.gameObject.CompareTag("Player"))
        {
            transform.LookAt(collision.gameObject.transform);//������ݒ�
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Object.Bullet��Collision�ɏՓ˂����ꍇ�A�̗͂����ۂ�����
        if(other.CompareTag("Bullet"))
        {
            hp -= 5;
            Debug.Log("��������");

        }
    }
    public void DestroyObject()
    {
        Destroy(gameObject);//���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�j��
    }
}