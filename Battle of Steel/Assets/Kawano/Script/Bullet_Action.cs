using UnityEngine;



public class Bullet_Action : MonoBehaviour
{
    [SerializeField] float life_time;//�e�ۂ̐�������
    public GameObject explosive_obj;//�e�۔����G�t�F�N�g
    private void Start()
    {
        Destroy(gameObject,life_time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            GameObject explosive = Instantiate(explosive_obj, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}