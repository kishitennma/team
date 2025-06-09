using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPText : MonoBehaviour
{
    public float myhp = 500.0f;
    private Text text;
    private int hp=0;

    float max;
    float BHP = 0.0f;
    private void Start()
    {
        //StartCoroutine(IncrementCoroutine());

        text = this.GetComponent<Text>();
        max = myhp;
    }

    private void Update()
    {
        hp= (int)myhp;
        gameObject.GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
        if (Input.GetKey(KeyCode.S))
        {
            myhp--;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            myhp++;
        }

        if (myhp / max < 0)
        {
            myhp = 0.0f;
        }
        else if (myhp / max > 1.0f)
        {
            myhp = max;
        }
        if (myhp / max <= 0)
        {
            text.color = new Color32(0, 0, 0, 255);
        }
        else if (myhp / max <= 0.2)
        {
            text.color = new Color32(235, 33, 13, 255);
        }
        else if (myhp / max <= 0.4)
        {
            text.color = new Color32(184, 235, 13, 255);
        }
        else
        {
            text.color = new Color32(13, 235, 69, 255);
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