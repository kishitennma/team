using UnityEngine;

public class EnemyBulletAction : MonoBehaviour
{
    [SerializeField] float life_time;//íeä€ÇÃê∂ë∂éûä‘

    private void Start()
    {
        Destroy(gameObject, life_time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("ìñÇΩÇ¡ÇΩ");
            Destroy(gameObject);
        }
    }

}
