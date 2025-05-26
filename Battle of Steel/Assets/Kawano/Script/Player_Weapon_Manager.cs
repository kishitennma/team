using UnityEngine;
using UnityEngine.UI;

public class Player_Weapon_Manager : MonoBehaviour
{
    [Header("����؂�ւ��R���|�[�l���g")]
    [Header("������")]
    [SerializeField] Transform set_weapon_point;//���폊���p�|�C���g
    [SerializeField] GameObject main_weapon;//�������Ă��郁�C������
    [SerializeField] GameObject sub_weapon;//�������Ă���T�u����
    [SerializeField] Text ammo_texts;//�e�ۂ�UI���
    public Animator player_animator;//�v���C���[�̃A�j���[�V�����R���g���[��
    public WeaponSystem weapon_system;//weapon_system ammo_text�ύX�p
    [Header("�؂�ւ��{�^��")]
    [SerializeField] KeyCode set_key = KeyCode.Q;//�؂�ւ��{�^��

    private bool hold_secondry_weapon;//�T�u����̏������
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
        if (sub_weapon!= null)
        {
            //�T�u������������Ă���A��Ɏ����Ă���̂����C������̏ꍇset_key�ŕύX
            if(!hold_secondry_weapon && Input.GetKeyDown(set_key))
            {
                Set_Weapon_hand(sub_weapon, main_weapon);
                hold_secondry_weapon = true;
            }
            else if(hold_secondry_weapon && Input.GetKeyDown(set_key))
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
        weapon_system = change_weapon.GetComponent<WeaponSystem>();//WeaponSystem�R���|�[�l���g�擾
        weapon_system.ammo_text = ammo_texts;
        change_weapon.SetActive(true);//�ύX��̕�����o��
        change_weapon.transform.position = set_weapon_point.transform.position;//�ʒu�𕐊����������ʒu�ɍ��킹��
        set_weapon.SetActive(false);//�ύX�O�̕��������
        Debug.Log("������������܂���");
    }
    //����ύX�p�A�j���[�V�����X�g�b�v
    public void Set_End_Change_Anim()
    {
        player_animator.SetBool("Change_Weapon", false);
    }
}