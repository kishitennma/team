using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class BlackEffect : MonoBehaviour
{
    public Image blackOverlay;  // �����摜 (UI Image)
    public float waitTime = 1f; // �ŏ��ɑ҂��ԁi1�b�j
    public float blinkDuration = 1f; // �_�ł��鎞�ԁi1�b�j
    public float blinkSpeed = 0.1f; // �_�ł̊Ԋu�i0.1�b�j
    void Start()
    {
        blackOverlay.enabled = false; // �ŏ��͔�\��
        StartCoroutine(BlinkSequence());
    }
    IEnumerator BlinkSequence()
    {
        yield return new WaitForSeconds(waitTime); // 1�b�҂�
        float elapsedTime = 0;
        while (elapsedTime < blinkDuration)
        {
            blackOverlay.enabled = !blackOverlay.enabled; // �\��/��\���؂�ւ�
            yield return new WaitForSeconds(blinkSpeed);
            elapsedTime += blinkSpeed;
        }
        blackOverlay.enabled = false; // �_�Ō�A���S��\���ɖ߂�
    }
}