using UnityEngine;

public class Bullet_Action : MonoBehaviour
{
    [SerializeField] float life_time;

    private void Start()
    {
        Destroy(gameObject,life_time);
    }
}
