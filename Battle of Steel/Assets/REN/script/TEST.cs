using UnityEngine;
using UnityEngine.EventSystems;
public class TEST : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("�J�[�\�����{�^���ɓ���܂����I");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("�J�[�\�����{�^�����痣��܂����I");
    }
}