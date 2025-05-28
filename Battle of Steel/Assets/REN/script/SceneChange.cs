using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneChange : MonoBehaviour
{
    public Button button; //UIのボタン
    public string sceneName = "NextScene"; //切り替え先のシーン名
    void Start()
    {
        button.onClick.AddListener(ChangeScene);
    }
    void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}