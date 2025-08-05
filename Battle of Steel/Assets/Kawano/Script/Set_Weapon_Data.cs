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
        Debug.Log(set_number + "== 選択された武器");
        if (weapon_selection.click_count == 2)
        {
            max_click_flag = true;
            if (selectWeapons.Count < 2)
            {
                return;
            }
        }


    }
    public void Save_Weapons_Index()
    {
        if (max_click_flag)
        {
            Debug.Log(selectWeapons[0] + "==" + selectWeapons[1]);
            if (selectWeapons[0] == -1 || selectWeapons[1] == -1)
            {
                Debug.LogError("値がありません");
            }
            Player_Status.weapons_f = selectWeapons[0];
            Player_Status.weapons_s = selectWeapons[1];
            max_click_flag = false;
        }
    }
    public void Delete_Weapon_Index()
    {
        if(selectWeapons.Count == 2)
        {
            selectWeapons[0] = -1;
            selectWeapons[1] = -1;
        }
        max_click_flag = false;

    }
}