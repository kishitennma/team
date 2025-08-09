using UnityEngine;
public class StageButton : MonoBehaviour
{
    //ここに増やしたい分コピーする
    //左右矢印(A, B)
    public GameObject Left;
    public GameObject Right;
    //説明テキスト用
    public GameObject[]Text;
    //この下はステージ用
    public GameObject[]Stage;
    //最大ステージ数
    const int MAX_STAGE = 4;

    int STnum = 0;

    //左ボタンが押された時に呼ばれる
    public void OnPushButtonLeft()
    {
        
        //下限ページ
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

        
        //上限ページ
        if (STnum != 3)
        {
            STnum++;
        }


        /*
        if(STnum != 3 && STnum != 0)
        {
            Left.SetActive(true);
            Right.SetActive(true);
        }
        else if(STnum == 0)
        {
            Left.SetActive(false);
            Right.SetActive(true);
        }
        else if(STnum == 3)
        {
            Left.SetActive(true);
            Right.SetActive(false);
        }
        */

            Stage_set();
    }
    void Stage_set()
    {
        for(int i=0;i<MAX_STAGE;i++)
        {
            if(i != STnum)
            {
                Stage[i].SetActive(false);
                Text[i].SetActive(false);
            }
            else
            {
                Stage[i].SetActive(true);
                Text[i].SetActive(true);
            }

        }
    }
    //スタート関数
    private void Start()
    {
        OnPushButtonLeft();
    }
}