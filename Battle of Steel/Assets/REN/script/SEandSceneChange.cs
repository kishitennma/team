using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class SEandSceneChange : MonoBehaviour
{
    public Button button; //�{�^��
    public AudioSource audioSource; //���ʉ��p AudioSource
    public AudioClip soundClip; //�Đ�������ʉ�
    public string sceneName = "�؂�ւ���̃V�[����"; //�؂�ւ���̃V�[����
    public float delayBeforeSceneChange = 0.5f; //���ʉ���炵�Ă���V�[���J�ڂ܂ł̑ҋ@���ԁi�b�j
    void Start()
    {
        button.onClick.AddListener(PlaySoundAndChangeScene);
    }
    void PlaySoundAndChangeScene()
    {
        audioSource.PlayOneShot(soundClip); //���ʉ���炷
        StartCoroutine(ChangeSceneAfterDelay()); //�w�莞�ԑ҂��ăV�[���J��
    }
    IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeSceneChange); //���ʉ���炵����ɑҋ@
        SceneManager.LoadScene(sceneName); //�V�[���J��
    }
}