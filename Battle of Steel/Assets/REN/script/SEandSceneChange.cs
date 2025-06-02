using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class SEandSceneChange : MonoBehaviour
{
    public Button button; //ボタン
    public AudioSource audioSource; //効果音用 AudioSource
    public AudioClip soundClip; //再生する効果音
    public string sceneName = "切り替え先のシーン名"; //切り替え先のシーン名
    public float delayBeforeSceneChange = 0.5f; //効果音を鳴らしてからシーン遷移までの待機時間（秒）
    void Start()
    {
        button.onClick.AddListener(PlaySoundAndChangeScene);
    }
    void PlaySoundAndChangeScene()
    {
        audioSource.PlayOneShot(soundClip); //効果音を鳴らす
        StartCoroutine(ChangeSceneAfterDelay()); //指定時間待ってシーン遷移
    }
    IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeSceneChange); //効果音を鳴らした後に待機
        SceneManager.LoadScene(sceneName); //シーン遷移
    }
}