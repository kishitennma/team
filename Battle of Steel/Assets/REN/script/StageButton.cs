using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    //ここに増やしたい分コピーする
    //左右矢印(A, B)
    public GameObject Left;
    public GameObject Right;
    //この下はステージ用
    public GameObject[]Stage;

    int STnum = 0;

    //左ボタンが押された時に呼ばれる
    public void OnPushButtonLeft()
    {
        if (STnum != 0)
        {
            STnum--;
        }
      

        Stage_set();
            //1を表示2を非表示
        //    ST_1.SetActive(false);
        //ST_2.SetActive(true);
    }
    //右ボタンが押された時に呼ばれる
    public void OnPushButtonRight()
    {
        if (STnum != 3)
        {
            STnum++;
        }
   
        Stage_set();
    }
    void Stage_set()
    {
        for(int i=0;i<4;i++)
        {
            if(i != STnum)
            {
                Stage[i].SetActive(false);
            }
            else
            {
                Stage[i].SetActive(true);
            }

        }
    }
    //スタート関数
    private void Start()
    {
        OnPushButtonLeft();
    }
}