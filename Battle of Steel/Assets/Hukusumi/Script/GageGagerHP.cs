using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GageGagerHP : MonoBehaviour
{
    GameObject HP_Text;//HP�擾
    HPText script;

    private int myhp ;//HP���ݒl
    private Image image;

    int max;//�ő�l
    private void Start()
    {
        HP_Text = GameObject.Find("HP_Text"); //Unity�������I�u�W�F�N�g�̖��O����擾���ĕϐ��Ɋi�[����
        script = HP_Text.GetComponent<HPText>(); //unitychan�̒��ɂ���UnityChanScript���擾���ĕϐ��Ɋi�[����
        image = this.GetComponent<Image>();
        myhp = Player_Status.Player_HP;
        max = Player_Status.Player_HP;
    }

    private void Update()
    {
        myhp = Player_Status.Player_HP;
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    myhp--;
        //}
        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    myhp++;
        //}

        //�Q�[�W�Ǘ�
        image.fillAmount = myhp / max;
        if(myhp / max <0)
        {
            image.fillAmount = 0.0f;
            myhp = 0;
        }
        else if(myhp / max >1.0f)
        {
            image.fillAmount = 1.0f;
            myhp = max;
        }

        //�F�Ǘ�
        if (image.fillAmount <= 0)
        {
            image.color = new Color32(0, 0, 0, 255);
        }
        else if (image.fillAmount < 0.2)
        {
            image.color = new Color32(235, 33, 13, 255);
        }
        else if (image.fillAmount < 0.4)
        {
            image.color = new Color32(184, 235, 13, 255);
        }
        else
        {
            image.color = new Color32(13, 235, 69, 255);
        }
    }


}