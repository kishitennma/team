using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class GunChange : MonoBehaviour
{
    public int Gun_Num=0;//銃コード※ここで変えるな

    public WeaponSystem w_index;//武器の番号メイン
    public WeaponSystem w_index2;//武器の番号サブ
    public Player_Weapon_Manager pw_manager;//現在値確認
    public bool sub = false;//サブかどうか

    // Update is called once per frame
    void Update()
    {
        if (Gun_Num == w_index.index)//メイン
        {
            //保持
        }
        else if (Gun_Num == w_index2.index)//サブ
        {
            sub = true;
        }
        else//その他
        {
            Destroy(this.gameObject);
        }
        //位置変更
        if (sub == false)
        {
            if (!pw_manager.hold_secondry_weapon)//メイン
            {
                transform.localPosition = new Vector3(300.0f, -112.0f, 0.0f);
            }
            else if (pw_manager.hold_secondry_weapon)//サブ
            {
                transform.localPosition = new Vector3(400.0f, -43.0f, 0.0f);
            }
        }
        else
        {
            if (pw_manager.hold_secondry_weapon)//メイン
            {
                transform.localPosition = new Vector3(300.0f, -112.0f, 0.0f);
            }
            else if (!pw_manager.hold_secondry_weapon)//サブ
            {
                transform.localPosition = new Vector3(400.0f, -43.0f, 0.0f);
            }
        }
    }
}
