using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneChange : MonoBehaviour
{
    public Button button; //UI�̃{�^��
    public string sceneName = "NextScene"; //�؂�ւ���̃V�[����
    void Start()
    {
        button.onClick.AddListener(ChangeScene);
    }
    void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}