using UnityEngine;

public class Bullet_Action : MonoBehaviour
{
    [SerializeField] float life_time;//�e�ۂ̐�������

    private void Start()
    {
        Destroy(gameObject,life_time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}