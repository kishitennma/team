using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    [Header("�X�e�[�^�X")]
    [SerializeField]public int HP = 0;
    [SerializeField] public int Attack = 0;
    [SerializeField] public int Deffence = 0;
    Animation Animation;

    void Start()
    {
        
    }

    void Update()
    {
        //�̗͂�0�Ȃ����
        if(HP < 0)
        {
            Destroy(this);
        }



    }
}