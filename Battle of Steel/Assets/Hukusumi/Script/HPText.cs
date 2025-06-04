using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPText : MonoBehaviour
{
    private float _myHp = 500.0f;
    private Text _text;
    private float P;

    float Max;
    float BHP = 0.0f;
    private void Start()
    {
        //StartCoroutine(IncrementCoroutine());

        _text = this.GetComponent<Text>();
        Max = _myHp;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _myHp--;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _myHp++;
        }

        P = _myHp / Max;
        if (_myHp / Max < 0)
        {
            P = 0.0f;
            _myHp = 0.0f;
        }
        else if (_myHp / Max > 1.0f)
        {
            P = 1.0f;
            _myHp = Max;
        }
        if ( P <= 0)
        {
            _text.color = new Color32(0, 0, 0, 255);
            Debug.LogWarning("Textがアサインされていません！");
        }
        else if (P <= 0.2)
        {
            _text.color = new Color32(235, 33, 13, 255);
        }
        else if (P <= 0.4)
        {
            _text.color = new Color32(184, 235, 13, 255);
        }
        else
        {
            _text.color = new Color32(13, 235, 69, 255);
        }
    }

    //private IEnumerator IncrementCoroutine()
    //{
    //    while (BHP < _myHp)
    //    {
    //        BHP++;
    //        Debug.Log("Current Value: " + BHP);
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}
}