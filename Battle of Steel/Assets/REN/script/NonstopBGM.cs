using UnityEngine;
using UnityEngine.SceneManagement;
public class NonstopBGM : MonoBehaviour
{
    public string[] persistentScenes = new string[8]; //BGMを維持するシーン名の配列
    private AudioSource audioSource;
    private static NonstopBGM instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //シーン変更時もオブジェクトを保持
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded; //シーン読み込み時の処理を登録
        }
        else
        {
            Destroy(gameObject); //すでに存在する場合、新しいものを破棄
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //設定されたシーンなら何もしない（BGM維持）
        if (System.Array.Exists(persistentScenes, s => s == scene.name))
        {
            return;
        }
        //設定外のシーンなら削除
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //シーン変更時のイベント解除
    }
}