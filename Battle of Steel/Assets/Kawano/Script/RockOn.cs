using UnityEngine;
//���b�N�I���V�X�e��

public class RockOn : MonoBehaviour
{
    [Header("�v���C���[�I�u�W�F�N�g")]
    [SerializeField] GameObject player_object;//�v���C���[

    private GameObject target;//�G

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //�͈͓��ɓ����ăG�l�~�[��������target�ɓ����
        if(other.gameObject.CompareTag("Enemy"))
        {
            target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //�͈͊O�ɏo����target��������
        if(other.gameObject.CompareTag("Enemy"))
        {
            target = null;
        }
    }
    //�^�[�Q�b�g����Ԃ�
    public GameObject GetTarget()
    {
        return this.target;
    }

    private GameObject RockOnPlayerToEnemy()
    {
        float search_radius = 10f;
        //var hits = Physics.SphereCastAll(
        //    player_object.transform.position,
        //    search_radius,
        //    player_object.transform.forward,
        //    0.01f
        //    )


        return null;
    }
}
