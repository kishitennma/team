using UnityEngine;
using UnityEngine.UI;

public class GageGagerG : MonoBehaviour
{
    private float gun_bullet = 500.0f;
    private Image image;
    float max;

    private void Start()
    {
        image = this.GetComponent<Image>();
        max = gun_bullet;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gun_bullet--;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            gun_bullet++;
        }

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