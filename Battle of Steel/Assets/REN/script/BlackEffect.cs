using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class BlackEffect : MonoBehaviour
{
    public Image blackOverlay;  // 黒い画像 (UI Image)
    public float waitTime = 1f; // 最初に待つ時間（1秒）
    public float blinkDuration = 1f; // 点滅する時間（1秒）
    public float blinkSpeed = 0.1f; // 点滅の間隔（0.1秒）
    void Start()
    {
        blackOverlay.enabled = false; // 最初は非表示
        StartCoroutine(BlinkSequence());
    }
    IEnumerator BlinkSequence()
    {
        yield return new WaitForSeconds(waitTime); // 1秒待つ
        float elapsedTime = 0;
        while (elapsedTime < blinkDuration)
        {
            blackOverlay.enabled = !blackOverlay.enabled; // 表示/非表示切り替え
            yield return new WaitForSeconds(blinkSpeed);
            elapsedTime += blinkSpeed;
        }
        blackOverlay.enabled = false; // 点滅後、完全非表示に戻す
    }
}