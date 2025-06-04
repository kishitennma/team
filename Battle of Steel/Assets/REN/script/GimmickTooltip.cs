using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;
public class GimmickTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text uiText;
    [SerializeField] private string tooltipText; //�e����̐����e�L�X�g
    [SerializeField] private Font customFont; //�ݒ肵�����t�H���g
    [SerializeField] private float glitchDuration = 0.5f; //�����_���p�����\������
    [SerializeField] private float revealDuration = 1.5f; //���X�ɓ��{��ɕς�鎞��
    private Coroutine revealCoroutine;
    void Awake()
    {
        uiText = GetComponent<Text>();
        uiText.text = ""; //������Ԃ͋�
        if (customFont != null)
            uiText.font = customFont; //�t�H���g��ݒ�
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("�J�[�\�����{�^���ɓ���܂����I"); //�m�F�p���O
        revealCoroutine = StartCoroutine(GlitchEffect());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("�J�[�\�����{�^�����痣��܂����I"); //�m�F�p���O
        if (revealCoroutine != null) StopCoroutine(revealCoroutine);
        uiText.text = ""; //��\��
    }
    private IEnumerator GlitchEffect()
    {
        float elapsedTime = 0;
        while (elapsedTime < glitchDuration)
        {
            elapsedTime += Time.deltaTime;
            uiText.text = GenerateRandomText(tooltipText.Length);
            yield return new WaitForSeconds(0.05f);
        }
        yield return RevealText(); //���{��֕ω�
    }
    private IEnumerator RevealText()
    {
        uiText.text = ""; //������
        string currentText = new string('_', tooltipText.Length);
        char[] revealArray = tooltipText.ToCharArray();
        for (int i = 0; i < revealArray.Length; i++)
        {
            yield return new WaitForSeconds(revealDuration / tooltipText.Length);
            currentText = currentText.Remove(i, 1).Insert(i, revealArray[i].ToString());
            uiText.text = currentText;
        }
    }
    private string GenerateRandomText(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }
}