using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Rank : MonoBehaviour
{
    [SerializeField] Text rankText;      //ランク（S〜Cなど）を表示するUIテキスト

    private int HP = 0;
    void Start()
    {
        //PlayerPrefsから前のシーンで保存された値を取得する
        HP = Player_Status.Player_HP;

        //デバッグ
        //HP = 90;
        //HP = 70;
        //HP = 50;
        //HP = 30;

        //countText.text = HP.ToString();   //現在の体力を表示(リザルトなので多分いらない)
        rankText.text = "";                 //初期は空

        //プレイヤーの残り体力に応じてランク分け
        if (HP == 100)
        {
            rankText.text = "Perfect!"; //EXランク
            rankText.color = Color.cyan;
        }
        else if (HP >= 80)
        {
            rankText.text = "Rank:S"; //最高ランク
            rankText.color = Color.yellow;
        }
        else if (HP >= 60)
        {
            rankText.text = "Rank:A"; //中ランク
            rankText.color = Color.green;
        }
        else if (HP >= 40)
        {
            rankText.text = "Rank:B"; //低ランク
            rankText.color = Color.red;
        }
        else
        {
            rankText.text = "Rank:C"; //最低ランク
            rankText.color = Color.gray;
        }
    }
}

/*   
  //Enemy_Managerのボス撃破時の処理のとこに書き足す用
  Player_Status playerStatus = GameObject.Find("Game_Manager").GetComponent<Player_Status>();
  PlayerPrefs.SetInt("FinalHP", Player_Status.Player_HP); //最終体力の保存
*/