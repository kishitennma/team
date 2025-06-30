using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //TextMeshProを使うための名前空間
public class Rank : MonoBehaviour
{

   //[SerializeField] TextMeshProUGUI countText;     //体力（またはスコア）を表示するUIテキスト(多分いらない)
    [SerializeField] TextMeshProUGUI rankText;      //ランク（A〜Dなど）を表示するUIテキスト
  
    void Start()
    {
        //PlayerPrefsから前のシーンで保存された値を取得する
        //"FinalHP" は保存時に使ったキー。第2引数の0は値が見つからなかったとき
        int HP = PlayerPrefs.GetInt("FinalHP", 0);

        //countText.text = HP.ToString();     //現在の体力を表示(リザルトなので多分いらない)
        rankText.text = "";                 //初期は空

        //プレイヤーの体力に応じてランク分け
        if (HP >= 100)
        {
            rankText.text = "Rank:S"; //最高ランク
        }
        else if (HP >= 70)
        {
            rankText.text = "Rank:A"; //中ランク
        }
        else if (HP >= 40)
        {
            rankText.text = "Rank:B"; //低ランク
        }
        else
        {
            rankText.text = "Rank:C"; //最低ランク
        }
    }
}

/*   
  //Enemy_Managerのボス撃破時の処理のとこに書き足す用
  Player_Status playerStatus = GameObject.Find("Game_Manager").GetComponent<Player_Status>();
  PlayerPrefs.SetInt("FinalHP", Player_Status.Player_HP); //最終体力の保存
*/