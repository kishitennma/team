using UnityEngine;
using System.Collections;
public class BluelineXscale : MonoBehaviour
{
    public RectTransform uiElement; //UI�摜
    public float startScaleX = 1f; //����X�X�P�[��
    public float endScaleX = 2f; //�ڕWX�X�P�[���i�L�΂��j
    public float targetTime = 2f; //���b�Ŋg�傷�邩
    public float waitTime = 1f; //�g��J�n�܂ł̑҂����ԁi�b�j
    private float elapsedTime = 0;
    void Start()
    {
        uiElement.localScale = new Vector3(startScaleX, uiElement.localScale.y, 1);
        StartCoroutine(ScaleAfterDelay());
    }
    IEnumerator ScaleAfterDelay()
    {
        yield return new WaitForSeconds(waitTime); //�w��b���ҋ@
        while (elapsedTime < targetTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / targetTime; //�w�莞�ԓ��Ɋ�������悤�ɒ���
            float easedT = Mathf.SmoothStep(0, 3, t); //�C�[�W���O����
            float currentScaleX = Mathf.Lerp(startScaleX, endScaleX, easedT);
            uiElement.localScale = new Vector3(currentScaleX, uiElement.localScale.y, 1);
            yield return null;
        }
    }
}