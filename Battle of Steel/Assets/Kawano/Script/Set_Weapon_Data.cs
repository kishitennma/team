using System.Collections.Generic;
using UnityEngine;

public class Set_Weapon_Data : MonoBehaviour
{
    List<int> selectWeapons = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //お試し(ほんとはボタンにこれをアタッチ)
        Set_Weapons_Index(1);//0番目の武器
        Set_Weapons_Index(0);
    }

    public void Set_Weapons_Index(int set_number)
    {
        if (selectWeapons.Count >= 2) return;
        if (selectWeapons.Contains(set_number)) return;

        selectWeapons.Add(set_number);
        //二回読み込まれたら、セーブ
        if(selectWeapons.Count == 2)
        {
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