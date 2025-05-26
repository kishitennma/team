using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ButtonHide : MonoBehaviour
{
    public Button button;
    public float blinkDuration = 1f; // �_�ł��鎞�ԁi�b�j
    public float blinkSpeed = 0.1f; // �_�ł̊Ԋu�i�b�j
    private Image buttonImage;
    void Start()
    {
        buttonImage = button.GetComponent<Image>();
        StartCoroutine(BlinkEffect());
    }
    IEnumerator BlinkEffect()
    {
        float elapsedTime = 0;
        while (elapsedTime < blinkDuration)
        {
            buttonImage.enabled = !buttonImage.enabled; // �\��/��\���̐؂�ւ�
            yield return new WaitForSeconds(blinkSpeed);
            elapsedTime += blinkSpeed;
        }
        buttonImage.enabled = true; // �Ō�ɕ\����Ԃɖ߂�
    }
}