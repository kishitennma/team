using UnityEngine;
public class Blueline : MonoBehaviour
{
    public RectTransform imageTransform;
    public Vector2 startPos; // 初期位置
    public Vector2 endPos;   // 目標位置
    public float speed = 2f; // 移動速度
    private float progress = 0;
    void Start()
    {
        imageTransform.anchoredPosition = startPos;
    }
    void Update()
    {
        if (progress < 1)
        {
            progress += Time.deltaTime * speed;
            imageTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
        }
    }
}