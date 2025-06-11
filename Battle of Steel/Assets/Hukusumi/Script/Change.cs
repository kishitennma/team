using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Change : MonoBehaviour
{
    public float C_S = 1;
    public bool C_Check=false;
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
        if (Input.GetKey(KeyCode.V))
        {
            if (C_Check ==true)
            {
                C_Check = false;
            }
        }
        
        if (C_Check == false)
        {
            rg.Rotate(new Vector3(0, 0, C_S));
            RectTransform rectTransform = GetComponent<RectTransform>();
            float rotationX = rectTransform.eulerAngles.x;
            float rotationY = rectTransform.eulerAngles.y;
            float rotationZ = rectTransform.eulerAngles.z;

            if (rotationZ > 180f) // ó·Ç¶ÇŒ45ìxà»è„Ç»ÇÁèåèê¨óß
            {
                rg.rotation = Quaternion.Euler(rotationX, rotationY, 0);
                C_Check = true;

            }

        }
    }
}
