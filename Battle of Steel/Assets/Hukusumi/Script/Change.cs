using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Change : MonoBehaviour
{
    public float C_S = 5;// ��]���x
    public bool C_Check=false;//��]�m�F
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
        //�f�o�b�N
        if (Input.GetKey(KeyCode.Q))
        {
            if (C_Check ==true)
            {
                C_Check = false;
            }
        }
        
        //����]
        if (C_Check == false)
        {
            rg.Rotate(new Vector3(0, 0, C_S));
            RectTransform rectTransform = GetComponent<RectTransform>();
            float rotationX = rectTransform.eulerAngles.x;
            float rotationY = rectTransform.eulerAngles.y;
            float rotationZ = rectTransform.eulerAngles.z;

            if (rotationZ > 180f) // �Ⴆ��45�x�ȏ�Ȃ��������
            {
                rg.rotation = Quaternion.Euler(rotationX, rotationY, 0);
                C_Check = true;

            }

        }
    }
}
