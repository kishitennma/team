using UnityEngine;
using System.Collections;
public class UISlideWithDelay : MonoBehaviour
{
    public RectTransform uiElement; //UI画像（PanelやImage）
    public Vector2 startPos; //初期位置
    public Vector2 endPos; //目標位置
    public float slideDuration = 1f; //スライド時間（秒）
    public float waitTime = 2f; //スライド開始までの待ち時間（秒）
    private float elapsedTime = 0;
    void Start()
    {
        uiElement.anchoredPosition = startPos;
        StartCoroutine(SlideAfterDelay());
    }
    IEnumerator SlideAfterDelay()
    {
        yield return new WaitForSeconds(waitTime); //指定秒数待機
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slideDuration;
            float easedT = Mathf.SmoothStep(0, 3, t); //イージング計算
            uiElement.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
            yield return null;
        }
    }
}