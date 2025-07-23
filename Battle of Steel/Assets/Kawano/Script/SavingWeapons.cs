using JetBrains.Annotations;
using UnityEngine;

public class SavingWeapons : MonoBehaviour
{
    public static int[] save_weapons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
