using UnityEngine;
using UnityEngine.UI;
public class Scrolld_bar : MonoBehaviour
{
    [SerializeField]public Scrollbar scrollbar;
    public Text Credit_Text;

    private float start_posY = 0;
    private float move_posY = 0;
    private Transform move_pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scrollbar.value = 0;
        start_posY = Credit_Text.transform.position.y;//èâä˙ÇÃà íuÇê›íË 
    }

    // Update is called once per frame
    void Update()
    {
        move_posY = start_posY - (scrollbar.value*100);
        move_pos.position = new(scrollbar.transform.position.x,move_posY, scrollbar.transform.position.z);
        Credit_Text.transform.position = move_pos.position;
    }
}
