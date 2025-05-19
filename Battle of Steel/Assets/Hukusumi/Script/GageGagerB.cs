using UnityEngine;
using UnityEngine.UI;

public class GageGagerB : MonoBehaviour
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
        if (_myHp / Max < 0)
        {
            _image.fillAmount = 0.0f;
            _myHp = 0.0f;
        }
        else if (_myHp / Max > 1.0f)
        {
            _image.fillAmount = 1.0f;
            _myHp = Max;
        }
    }
}