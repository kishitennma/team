using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Set_Weapon_Data : MonoBehaviour
{
    List<int> selectWeapons = new();
    public bool max_click_flag;
    public void Set_Weapons_Index(int set_number)
    {
        //もしすでにその武器データを取得していた場合はそのデータを削除し、この処理を終了
        if (selectWeapons.Contains(set_number))
        {
            selectWeapons.Remove(set_number);
            return;
        }
        //押されたボタンに対応している数字をリストに追加
        selectWeapons.Add(set_number);

        if (weapon_selection.click_count == 2)
        {
            max_click_flag = true;
            if (selectWeapons.Count < 2)
            {
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