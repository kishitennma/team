using UnityEngine;
using UnityEngine.UI;

public class GageGagerB : MonoBehaviour
{
    private float boostG = 500.0f;
    private Image image;

    float max;
    private void Start()
    {
        image = this.GetComponent<Image>();
        max = boostG;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            boostG--;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            boostG++;
        }

        image.fillAmount = boostG / max;
        if (boostG / max < 0)
        {
            image.fillAmount = 0.0f;
            boostG = 0.0f;
        }
        else if (boostG / max > 1.0f)
        {
            image.fillAmount = 1.0f;
            boostG = max;
        }
    }
}