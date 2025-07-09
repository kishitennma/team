using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poze : MonoBehaviour
{
    [SerializeField] public GameObject poze;
    bool C_Check = false;//˜A‘Å–hŽ~
    bool C_Poze = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        poze.SetActive(C_Poze);
        if (Input.GetKey(KeyCode.Escape))
        {
            if (C_Check == false)
            {
                C_Check = true;
                C_Poze =! C_Poze;
            }
        }
        else
        {
            C_Check = false;
        }
    }
}
