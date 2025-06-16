using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPText : MonoBehaviour
{
    //public float myhp = 500.0f;//�����l�f�o�b�N
    private Text text;
    public PlayerController Game_Manager;//�v���C���[�擾
    public int hp;//hp

    int max;//�ő�l
    float BHP = 0.0f;
    private void Start()
    {
        //StartCoroutine(IncrementCoroutine());

        text = this.GetComponent<Text>();
        max = Player_Status.Player_HP;
    }

    private void Update()
    {
        hp= Player_Status.Player_HP;
        gameObject.GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
        //�f�o�b�N
        //if (Input.GetKey(KeyCode.S))
        //{
        //    hp--;
        //}
        //else if (Input.GetKey(KeyCode.W))
        //{
        //    hp++;
        //}

        //���ߑ΍�
        if (hp / max < 0)
        {
            hp = 0;
        }
        else if (hp / max > 1.0f)
        {
            hp = max;
        }
        //�F�Ǘ�
        if (hp / max <= 0)
        {
            text.color = new Color32(0, 0, 0, 255);
        }
        else if (hp / max < 0.2)
        {
            text.color = new Color32(235, 33, 13, 255);
        }
        else if (hp / max < 0.4)
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