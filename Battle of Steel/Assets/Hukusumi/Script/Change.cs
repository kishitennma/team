using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Change : MonoBehaviour
{
    public float CKS = 1;
    public bool C=false;
    RectTransform RectTransform_get;
    int x, y;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        RectTransform_get = gameObject.GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (C == false)
        {
            RectTransform_get.Rotate(new Vector3(0, 0, CKS));
            RectTransform rectTransform = GetComponent<RectTransform>();
            float rotationX = rectTransform.eulerAngles.x;
            float rotationY = rectTransform.eulerAngles.y;
            float rotationZ = rectTransform.eulerAngles.z;

            if (rotationZ > 180f) // �Ⴆ��45�x�ȏ�Ȃ��������
            {
                RectTransform_get.rotation = Quaternion.Euler(rotationX, rotationY, 0);
                C = true;

            }

        }
    }
}
