using UnityEngine;
using UnityEngine.EventSystems;
public class TEST : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("カーソルがボタンに入りました！");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("カーソルがボタンから離れました！");
    }
}