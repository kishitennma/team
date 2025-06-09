using UnityEngine;

enum Bullet_type
{
    Null,
    Homing,
    Explosive,
}


public class Bullet_Action : MonoBehaviour
{
    [SerializeField] float life_time;//�e�ۂ̐�������
    [SerializeField] Bullet_type type;
    private void Start()
    {
        Destroy(gameObject,life_time);
    }
    private void Update()
    {
        if(type == Bullet_type.Homing)
        {
            //�z�[�~���O�e
        }
        else if(type == Bullet_type.Explosive)
        {
            //�����e

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("��������");
            Destroy(gameObject);
        }
    }

}