using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponSelection : MonoBehaviour
{

    private int click_count = 0; //全体のクリック回数
    private const int MAX_WEAPONS = 2; //最大装備数
    private List<string> selected_weapon = new List<string>(); //装備された武器リスト
    public Dictionary<string, Text> weapon_number_map = new Dictionary<string, Text>(); //武器ボタンの番号UI
    public void select_Weapon(string weapon_name, Text number_ui)

    {

        if (selected_weapon.Contains(weapon_name))
        {
            //もし武器が選択済みなら解除
            selected_weapon.Remove(weapon_name);
            click_count--; //カウントを減らす
            update_NumberUI("", number_ui); //数字を消す
            Debug.Log($"武器 {weapon_name} を解除 → 残りクリック数: {click_count}");
        }

        else

        {

            if (click_count >= MAX_WEAPONS)
            {
                Debug.Log("武器は2つまでしか選べません！");
                return;
            }

            //武器選択
            selected_weapon.Add(weapon_name);
            click_count++; //カウントを増やす
            update_NumberUI(click_count.ToString(), number_ui); //ボタンの上にクリック順の数字を表示
            Debug.Log($"武器 {weapon_name} を選択 → 現在のクリック数: {click_count}");
        }

    }

    private void update_NumberUI(string text, Text number_ui)
    {
        if (number_ui != null)
        {
            number_ui.text = text; //数字を更新
        }
    }
}
