using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ButtonBlinkEffect : MonoBehaviour
{
    public Button button;
    public float hiddenDuration = 1f; // 非表示時間（1秒）
    public float blinkDuration = 1f; // 点滅時間（1秒）
    public float blinkSpeed = 0.1f; // 点滅の間隔（0.1秒）
    private Image buttonImage;
    void Start()
    {
        buttonImage = button.GetComponent<Image>();
        buttonImage.enabled = false; // 最初は非表示
        StartCoroutine(BlinkSequence());
    }
    IEnumerator BlinkSequence()
    {
        yield return new WaitForSeconds(hiddenDuration); // 1秒待つ
        StartCoroutine(BlinkEffect());
    }
    IEnumerator BlinkEffect()
    {
        float elapsedTime = 0;
        while (elapsedTime < blinkDuration)
        {
            buttonImage.enabled = !buttonImage.enabled; // 表示/非表示切り替え
            yield return new WaitForSeconds(blinkSpeed);
            elapsedTime += blinkSpeed;
        }
        buttonImage.enabled = true; // 最後に通常表示へ戻す
    }
}