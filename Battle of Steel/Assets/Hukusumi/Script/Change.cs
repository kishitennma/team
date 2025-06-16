using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Change : MonoBehaviour
{
    public float C_S = 5;// 回転速度
    public bool C_Check=false;//回転確認
    RectTransform rg;
    int x, y;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rg = gameObject.GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //デバック
        if (Input.GetKey(KeyCode.Q))
        {
            if (C_Check ==true)
            {
                C_Check = false;
            }
        }
        
        //半回転
        if (C_Check == false)
        {
            rg.Rotate(new Vector3(0, 0, C_S));
            RectTransform rectTransform = GetComponent<RectTransform>();
            float rotationX = rectTransform.eulerAngles.x;
            float rotationY = rectTransform.eulerAngles.y;
            float rotationZ = rectTransform.eulerAngles.z;

            if (rotationZ > 180f) // 例えば45度以上なら条件成立
            {
                rg.rotation = Quaternion.Euler(rotationX, rotationY, 0);
                C_Check = true;

            }

        }
    }
}
