using UnityEngine;

public class EnemyBulletAction : MonoBehaviour
{
    [SerializeField] float life_time;//�e�ۂ̐�������

    private void Start()
    {
        Destroy(gameObject, life_time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("��������");
            Destroy(gameObject);
        }
    }

}
