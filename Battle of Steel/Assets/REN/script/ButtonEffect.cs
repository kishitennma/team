using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ButtonBlinkEffect : MonoBehaviour
{
    public Button button;
    public float hiddenDuration = 1f; // ��\�����ԁi1�b�j
    public float blinkDuration = 1f; // �_�Ŏ��ԁi1�b�j
    public float blinkSpeed = 0.1f; // �_�ł̊Ԋu�i0.1�b�j
    private Image buttonImage;
    void Start()
    {
        buttonImage = button.GetComponent<Image>();
        buttonImage.enabled = false; // �ŏ��͔�\��
        StartCoroutine(BlinkSequence());
    }
    IEnumerator BlinkSequence()
    {
        yield return new WaitForSeconds(hiddenDuration); // 1�b�҂�
        StartCoroutine(BlinkEffect());
    }
    IEnumerator BlinkEffect()
    {
        float elapsedTime = 0;
        while (elapsedTime < blinkDuration)
        {
            buttonImage.enabled = !buttonImage.enabled; // �\��/��\���؂�ւ�
            yield return new WaitForSeconds(blinkSpeed);
            elapsedTime += blinkSpeed;
        }
        buttonImage.enabled = true; // �Ō�ɒʏ�\���֖߂�
    }
}