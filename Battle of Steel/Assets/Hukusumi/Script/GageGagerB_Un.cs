using UnityEngine;
using UnityEngine.UI;

public class GageGagerB_Un : MonoBehaviour
{
    private Image image;
    public GameObject Boost;//�Q�[�W�l�擾
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Boost = GameObject.Find("BoostBar");
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //�_�Ŏ��F�ύX
        if (Boost.GetComponent<GageGagerB>().Zero)
        {
            image.color = new Color32(255, 0, 0, 54);
        }
        else
        {
            image.color = new Color32(255, 255, 255, 54);
        }
    }
}
