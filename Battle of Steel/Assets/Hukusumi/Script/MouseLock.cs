using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLock : MonoBehaviour
{
    private float get_hp;//現在の体力
    public PlayerController Game_Manager;//プレイヤー取得

    // Update is called once per frame
    void Update()
    {
        get_hp=Player_Status.Player_HP;//現在値取得
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
