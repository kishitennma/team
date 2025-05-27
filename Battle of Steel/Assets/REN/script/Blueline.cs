using UnityEngine;
public class Blueline : MonoBehaviour
{
    public RectTransform imageTransform;
    public Vector2 startPos; // �����ʒu
    public Vector2 endPos;   // �ڕW�ʒu
    public float speed = 2f; // �ړ����x
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