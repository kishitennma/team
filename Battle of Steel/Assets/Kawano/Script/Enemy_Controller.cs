using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    [Header("ステータス")]
    [SerializeField]public int HP = 0;
    [SerializeField] public int Attack = 0;
    [SerializeField] public int Deffence = 0;
    Animation Animation;

    void Start()
    {
        
    }

    void Update()
    {
        //体力が0なら消滅
        if(HP < 0)
        {
            Destroy(this);
        }



    }
}