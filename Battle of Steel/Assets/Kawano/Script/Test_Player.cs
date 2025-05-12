using UnityEngine;

//�v���C���[�X�N���v�g�e�X�g�p�@��ō폜
public class Test_Player : MonoBehaviour
{
    [SerializeField] GameObject player;

    public GameObject Cam;
    public Animator animator; // �L�����N�^�[�I�u�W�F�N�g��Animator


    public float moveSpeed = 30.0f; // �L�����N�^�[�̈ړ����x

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
        animator.SetBool("Run", true);

        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("�_�b�V��");
            //�v���C���[�̐��ʂɌ������Ĉړ�����
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            //�v���C���[�̐��ʂɌ������Ĉړ�����
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetBool("Run", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * -moveSpeed * Time.deltaTime;
            animator.SetBool("Run", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += transform.right * -moveSpeed * Time.deltaTime;
            animator.SetBool("Run", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            //animator.SetBool("Run", true);
        }
        animator.SetBool("Run", false);
    }

}