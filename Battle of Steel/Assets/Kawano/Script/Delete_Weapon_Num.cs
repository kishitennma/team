using UnityEngine;

public class Delete_Weapon_Num : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //メニューなどで初期化するための関数
    public void Delete_Set_Weapons()
    {
        weapon_selection.selected_weapon.Clear();//リストないのデータ削除
        weapon_selection.click_count = 0;//クリックカウント初期化
    }
}