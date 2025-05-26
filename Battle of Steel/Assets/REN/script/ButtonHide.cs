using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ButtonHide : MonoBehaviour
{
    public Button button;
    public float blinkDuration = 1f; // 点滅する時間（秒）
    public float blinkSpeed = 0.1f; // 点滅の間隔（秒）
    private Image buttonImage;
    void Start()
    {
        buttonImage = button.GetComponent<Image>();
        StartCoroutine(BlinkEffect());
    }
    IEnumerator BlinkEffect()
    {
        float elapsedTime = 0;
        while (elapsedTime < blinkDuration)
        {
            buttonImage.enabled = !buttonImage.enabled; // 表示/非表示の切り替え
            yield return new WaitForSeconds(blinkSpeed);
            elapsedTime += blinkSpeed;
        }
        buttonImage.enabled = true; // 最後に表示状態に戻す
    }
}