using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController: MonoBehaviour
{
    [SerializeField] GameObject player;

    Vector3 currentPos;//現カメラ位置
    Vector3 pastPos;//前カメラ位置

    Vector3 diff;//移動距離
    void Start()
    {
        pastPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //------カメラの移動------

        //プレイヤーの現在地の取得
        currentPos = player.transform.position;

        diff = currentPos - pastPos;

        transform.position = Vector3.Lerp(transform.position, transform.position + diff, 1.0f);//カメラをプレイヤーの移動差分だけうごかすよ

        pastPos = currentPos;


        //------カメラの回転------

        // マウスの移動量を取得
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // X方向に一定量移動していれば横回転
        if (Mathf.Abs(mx*200) > 0.01f)
        {
            // 回転軸はワールド座標のY軸
            transform.RotateAround(player.transform.position, Vector3.up, mx);
        }

        // Y方向に一定量移動していれば縦回転
        if (Mathf.Abs(my*200) > 0.01f)
        {
            // 回転軸はカメラ自身のX軸
            transform.RotateAround(player.transform.position, transform.right, -my);
        }
    }
}
