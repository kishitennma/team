using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Fadeloop : MonoBehaviour
{
    public Image uiElement; //UI画像
    public float fadeDuration = 2f; //フェードの時間（秒）
    public float minAlpha = 0f; //最低透明度（0 = 完全透明）
    public float maxAlpha = 1f; //最大透明度（1 = 完全不透明）
    void Start()
    {
        StartCoroutine(FadeLoop());
    }
    IEnumerator FadeLoop()
    {
        while (true) //無限ループでフェードを繰り返す
        {
            yield return StartCoroutine(Fade(minAlpha, maxAlpha)); //フェードイン
            yield return StartCoroutine(Fade(maxAlpha, minAlpha)); //フェードアウト
        }
    }
    IEnumerator Fade(float fromAlpha, float toAlpha)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float easedT = Mathf.SmoothStep(0, 1, t); //イージング処理
            float currentAlpha = Mathf.Lerp(fromAlpha, toAlpha, easedT);
            Color newColor = uiElement.color;
            newColor.a = currentAlpha;
            uiElement.color = newColor;
            yield return null;
        }
    }
}