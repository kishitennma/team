using UnityEngine;

public class EnemyBulletAction : MonoBehaviour
{
    [SerializeField] float life_time;//�e�ۂ̐�������
    public GameObject explosive_effect;//�����G�t�F�N�g
    public int attack_damage = 0;//�e�ۂ̃_���[�W
    private Damage_Calclate cal;//�_���[�W�v�Z

    private void Start()
    {
        GameObject calc = GameObject.Find("Game_Manager");
        cal = calc.GetComponent<Damage_Calclate>();
        Destroy(gameObject, life_time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //�e�ۂ��v���C���[�ɓ���������̗͂����炷
            GameObject explosive = Instantiate(explosive_effect, gameObject.transform.position, Quaternion.identity);
            Debug.Log("���݂̗̑�" +Player_Status.Player_HP );
            Player_Status.Player_HP = cal.Damage_Cal(attack_damage,Player_Status.Player_HP);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            //�ǂɓ������������
            Destroy(gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //�e�ۂ��v���C���[�ɓ���������̗͂����炷
            GameObject explosive = Instantiate(explosive_effect, gameObject.transform.position, Quaternion.identity);
            Debug.Log("���݂̗̑�" + Player_Status.Player_HP);
            Player_Status.Player_HP = cal.Damage_Cal(attack_damage, Player_Status.Player_HP);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            //�ǂɓ������������
            Destroy(gameObject);
        }

    }
}