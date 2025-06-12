using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager enemy_count;
    private List<GameObject> enemys = new();

    private void Awake()
    {
        if(enemy_count == null)
        {
            enemy_count = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RegisterEnemy(GameObject enemy)
    {
        if(!enemys.Contains(enemy))
        {
            enemys.Add(enemy);
        }
    }

}
