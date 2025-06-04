using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class RandomText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text uiText; //UI�e�L�X�g
    private string fullText; //UI�e�L�X�g�̓��e���擾
    public float totalRevealTime = 2f;//�S�Ă̕������\�������܂ł̍��v����
    private float revealSpeed; //1�������Ƃ̕\�����x
    private Coroutine revealCoroutine;
    void Awake()
    {
        uiText = GetComponent<Text>(); //UI�e�L�X�g���擾
        fullText = uiText.text; //UI�e�L�X�g�̓��e���擾
        uiText.text = ""; //������Ԃ͋�
        revealSpeed = totalRevealTime / fullText.Length; //�e�����̕\���Ԋu���v�Z
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        revealCoroutine = StartCoroutine(RevealText()); //�z�o�[���ɕ\���J�n
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine); //�r���Œ�~
        }
        uiText.text = ""; //��\���ɖ߂�
    }
    IEnumerator RevealText()
    {
        uiText.text = ""; //������
        List<char> characters = new List<char>(fullText.ToCharArray()); //�e�L�X�g�����X�g��
        List<char> displayedChars = new List<char>(); //�\���ς݂̕������X�g
        while (displayedChars.Count < fullText.Length)
        {
            int randomIndex = Random.Range(0, characters.Count); //�����_���Ȉʒu���擾
            displayedChars.Add(characters[randomIndex]); // ����ǉ�
            characters.RemoveAt(randomIndex); //�g�p�ς݂̕������폜
            uiText.text = new string(displayedChars.ToArray()); //�X�V
            yield return new WaitForSeconds(revealSpeed); //�v�Z���ꂽ�Ԋu�őҋ@
        }
    }
}