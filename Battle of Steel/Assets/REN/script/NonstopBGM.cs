using UnityEngine;
using UnityEngine.SceneManagement;
public class NonstopBGM : MonoBehaviour
{
    public string[] persistentScenes = new string[8]; //BGM���ێ�����V�[�����̔z��
    private AudioSource audioSource;
    private static NonstopBGM instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //�V�[���ύX�����I�u�W�F�N�g��ێ�
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded; //�V�[���ǂݍ��ݎ��̏�����o�^
        }
        else
        {
            Destroy(gameObject); //���łɑ��݂���ꍇ�A�V�������̂�j��
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //�ݒ肳�ꂽ�V�[���Ȃ牽�����Ȃ��iBGM�ێ��j
        if (System.Array.Exists(persistentScenes, s => s == scene.name))
        {
            return;
        }
        //�ݒ�O�̃V�[���Ȃ�폜
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //�V�[���ύX���̃C�x���g����
    }
}