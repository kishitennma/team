using UnityEngine;
using UnityEngine.UI;

public class GunChange : MonoBehaviour
{
    public int Gun_Num=0;//銃コード

    bool tb=false;//デバック長押しロック
    RectTransform rectTransform_get;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform_get = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //デバック
        if (Input.GetKey(KeyCode.Q))
        {
            if (tb == false)
            {
                if (Gun_Num == 1)
                {
                    Gun_Num = 2;
                }
                else
                {
                    Gun_Num = 1;
                }
                tb = true;
            }
        }
        else
        {
            tb = false;
        }

        //プロト位置変更
        if (Gun_Num==1)//メイン
        {
            transform.localPosition = new Vector3(283.5f, -112.0f, 0.0f);
        }
        else if(Gun_Num==2)//サブ
        {
            transform.localPosition = new Vector3(400.0f, -43.0f, 0.0f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
