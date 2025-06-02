using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class FadeOnButtonClick : MonoBehaviour
{
    public Image uiElement; //UI�摜
    public Button triggerButton; //�t�F�[�h���J�n����{�^��
    public float startAlpha = 0f; //�X�^�[�g���̓����x�i0 = ���S����, 1 = ���S�s�����j
    public float endAlpha = 1f; //�t�B�j�b�V�����̓����x
    public float fadeDuration = 2f; //�����x�ω��ɂ����鎞�ԁi�b�j
    public float waitTime = 1f; //�t�F�[�h�J�n�܂ł̑ҋ@���ԁi�b�j
    private float elapsedTime = 0;
    void Start()
    {
        //���������x�ݒ�
        Color startColor = uiElement.color;
        startColor.a = startAlpha;
        uiElement.color = startColor;
        //�{�^���������ꂽ�� `StartFade()` ���Ăяo��
        triggerButton.onClick.AddListener(StartFade);
    }
    void StartFade()
    {
        StartCoroutine(FadeAfterDelay());
    }
    IEnumerator FadeAfterDelay()
    {
        yield return new WaitForSeconds(waitTime); //�w��b���ҋ@
        elapsedTime = 0; //�t�F�[�h�J�n���Ƀ^�C�}�[�����Z�b�g
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; //�w�莞�ԓ��Ŋ�������悤�ɒ���
            float easedT = Mathf.SmoothStep(0, 1, t); //�C�[�W���O����
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, easedT);
            Color newColor = uiElement.color;
            newColor.a = currentAlpha;
            uiElement.color = newColor;
            yield return null;
        }
    }
}