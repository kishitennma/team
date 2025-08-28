using UnityEngine;
using UnityEngine.UI;
public class StageSelecting : MonoBehaviour
{
    [Header("ステージ用オブジェクト")]
    public Text stage_name;//ステージの名前
    public Text stage_text;//ステージの説明、難易度表示
    public Image stage_image;//ステージの画像
    public SEandSceneChange scene;//シーン切り替えスクリプト
    public GameObject Star;//クリア時の★
    [Header("ステージ画像,ステージ名")]
    public Sprite[] S_Images;
    public string[] S_Names;
    public string[] S_Texts;
    [Header("ステージのシーン名")]
    public string[] scene_setname;//シーンの名前
    //各ステージのクリア状況
    public static bool[] clear_flags = 
    { 
    false, false, false, false, false, false,
    };    int now_stage = 0;//現在選択しているステージ
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        now_stage = 0;//初期化
        Star.SetActive(false);//初期は非表示
    }
    // Update is called once per frame
    void Update()
    {
        Set_Stage_Info(now_stage);
    }

    public void Set_Stage_Info(int now)
    {
        //現在選択している数字に合わせて画像と名前を設定
        for (int i = 0; i < S_Images.Length; i++)
        {
            if(i == now_stage)
            {
                stage_image.sprite = S_Images[i];
                stage_text.text = S_Texts[i];
                stage_name.text = S_Names[i];
                scene.sceneName = scene_setname[i];
                Scene_Save.Scene_Value = now_stage;//リトライ用の値設定

                //クリア状況から★を表示
                if (clear_flags[now_stage])
                {
                    Star.SetActive(true);
                }
                else
                {
                    Star.SetActive(false);   
                }
            }
        }
    }
    public void Add_num()
    {
        if(now_stage < S_Images.Length-1)
        {
            now_stage++;
        }
    }
    public void Sub_num()
    {
        if(now_stage > 0)
        {
            now_stage--;
        }
    }
}