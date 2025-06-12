using System.Collections.Generic;
using UnityEngine;

public class Set_Weapon_Data : MonoBehaviour
{
    List<int> selectWeapons = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Set_Weapons_Index(int set_number)
    {
        if (weapon_selection.click_count >= 2) return;
        //if (selectWeapons.Contains(set_number)) return;
        //Debug.LogError("set_number ="+set_number);
        //selectWeapons.Add(set_number);
        //二回読み込まれたら、セーブ
        if(weapon_selection.click_count == 2)
        {
            Debug.Log("aaaaa");
            PlayerPrefs.SetInt("Select_f", selectWeapons[0]);
            PlayerPrefs.SetInt("Select_s", selectWeapons[1]);
            PlayerPrefs.Save();
        }
    }
    public void Compare_Weapon_Index()
    {
        int index_a = PlayerPrefs.GetInt("Select_f",0);
        int index_b = PlayerPrefs.GetInt("Select_s",1);

        int main_index = Mathf.Max(index_a, index_b);
        int sub_index = Mathf.Min(index_a, index_b);

    }
}