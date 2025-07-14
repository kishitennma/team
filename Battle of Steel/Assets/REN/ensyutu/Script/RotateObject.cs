using UnityEngine;
public class RotateObject : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 0, 0); //軸を10度ずつ回転（調整可能）
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime); //毎フレーム回転
    }
}