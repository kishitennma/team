using UnityEngine;
using System.Collections;
public class BluelineXscale : MonoBehaviour
{
    public RectTransform uiElement; //UI画像
    public float startScaleX = 1f; //初期Xスケール
    public float endScaleX = 2f; //目標Xスケール（伸ばす）
    public float targetTime = 2f; //何秒で拡大するか
    public float waitTime = 1f; //拡大開始までの待ち時間（秒）
    private float elapsedTime = 0;
    void Start()
    {
        uiElement.localScale = new Vector3(startScaleX, uiElement.localScale.y, 1);
        StartCoroutine(ScaleAfterDelay());
    }
    IEnumerator ScaleAfterDelay()
    {
        yield return new WaitForSeconds(waitTime); //指定秒数待機
        while (elapsedTime < targetTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / targetTime; //指定時間内に完了するように調整
            float easedT = Mathf.SmoothStep(0, 3, t); //イージング処理
            float currentScaleX = Mathf.Lerp(startScaleX, endScaleX, easedT);
            uiElement.localScale = new Vector3(currentScaleX, uiElement.localScale.y, 1);
            yield return null;
        }
    }
}