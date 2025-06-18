using System;
using UnityEngine;
using UnityEngine.UI;

public class GageGagerG : MonoBehaviour
{
    private Image image;
    float max=0;//最大値
    public GunChange Num;//武器コード
    public WeaponSystem w_index;//武器の番号メイン
    public WeaponSystem w_index2;//武器の番号サブ
    bool sub=false;
    float gun_bullet;


    private void Start()
    {
        image = this.GetComponent<Image>();
    }

    private void Update()
    {
        if (Num.Gun_Num == w_index2.index)//サブ
        {
            sub = true;
        }
        if(sub==false)
        {
            gun_bullet = w_index.bullets_left;
        }
        else
        {
            gun_bullet = w_index2.bullets_left;
        }
        if(max< gun_bullet)
        {
            max = gun_bullet;
        }
            //デバック
            //if (Input.GetKey(KeyCode.Q))
            //{
            //    gun_bullet--;
            //}
            //else if (Input.GetKey(KeyCode.E))
            //{
            //    gun_bullet++;
            //}

            //ゲージ管理
            image.fillAmount = gun_bullet / max;
        if (gun_bullet / max < 0)
        {
            image.fillAmount = 0.0f;
            gun_bullet = 0.0f;
        }
        else if (gun_bullet / max > 1.0f)
        {
            image.fillAmount = 1.0f;
            gun_bullet = max;
        }
    }
}