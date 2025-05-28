using UnityEngine;
using System.Collections;
public class UISlideWithDelay : MonoBehaviour
{
    public RectTransform uiElement; //UI�摜�iPanel��Image�j
    public Vector2 startPos; //�����ʒu
    public Vector2 endPos; //�ڕW�ʒu
    public float slideDuration = 1f; //�X���C�h���ԁi�b�j
    public float waitTime = 2f; //�X���C�h�J�n�܂ł̑҂����ԁi�b�j
    private float elapsedTime = 0;
    void Start()
    {
        uiElement.anchoredPosition = startPos;
        StartCoroutine(SlideAfterDelay());
    }
    IEnumerator SlideAfterDelay()
    {
        yield return new WaitForSeconds(waitTime); //�w��b���ҋ@
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slideDuration;
            float easedT = Mathf.SmoothStep(0, 3, t); //�C�[�W���O�v�Z
            uiElement.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
            yield return null;
        }
    }
}