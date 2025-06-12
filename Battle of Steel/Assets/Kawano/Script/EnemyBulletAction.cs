using UnityEngine;

public class EnemyBulletAction : MonoBehaviour
{
    [SerializeField] float life_time;//�e�ۂ̐�������

    public int attack_damage;//�e�ۂ̃_���[�W
    private Damage_Calclate cal;

    private void Start()
    {
        Destroy(gameObject, life_time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            cal.Damage_Cal((int)attack_damage, Player_Status.Player_HP);
            Debug.Log("�v���C���[�ɓ�������");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("��������");
            Destroy(gameObject);
        }
    }
}