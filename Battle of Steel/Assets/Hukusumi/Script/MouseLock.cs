using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLock : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))//カーソル有効化
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else//カーソル無効化
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
