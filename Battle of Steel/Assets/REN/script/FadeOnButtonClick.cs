using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class FadeOnButtonClick : MonoBehaviour
{
    public Image uiElement; //UI画像
    public Button triggerButton; //フェードを開始するボタン
    public float startAlpha = 0f; //スタート時の透明度（0 = 完全透明, 1 = 完全不透明）
    public float endAlpha = 1f; //フィニッシュ時の透明度
    public float fadeDuration = 2f; //透明度変化にかかる時間（秒）
    public float waitTime = 1f; //フェード開始までの待機時間（秒）
    private float elapsedTime = 0;
    void Start()
    {
        //初期透明度設定
        Color startColor = uiElement.color;
        startColor.a = startAlpha;
        uiElement.color = startColor;
        //ボタンが押されたら `StartFade()` を呼び出す
        triggerButton.onClick.AddListener(StartFade);
    }
    void StartFade()
    {
        StartCoroutine(FadeAfterDelay());
    }
    IEnumerator FadeAfterDelay()
    {
        yield return new WaitForSeconds(waitTime); //指定秒数待機
        elapsedTime = 0; //フェード開始時にタイマーをリセット
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; //指定時間内で完了するように調整
            float easedT = Mathf.SmoothStep(0, 1, t); //イージング処理
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, easedT);
            Color newColor = uiElement.color;
            newColor.a = currentAlpha;
            uiElement.color = newColor;
            yield return null;
        }
    }
}