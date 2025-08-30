using UnityEngine;

public class Camera_Rotate : MonoBehaviour
{
    public Transform Player;//プレイヤー
    public float Speed = 100f;
    // Update is called once per frame
    void FixedUpdate()
    {
        // マウスの移動量を取得
        float mx = Input.GetAxis("Mouse X");
        if(Mathf.Abs(mx) > 0.001f)
        {
            Player.Rotate(Vector3.up*mx * Speed * Time.deltaTime);
        }
    }
}