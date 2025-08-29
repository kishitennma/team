using UnityEngine;
using UnityEngine.UI;
public class Tutorial_Manager : MonoBehaviour
{
    public Text tutorial_text;
    public GameObject break_wall;//壊れる壁
    public GameObject break_wall_second;//壊れる壁
    private int timer = 0;
    private int tutorial_number;//チュートリアルナンバー
    private bool tutorial_flag = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorial_text.text = " ";
        tutorial_number = 0;
        break_wall.SetActive(true);
        break_wall_second.SetActive(true);
    }

    private void Update()
    {
        if(timer <= 100)
        timer++;
        //時間経過でチュートリアルを更新
        if(tutorial_flag)
        {
            if (tutorial_number == 0 && timer > 100)
            {
                tutorial_number = 1;//移動
            }
            if (tutorial_number == 1 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)))
            {
                tutorial_number = 2;//ダッシュ
            }
            if (tutorial_number == 2 && Input.GetKeyDown(KeyCode.LeftShift))
            {
                tutorial_number = 3;//ジャンプ
            }
            if (tutorial_number == 3 && Input.GetKeyDown(KeyCode.Space))
            {
                tutorial_number = 4;//弾丸発射
            }
            if (tutorial_number == 4 && Input.GetMouseButtonDown(0))
            {
                break_wall.SetActive(false);
                tutorial_number = 5;//ロックオン
            }
            if (tutorial_number == 5 && Input.GetMouseButtonDown(1) )
            {
                tutorial_number = 6;//武器切り替え
            }
            if(tutorial_number == 6 && Input.GetKeyDown(KeyCode.Q))
            {
                tutorial_number = 7;//回復
                Player_Status.Player_HP = 70;
            }
            if(tutorial_number == 7 && Input.GetKeyDown(KeyCode.R))
            {
                break_wall_second.SetActive(false);
                tutorial_number = 8;
            }
            if(tutorial_number == 8)
            {
                tutorial_flag = false;
            }
        }
        Set_Tutorial_Text();
    }

    private void Set_Tutorial_Text()
    {
        switch(tutorial_number)
        {

            case 1:
                tutorial_text.text = "WASD : 移動";break;
            case 2:
                tutorial_text.text = "左Shift + WASD : ダッシュ";break;
            case 3:
                tutorial_text.text = "Space : ジャンプ"; break;
            case 4:
                tutorial_text.text = "左クリック : 弾丸発射";break;
            case 5:
                tutorial_text.text = "右クリック : ロックオン";break;
            case 6:
                tutorial_text.text = "Qキー : 武器切り替え";break;
            case 7:
                tutorial_text.text = "Rキー : 回復(三回まで)";break;
            case 8:
                tutorial_text.text = "全ての敵を倒せ！";break;
        }
    }
}