using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GageGagerHP : MonoBehaviour
{
    private int myhp ;//HP現在値
    private Image image;//image取得

    int max;//最大値
    private void Start()
    {
        image = this.GetComponent<Image>();
        max = Player_Status.Player_HP;
    }

    private void Update()
    {
        //現在値
        myhp = Player_Status.Player_HP;

        //ゲージ管理
        image.fillAmount = (float)myhp / (float)max;
        //超過防止
        if((myhp / max) <0)
        {
            image.fillAmount = 0.0f;
            myhp = 0;
        }
        else if(myhp / max >1.0f)
        {
            image.fillAmount = 1.0f;
            myhp = max;
        }

        //色管理
        if (image.fillAmount <= 0)
        {
            image.color = new Color32(0, 0, 0, 255);
        }
        else if (image.fillAmount < 0.21)
        {
            image.color = new Color32(235, 33, 13, 255);
        }
        else if (image.fillAmount < 0.41)
        {
            image.color = new Color32(184, 235, 13, 255);
        }
        else
        {
            image.color = new Color32(13, 235, 69, 255);
        }
    }


}