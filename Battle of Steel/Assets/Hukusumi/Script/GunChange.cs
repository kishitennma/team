using UnityEngine;
using UnityEngine.UI;

public class GunChange : MonoBehaviour
{
    public int Gun_Num=0;//銃コード
    int Gun_Cord;

    public WeaponSystem w_index;//武器の番号
    public Player_Weapon_Manager pw_manager;
    public bool change_weapon_flag;
    bool tb=false;//デバック長押しロック
    RectTransform rectTransform_get;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform_get = gameObject.GetComponent<RectTransform>();
        Gun_Cord = w_index.index;
        Gun_Num = Gun_Cord;
    }

    // Update is called once per frame
    void Update()
    {
        //デバック
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    if (tb == false)
        //    {
        //        if (Gun_Num == 1)
        //        {
        //            Gun_Num = 2;
        //        }
        //        else
        //        {
        //            Gun_Num = 1;
        //        }
        //        tb = true;
        //    }
        //}
        //else
        //{
        //    tb = false;
        //}

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (!tb)
            {
                if (change_weapon_flag)
                {
                    change_weapon_flag = false;
                }
                else
                {
                    change_weapon_flag = true;
                }
                tb = false;
            }
            else
            {
                tb = false;
            }
        }
        //プロト位置変更
        if (!change_weapon_flag)//メイン
        {
            transform.localPosition = new Vector3(300.0f, -112.0f, 0.0f);
        }
        else if (change_weapon_flag)//サブ
        {
            transform.localPosition = new Vector3(400.0f, -43.0f, 0.0f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
