using System;
using UnityEngine;
using UnityEngine.UI;

public class GageGagerG : MonoBehaviour
{
    private Image image;//image取得
    float max=0;//最大値
    public GunChange Num;//武器コード
    public WeaponSystem w_index;//武器の番号メイン
    public WeaponSystem w_index2;//武器の番号サブ
    bool sub=false;//サブ確認
    float gun_bullet;//残弾


    private void Start()
    {
        image = this.GetComponent<Image>();
    }

    private void Update()
    {
        //武器位置確認
        if (Num.Gun_Num == w_index2.index)
        {
            sub = true;
        }
        //反映値選別
        if(sub==false)//メイン武器
        {
            gun_bullet = w_index.bullets_left;
        }
        else//サブ武器
        {
            gun_bullet = w_index2.bullets_left;
        }
        //リカバリー
        if(max< gun_bullet)
        {
            max = gun_bullet;
        }

        //ゲージ管理
        image.fillAmount = gun_bullet / max;
    }
}