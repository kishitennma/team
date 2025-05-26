using UnityEngine;
using UnityEngine.UI;

public class Player_Weapon_Manager : MonoBehaviour
{
    [Header("������")]
    [SerializeField] Transform set_weapon_point;//���폊���p�|�C���g
    [SerializeField] GameObject[] hold_weapons;//�������Ă��镐��
    [SerializeField] Text ammo_texts;//�e�ۂ�UI���
    public Animator player_animator;//�v���C���[�̃A�j���[�V�����R���g���[��
    [Header("�؂�ւ��{�^��")]
    [SerializeField] KeyCode set_key = KeyCode.Q;//�؂�ւ��{�^��
    public WeaponSystem weapon_system;//weapon_system ammo_text�ύX�p

    private bool hold_secondry_weapon = false;//�T�u����̏������
    private void Start()
    {
        if (hold_weapons[0] != null)
        {
            Set_Weapon_hand(hold_weapons[0], hold_weapons[1]);//���C���������Ɏ�������
        }
        else
        {
            Debug.LogError("���C�����킪��݂��܂�܂���ł���");
        }
    }
    private void Update()
    {
        if (hold_weapons[1]!= null)
        {
            //�T�u������������Ă���A��Ɏ����Ă���̂����C������̏ꍇset_key�ŕύX
            if(!hold_secondry_weapon && Input.GetKeyDown(set_key))
            {
                Set_Weapon_hand(hold_weapons[1], hold_weapons[0]);
                hold_secondry_weapon = true;
            }
            else if(hold_secondry_weapon && Input.GetKeyDown(set_key))
            {
                Set_Weapon_hand(hold_weapons[0], hold_weapons[1]);
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
        weapon_system = change_weapon.GetComponent<WeaponSystem>();
        weapon_system.ammo_text = ammo_texts;
        change_weapon.SetActive(true);
        change_weapon.transform.position = set_weapon_point.transform.forward;//�|�C���g�̐��ʂ̕����֌�������
        set_weapon.SetActive(false);
        Debug.Log("������������܂���");
    }
    //����ύX�p�A�j���[�V�����X�g�b�v
    public void Set_End_Change_Anim()
    {
        player_animator.SetBool("Change_Weapon", false);
    }
}