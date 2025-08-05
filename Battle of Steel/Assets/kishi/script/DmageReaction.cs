using UnityEngine;
using UnityEngine.UI;


public class DmageReaction : MonoBehaviour
{
    [SerializeField] Image DmgImg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DmgImg.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        DmgImg.color = Color.Lerp(DmgImg.color,Color.clear, Time.deltaTime);
        Damaged();


    }

    void Damaged()
    {
        if(Input.GetKey(KeyCode.L))
        {
            DmgImg.color = new Color(0.7f, 0, 0, 0.7f);
            Debug.Log("damaged");
        }
            
        
    }
}
