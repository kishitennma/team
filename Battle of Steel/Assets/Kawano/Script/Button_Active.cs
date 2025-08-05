using UnityEngine;

public class Button_Active : MonoBehaviour
{
    public GameObject Next_Button;
    private int index_num;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Next_Button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        index_num = weapon_selection.click_count;
        //二つ武器を選択したときのみ次に進むボタンを表示
        if (index_num == 2)
            Next_Button.SetActive(true);
        else
            Next_Button.SetActive(false);

    }
}
