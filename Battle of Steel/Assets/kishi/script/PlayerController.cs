using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float moveSpeedIn;


    Rigidbody playerRb;

    Vector3 moveSpeed;

    Vector3 currentPos;
    Vector3 pastPos;

    Vector3 delta;

    Quaternion playerRot;
    float currentAngularVelocity;

    [SerializeField]
    float maxAngularVelocity = Mathf.Infinity;

    [SerializeField]
    float smoothTime = 0.1f;

    float diffAngle;
    float rotAngle;

    Quaternion nextRot;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        pastPos = transform.position;
    }

    void Update()
    {
      

       
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
      
        Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

      
        moveSpeed = Vector3.zero;

        //�ړ�����
        if (Input.GetKey(KeyCode.W))
        {
            moveSpeed = moveSpeedIn * cameraForward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveSpeed = -moveSpeedIn * cameraRight;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveSpeed = -moveSpeedIn * cameraForward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveSpeed = moveSpeedIn * cameraRight;
        }

       
        Move();

        //����������
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            //playerRb.velocity = Vector3.zero;
            // playerRb.angularVelocity = Vector3.zero;
        }

        //------�v���C���[�̉�]------

        ////���݂̈ʒu
        currentPos = transform.position;

        //�ړ��ʌv�Z
        delta = currentPos - pastPos;
        delta.y = 0;

        //�ߋ��̈ʒu�̍X�V
        pastPos = currentPos;

        if (delta == Vector3.zero)
            return;

        playerRot = Quaternion.LookRotation(delta, Vector3.up);

        diffAngle = Vector3.Angle(transform.forward, delta);

        //Vector3.SmoothDamp��Vector3�^�̒l�����X�ɕω�������
        //Vector3.SmoothDamp (���ݒn, �ړI�n, ref ���݂̑��x, �J�ڎ���, �ō����x);
        rotAngle = Mathf.SmoothDampAngle(0, diffAngle, ref currentAngularVelocity, smoothTime, maxAngularVelocity);

        nextRot = Quaternion.RotateTowards(transform.rotation, playerRot, rotAngle);

        transform.rotation = nextRot;

    }

   
    private void Move()
    {
        //playerRb.AddForce(moveSpeed, ForceMode.Force);

        playerRb.linearVelocity = moveSpeed;
    }
}
