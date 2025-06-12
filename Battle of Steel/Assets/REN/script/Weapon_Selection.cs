using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class weapon_selection : MonoBehaviour
{
    private static int click_count = 0;
    private const int MAX_WEAPONS = 2;
    private static List<string> selected_weapon = new List<string>();

    public Text number_ui; //�{�^���̏�ɕ\�����鐔��

    private static Dictionary<string, weapon_selection> weapon_instances = new Dictionary<string, weapon_selection>();

    void Start()
    {
        if (number_ui == null)
        {
            Debug.LogError(gameObject.name + " �� number_ui ���ݒ肳��Ă��܂���I");
        }

        weapon_instances[gameObject.name] = this; //���킲�ƂɃC���X�^���X��ۑ�
    }

    public void select_weapon()
    {
        string weapon_name = gameObject.name;
        if (selected_weapon.Contains(weapon_name))
        {
            selected_weapon.Remove(weapon_name);
            click_count--;
            update_number_ui("");
            update_weapon_order(); //�L�����Z�����ɏ��ԃ��Z�b�g
        }
        else
        {
            if (click_count >= MAX_WEAPONS)
            {
                Debug.Log("�����2�܂ł����I�ׂ܂���");
                return;
            }

            selected_weapon.Add(weapon_name);
            click_count++;
            update_number_ui(click_count.ToString());
        }

        Debug.Log("���݂̑���: " + string.Join(", ", selected_weapon));
    }

    private void update_weapon_order()
    {
        for (int i = 0; i < selected_weapon.Count; i++)
        {
            if (weapon_instances.ContainsKey(selected_weapon[i]))
            {
                weapon_instances[selected_weapon[i]].update_number_ui((i + 1).ToString()); //�N���b�N���C��
            }
        }
    }

    private void update_number_ui(string text)
    {
        if (number_ui != null)
        {
            number_ui.text = text;
        }
    }
}