using UnityEngine;

public class Pos_Seting : MonoBehaviour
{
    public GameObject Set_Pos;
    public GameObject Get_Pos;

    // Update is called once per frame
    void Update()
    {
        Set_Pos.transform.position = Get_Pos.transform.position;
    }
}
