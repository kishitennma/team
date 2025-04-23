using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] GameObject player;

    public GameObject Cam;
    private Animator animator; // �L�����N�^�[�I�u�W�F�N�g��Animator
    public RuntimeAnimatorController walking;
    public RuntimeAnimatorController running;
    public RuntimeAnimatorController standing;

    public float moveSpeed = 5.0f; // �L�����N�^�[�̈ړ����x

    public bool damaged;

    private void Start()
    {
        animator = GetComponent<Animator>();
        damaged = false;
    }

    void Screen_movement(float mx)
    {
        // X�����Ɉ��ʈړ����Ă���Ή���]
        //0.0000001f�͊��炩��
        if (Mathf.Abs(mx) > 0.0000001f)
        {
            mx = mx * 5;

            // ��]���̓��[���h���W��Y��
            player.transform.RotateAround(player.transform.position, Vector3.up, mx);
        }
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);

        if (Input.GetKey(KeyCode.W))
        {
            // "W"�L�[�������ꂽ�Ƃ��̏����������ɋL�q
            print("��������");
            animator.runtimeAnimatorController = walking;

            //�v���C���[�̐��ʂɌ������Ĉړ�����
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.runtimeAnimatorController = standing;
        }
    }
}
