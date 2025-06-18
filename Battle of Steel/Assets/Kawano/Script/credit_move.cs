using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class credit_move : MonoBehaviour
{
    public GameObject[] credit_list;
    public Text page_text;
    private int draw_int;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        page_text.text = "D:次のページに進む";
        draw_int = 1;
        credit_list[0].SetActive(true);
        credit_list[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            draw_int++;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            draw_int--;
        }
        
        if (draw_int > credit_list.Count())
        {
            draw_int = credit_list.Count();
        }
        else if (draw_int < 1)
        {
            draw_int = 1;
        }
        Draw_Credit_Status(draw_int);
    }

    void Draw_Credit_Status(int count)
    {
        switch (count)
        {
            case 1:
                {
                    page_text.text = "D:次のページに進む";
                    credit_list[0].SetActive(true);
                    credit_list[1].SetActive(false);
                    credit_list[2].SetActive(false);
                }
                break;
            case 2:
                {
                    page_text.text = "A:前のページに戻る、D:次のページに進む";
                    credit_list[0].SetActive(false);
                    credit_list[1].SetActive(true);
                    credit_list[2].SetActive(false);

                }
                break;
            case 3:
                {
                    page_text.text = "A:前のページに戻る";
                    credit_list[0].SetActive(false);
                    credit_list[1].SetActive(false);
                    credit_list[2].SetActive(true);
                }
                break;
        }
    }
}