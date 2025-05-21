using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GageGagerHP : MonoBehaviour
{
    private float _myHp = 500.0f;
    private Image _image;

    float Max;
    private void Start()
    {
        _image = this.GetComponent<Image>();
        Max=_myHp;
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

        _image.fillAmount = _myHp / Max;
        if(_myHp / Max <0)
        {
            _image.fillAmount = 0.0f;
            _myHp = 0.0f;
        }
        else if(_myHp / Max >1.0f)
        {
            _image.fillAmount = 1.0f;
            _myHp = Max;
        }

        if (_image.fillAmount <= 0.2)
        {
            _image.color = new Color32(235, 33, 13, 255);
        }
        else if (_image.fillAmount <= 0.4)
        {
            _image.color = new Color32(184, 235, 13, 255);
        }
        else
        {
            _image.color = new Color32(13, 235, 69, 255);
        }
    }
}