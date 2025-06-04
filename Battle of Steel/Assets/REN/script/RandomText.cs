using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class RandomText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text uiText; //UIテキスト
    private string fullText; //UIテキストの内容を取得
    public float totalRevealTime = 2f;//全ての文字が表示されるまでの合計時間
    private float revealSpeed; //1文字ごとの表示速度
    private Coroutine revealCoroutine;
    void Awake()
    {
        uiText = GetComponent<Text>(); //UIテキストを取得
        fullText = uiText.text; //UIテキストの内容を取得
        uiText.text = ""; //初期状態は空白
        revealSpeed = totalRevealTime / fullText.Length; //各文字の表示間隔を計算
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        revealCoroutine = StartCoroutine(RevealText()); //ホバー時に表示開始
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine); //途中で停止
        }
        uiText.text = ""; //非表示に戻す
    }
    IEnumerator RevealText()
    {
        uiText.text = ""; //初期化
        List<char> characters = new List<char>(fullText.ToCharArray()); //テキストをリスト化
        List<char> displayedChars = new List<char>(); //表示済みの文字リスト
        while (displayedChars.Count < fullText.Length)
        {
            int randomIndex = Random.Range(0, characters.Count); //ランダムな位置を取得
            displayedChars.Add(characters[randomIndex]); // 字を追加
            characters.RemoveAt(randomIndex); //使用済みの文字を削除
            uiText.text = new string(displayedChars.ToArray()); //更新
            yield return new WaitForSeconds(revealSpeed); //計算された間隔で待機
        }
    }
}