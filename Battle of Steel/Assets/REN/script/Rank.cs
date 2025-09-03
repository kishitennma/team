using UnityEngine;
using UnityEngine.UI;
public class Rank : MonoBehaviour
{
    [SerializeField] Text rankText;   //ランク（S〜Cなど）を表示するUIテキスト
    [SerializeField] Text hp_text;    //表示する最終体力
    [SerializeField] Text heal_text;  //表示する回復使用回数
    [SerializeField] Text hit_text;   //当たった回数表示
    private int HP = 0;
    private int sub_heal_cnt = Player_Status.MAX_HEAL_COUNTS;
    private int heal_cnt;
    void Start()
    {
        //PlayerPrefsから前のシーンで保存された値を取得する
        HP = Player_Status.Player_HP;
        heal_cnt = (sub_heal_cnt - Player_Status.heal_count);
        rankText.text = "";                                          //初期は空
        hp_text.text =   "体力　　　: " + HP;                        //体力の表示
        heal_text.text = "回復回数  : " + heal_cnt;                  //最大数からの差分を表示
        hit_text.text =  "被弾回数  : " + EnemyBulletAction.hit_counts;//被弾回数表示

        //プレイヤーの残り体力、回復回数に応じてランク分け
        if (HP == 100 && heal_cnt == 0)
        {
            rankText.text = "Perfect!"; //EXランク
            rankText.color = Color.cyan;
        }
        else if (HP >= 80 && heal_cnt <= 1)
        {
            rankText.text = "Rank:S"; //最高ランク
            rankText.color = Color.yellow;
        }
        else if (HP >= 60 && heal_cnt <= 2)
        {
            rankText.text = "Rank:A"; //中ランク
            rankText.color = Color.green;
        }
        else if (HP >= 50 && heal_cnt <= 3)
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
