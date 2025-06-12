using UnityEngine;

public class Set_E_Count : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemy_Manager.enemy_count.RegisterEnemy(gameObject);
    }
    private void OnDestroy()
    {
     if(Enemy_Manager.enemy_count!= null)
        {
            Enemy_Manager.enemy_count.UnregisterEnemy(gameObject);
        }
    }
}