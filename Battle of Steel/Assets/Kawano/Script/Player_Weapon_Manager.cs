using UnityEngine;
using UnityEngine.UI;

public class Player_Weapon_Manager : MonoBehaviour
{
    [Header("����؂�ւ��R���|�[�l���g")]
    [Header("������")]
    [SerializeField] Transform set_weapon_point;//���폊���p�|�C���g
    [SerializeField] Transform hand_point;
    public GameObject main_weapon;//�������Ă��郁�C������
    public GameObject sub_weapon;//�������Ă���T�u����
    [SerializeField] Text ammo_texts;//�e�ۂ�UI���
    public Animator player_animator;//�v���C���[�̃A�j���[�V�����R���g���[��
    
    [Header("�{�^������")]
    [SerializeField] KeyCode set_key = KeyCode.Q;//�؂�ւ��{�^��

    private bool hold_secondry_weapon;//�T�u����̏������
    private bool anim_end_flag=false;//�A�j���V�����I���t���O
    private WeaponSystem weapon_system;//weapon_system ammo_text�ύX�p
    private void Start()
    {
        if (main_weapon != null)
        {
            Set_Weapon_hand(main_weapon, sub_weapon);//���C���������Ɏ�������
        }
        else
        {
            Debug.LogError("���C�����킪��݂��܂�܂���ł���");
        }
    }
    private void Update()
    {
        set_weapon_point.position = hand_point.position;
        main_weapon.transform.position = set_weapon_point.position;
        sub_weapon.transform.position = set_weapon_point.position;
        if (sub_weapon!= null)
        {
            //�T�u������������Ă���A��Ɏ����Ă���̂����C������̏ꍇset_key�ŕύX
            if(!hold_secondry_weapon && Input.GetKeyDown(set_key)&& anim_end_flag == false)
            {
                Set_Weapon_hand(sub_weapon, main_weapon);
                hold_secondry_weapon = true;
            }
            else if(hold_secondry_weapon && Input.GetKeyDown(set_key) && anim_end_flag == false)
            {
                Set_Weapon_hand(main_weapon, sub_weapon);
                hold_secondry_weapon = false;
            }
        }
        else
        {
            Debug.LogError("�T�u���킪��݂��܂�܂���ł���");
        }
    }

    private void Set_Weapon_hand(GameObject change_weapon,GameObject set_weapon)
    {
        player_animator.SetBool("Change_Weapon",true);
        anim_end_flag = true;
        weapon_system = change_weapon.GetComponent<WeaponSystem>();//WeaponSystem�R���|�[�l���g�擾
        weapon_system.ammo_text = ammo_texts;
        change_weapon.transform.position = set_weapon_point.transform.position;//�ʒu�𕐊����������ʒu�ɍ��킹��
        Debug.Log("������������܂���");
    }
  
    //����ύX�p�A�j���[�V�����X�g�b�v
    public void Set_End_Change_Anim()
    {
        //�ύX��̕���o��
        if (hold_secondry_weapon)
        {
            main_weapon.SetActive(false);//�ύX�O�̕��������
            sub_weapon.SetActive(true);//�ύX��̕�����o��
        }
        else
        {
            sub_weapon.SetActive(false);//�ύX�O�̕��������
            main_weapon.SetActive(true);//�ύX��̕�����o��
        }
        player_animator.SetBool("Change_Weapon", false);
        anim_end_flag = false;
    }
}
