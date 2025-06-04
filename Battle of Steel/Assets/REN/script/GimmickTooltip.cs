using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;
public class GimmickTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text uiText;
    [SerializeField] private string tooltipText; //各武器の説明テキスト
    [SerializeField] private Font customFont; //設定したいフォント
    [SerializeField] private float glitchDuration = 0.5f; //ランダム英数字表示時間
    [SerializeField] private float revealDuration = 1.5f; //徐々に日本語に変わる時間
    private Coroutine revealCoroutine;
    void Awake()
    {
        uiText = GetComponent<Text>();
        uiText.text = ""; //初期状態は空白
        if (customFont != null)
            uiText.font = customFont; //フォントを設定
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("カーソルがボタンに入りました！"); //確認用ログ
        revealCoroutine = StartCoroutine(GlitchEffect());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("カーソルがボタンから離れました！"); //確認用ログ
        if (revealCoroutine != null) StopCoroutine(revealCoroutine);
        uiText.text = ""; //非表示
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
        yield return RevealText(); //日本語へ変化
    }
    private IEnumerator RevealText()
    {
        uiText.text = ""; //初期化
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