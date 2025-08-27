using UnityEngine;

public class Set_FPS : MonoBehaviour
{
    public static int FPS_Value = 120;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        Application.targetFrameRate = FPS_Value;

    }
}