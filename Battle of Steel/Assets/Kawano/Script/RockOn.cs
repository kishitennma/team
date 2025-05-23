using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
//���b�N�I���V�X�e��

public class RockOn : MonoBehaviour
{
    [Header("�I�u�W�F�N�g")]
    [SerializeField] GameObject player;//�v���C���[�I�u�W�F�N�g
    [Header("���b�N�I���ݒ�")]
    [SerializeField] float lockon_range = 20.0f;//���b�N�I���̋���
    public LayerMask enemy_layer;//�G�̊K�w

    private Transform lockon_target;
    private bool is_lock_on;
    private float rotate_speed = 90;
    private bool lockon_flag = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (!lockon_flag)
                lockon_flag = true;
            else
                lockon_flag = false;
        }

        if(lockon_flag == true)
        {
            if (!is_lock_on )
            {
                Set_LockOn();
                RotateTarget();
            }
            else
            {
                float distance = Vector3.Distance(transform.position, lockon_target.position);
                if (distance > lockon_range || !lockon_target.gameObject.activeInHierarchy)
                {
                    Stop_LockOn();
                    Debug.Log("���b�N�I���Ώۂ��������܂���");
                }
            }
            if (is_lock_on && lockon_target != null)
            {

            }


        }


    }

    //���b�N�I���ݒ�
    private void Set_LockOn()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position,lockon_range);
        Transform clossest = null;
        float minDistance = Mathf.Infinity;

        foreach(Collider hit in hits)
        {
            //�G�l�~�[�^�O�Ȃ�
            if(hit.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    clossest = hit.transform;
                }
            }
        }
        if(clossest != null)
        {
            lockon_target = clossest;
            is_lock_on = false;
        }
    }
    //���b�N�I�����~
    private void Stop_LockOn()
    {
        lockon_target = null;
        is_lock_on = false;
        lockon_flag = false;
    }

    void RotateTarget()
    {
        Vector3 direction = (lockon_target.position - transform.position);
        direction.y = 0;//���������̂�
        if(direction != Vector3.zero)
        {
            Quaternion lockrotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lockrotation, Time.deltaTime * rotate_speed);
            transform.rotation = lockrotation;
        }
    }
}