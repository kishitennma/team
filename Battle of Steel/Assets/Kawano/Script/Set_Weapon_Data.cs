using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Set_Weapon_Data : MonoBehaviour
{
    List<int> selectWeapons = new();
    public bool max_click_flag;
    public Button SButton;
    public Text Ready;
    private int index_num = 0;
    private Color nowColor;
    private float timer;
    private float dur = 1f;
    private void Start()
    {
        Ready.text = "Select 2 Weapons";
        Ready.color = Color.white;
        Delete_Weapon_Index();
    }
    private void Update()
    {
        //仮で常に取得
        index_num = weapon_selection.click_count;

        //武器を二つ選択したら次に進めるようにする
        if (index_num == 2)
        {
            timer += Time.deltaTime;
            float t = Mathf.PingPong(timer / dur,1f);
            nowColor = Color.Lerp(Color.cyan, Color.yellow,t);
            var cb = SButton.colors;
            cb.normalColor = nowColor;
            SButton.colors = cb;
            Ready.text = "Ready?";
            Ready.color = Color.cyan;

            max_click_flag = true;
        }
        if (index_num < 2)
        {
            Ready.text = "Select 2 Weapon";
            Ready.color = Color.white;
            max_click_flag = false;
        }
    }

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
    }
    public void Save_Weapons_Index()
    {
        if (max_click_flag)
        {
            Debug.Log("値のカウント　+ " + selectWeapons.Count);
            if (selectWeapons[0] == -1 || selectWeapons[1] == -1)
            {
                Debug.LogError("値がありません");
            }
            //武器のインデックス番号を渡す
            Player_Status.weapons_f = selectWeapons[0];
            Player_Status.weapons_s = selectWeapons[1];
            max_click_flag = false;
        }
    }
    //インデックス削除関数
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