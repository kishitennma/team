using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Fadeloop : MonoBehaviour
{
    public Image uiElement; //UI�摜
    public float fadeDuration = 2f; //�t�F�[�h�̎��ԁi�b�j
    public float minAlpha = 0f; //�Œᓧ���x�i0 = ���S�����j
    public float maxAlpha = 1f; //�ő哧���x�i1 = ���S�s�����j
    void Start()
    {
        StartCoroutine(FadeLoop());
    }
    IEnumerator FadeLoop()
    {
        while (true) //�������[�v�Ńt�F�[�h���J��Ԃ�
        {
            yield return StartCoroutine(Fade(minAlpha, maxAlpha)); //�t�F�[�h�C��
            yield return StartCoroutine(Fade(maxAlpha, minAlpha)); //�t�F�[�h�A�E�g
        }
    }
    IEnumerator Fade(float fromAlpha, float toAlpha)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float easedT = Mathf.SmoothStep(0, 1, t); //�C�[�W���O����
            float currentAlpha = Mathf.Lerp(fromAlpha, toAlpha, easedT);
            Color newColor = uiElement.color;
            newColor.a = currentAlpha;
            uiElement.color = newColor;
            yield return null;
        }
    }
}