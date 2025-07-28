using UnityEngine;

public class Set_FPS : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        Application.targetFrameRate = 120;

    }
}