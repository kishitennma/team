using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponSelection : MonoBehaviour
{

    private int click_count = 0; //�S�̂̃N���b�N��
    private const int MAX_WEAPONS = 2; //�ő呕����
    private List<string> selected_weapon = new List<string>(); //�������ꂽ���탊�X�g
    public Dictionary<string, Text> weapon_number_map = new Dictionary<string, Text>(); //����{�^���̔ԍ�UI
    public void select_Weapon(string weapon_name, Text number_ui)

    {

        if (selected_weapon.Contains(weapon_name))
        {
            //�������킪�I���ς݂Ȃ����
            selected_weapon.Remove(weapon_name);
            click_count--; //�J�E���g�����炷
            update_NumberUI("", number_ui); //����������
            Debug.Log($"���� {weapon_name} ������ �� �c��N���b�N��: {click_count}");
        }

        else

        {

            if (click_count >= MAX_WEAPONS)
            {
                Debug.Log("�����2�܂ł����I�ׂ܂���I");
                return;
            }

            //����I��
            selected_weapon.Add(weapon_name);
            click_count++; //�J�E���g�𑝂₷
            update_NumberUI(click_count.ToString(), number_ui); //�{�^���̏�ɃN���b�N���̐�����\��
            Debug.Log($"���� {weapon_name} ��I�� �� ���݂̃N���b�N��: {click_count}");
        }

    }

    private void update_NumberUI(string text, Text number_ui)
    {
        if (number_ui != null)
        {
            number_ui.text = text; //�������X�V
        }
    }
}
