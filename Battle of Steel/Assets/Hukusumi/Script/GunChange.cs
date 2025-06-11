using UnityEngine;
using UnityEngine.UI;

public class GunChange : MonoBehaviour
{
    public int Gun_Num=0;
    RectTransform rectTransform_get;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform_get = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Gun_Num==1)
        {
            //Vector3 pos = rectTransform_get.position;
            //pos.x = 5;
            //pos.y = 5;
            //pos.z = 5;
            //rectTransform_get.position = pos;
            transform.localPosition = new Vector3(283.5f, -112.0f, 0.0f);
        }
        else if(Gun_Num==2)
        {
            transform.localPosition = new Vector3(400.0f, -43.0f, 0.0f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
