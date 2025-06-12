using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Set_Weapon_Data : MonoBehaviour
{
    List<int> selectWeapons = new();
    public bool max_click_flag;
    public void Set_Weapons_Index(int set_number)
    {
        //�������łɂ��̕���f�[�^���擾���Ă����ꍇ�͂��̃f�[�^���폜���A���̏������I��
        if (selectWeapons.Contains(set_number))
        {
            Debug.Log("�l���폜");
            selectWeapons.Remove(set_number);
            return;
        }
        //�����ꂽ�{�^���ɑΉ����Ă��鐔�������X�g�ɒǉ�
        selectWeapons.Add(set_number);
        Debug.Log(set_number + "�����܂���");

        if (weapon_selection.click_count == 2)
        {
            max_click_flag = true;
            if (selectWeapons.Count < 2)
            {
                Debug.LogError("�l���s�\���ł�");
                return;
            }
            Save_Weapons_Index();
        }

    }
    public void Save_Weapons_Index()
    {
        if (max_click_flag)
        {
            PlayerPrefs.SetInt("Select_f", selectWeapons[0]);
            PlayerPrefs.SetInt("Select_s", selectWeapons[1]);
            PlayerPrefs.Save();
        }
    }
}