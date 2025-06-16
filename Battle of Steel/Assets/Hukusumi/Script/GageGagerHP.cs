using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GageGagerHP : MonoBehaviour
{
    GameObject HP_Text;//HP取得
    HPText script;

    private int myhp ;//HP現在値
    private Image image;

    int max;//最大値
    private void Start()
    {
        HP_Text = GameObject.Find("HP_Text"); //Unityちゃんをオブジェクトの名前から取得して変数に格納する
        script = HP_Text.GetComponent<HPText>(); //unitychanの中にあるUnityChanScriptを取得して変数に格納する
        image = this.GetComponent<Image>();
        myhp = Player_Status.Player_HP;
        max = Player_Status.Player_HP;
    }

    private void Update()
    {
        myhp = Player_Status.Player_HP;
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    myhp--;
        //}
        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    myhp++;
        //}

        //ゲージ管理
        image.fillAmount = myhp / max;
        if(myhp / max <0)
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
        else if (image.fillAmount < 0.2)
        {
            image.color = new Color32(235, 33, 13, 255);
        }
        else if (image.fillAmount < 0.4)
        {
            image.color = new Color32(184, 235, 13, 255);
        }
        else
        {
            image.color = new Color32(13, 235, 69, 255);
        }
    }


}