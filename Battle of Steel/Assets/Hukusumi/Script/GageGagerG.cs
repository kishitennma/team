using UnityEngine;
using UnityEngine.UI;

public class GageGagerG : MonoBehaviour
{
    private float gun_bullet = 500.0f;//初期値デバック
    private Image image;
    float max;//最大値

    private void Start()
    {
        image = this.GetComponent<Image>();
        max = gun_bullet;
    }

    private void Update()
    {
        //デバック
        if (Input.GetKey(KeyCode.Q))
        {
            gun_bullet--;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            gun_bullet++;
        }

        //ゲージ管理
        image.fillAmount = gun_bullet / max;
        if (gun_bullet / max < 0)
        {
            image.fillAmount = 0.0f;
            gun_bullet = 0.0f;
        }
        else if (gun_bullet / max > 1.0f)
        {
            image.fillAmount = 1.0f;
            gun_bullet = max;
        }
    }
}