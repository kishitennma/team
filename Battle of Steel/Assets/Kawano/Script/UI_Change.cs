using UnityEngine;
using UnityEngine.UI;

public class UI_Change : MonoBehaviour
{
    public int now_weap_count;
    public Button SButton;
    public Text Ready;

    private Color nowColor;
    private float timer;
    private float dur = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Ready.text = "Select 2 Weapons";
        Ready.color = Color.white;

    }

    // Update is called once per frame
    void Update()
    {
        now_weap_count = weapon_selection.click_count;

        if (now_weap_count == 2)
        {
            timer += Time.deltaTime;
            float t = Mathf.PingPong(timer / dur, 1f);
            nowColor = Color.Lerp(Color.cyan, Color.yellow, t);
            var cb = SButton.colors;
            cb.normalColor = nowColor;
            SButton.colors = cb;
            Ready.text = "Ready?";
            Ready.fontSize = 100;
            Ready.color = Color.cyan;

        }
        else
        {
            Ready.text = "Select 2 Weapon";
            Ready.fontSize = 60;
            Ready.color = Color.white;
        }
    }
}