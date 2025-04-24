using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] GameObject player;
    public float distance; // �J�����ƃv���C���[�Ԃ̋���
    public float height; // �J�����̍���
    public float smoothSpeed; // �J�����̉�]���x

    void Update()
    {
        // �}�E�X�̈ړ��ʂ��擾
        float my = Input.GetAxis("Mouse Y");
        float mx = Input.GetAxis("Mouse X");

        // X�����Ɉ��ʈړ����Ă���Ή���]
        //0.0000001f�͊��炩��
        if (Mathf.Abs(mx) > 0.0000001f)
        {
            mx = mx * 5;

            // ��]���̓��[���h���W��Y��
            transform.RotateAround(player.transform.position, Vector3.up, mx);

        }

        // Y�����Ɉ��ʈړ����Ă���Ώc��]
        if (Mathf.Abs(my) > 0.0000001f)
        {
            //�����̐���
            if ((height - my) < -2 || (height - my) > 4)
            {
                my = 0;
            }
            height -= my / 10;
        }
    }

    void LateUpdate()
    {
        // �v���C���[�̒��S�ʒu���v�Z
        Vector3 playerCenter = player.transform.position + Vector3.up * height;

        // �v���C���[�̌��Ɉʒu����^�[�Q�b�g�ʒu���v�Z
        Vector3 targetPosition = playerCenter - player.transform.forward * distance;


        // �J�����̈ʒu�����炩�ɍX�V
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // �J�����͏�Ƀv���C���[������
        transform.LookAt(player.transform);
    }
}
