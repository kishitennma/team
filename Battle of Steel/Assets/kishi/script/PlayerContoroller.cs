using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;//�L�����N�^�[�I�u�W�F�N�g
    public Rigidbody rb;//�L�����N�^�[�I�u�W�F�N�g��RigidBody
    public GameObject cam;
    public Camera maincam;
    public Animator animator; // �L�����N�^�[�I�u�W�F�N�g��Animator

    public bool jump_flag = true;//�n��ł̃W�����v�t���O
    public bool jump_second = false;//�󒆂ł̃W�������v�t���O
    public float jumppower;

    [SerializeField] float move_speed;//�L�����N�^�[�̈ړ����x
    [SerializeField] float dash_speed;//�_�b�V���␳���x

    private float NormalizeTime;
    private float move_x, move_y;//�ړ�����
    private float target_x, target_y;//���`�ۊǗp
    private Vector3 input_direction;//���͕���

    public int attack_power;
    public float boost = 100.0f;//�u�[�X�g�c��
    public float boost_max = 100.0f;//�u�[�X�g�̏��

    public float target_fov;
    public float fov_changeamount = 10.0f;
    public float min_fov = 60.0f;
    public float max_fov = 90.0f;

    private bool Collision_Hit = false;




    /// <summary>
    /// �W�����v�̃t���O����
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            rb.MovePosition(new Vector3(rb.transform.position.x, rb.transform.position.y + 0.1f, rb.transform.position.z));

        if (jump_flag == false)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                jump_flag = true;
                jump_second = false;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        //���̂����܂��Ă���Ԃ͈ړ��t���O��false
        Collision_Hit = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        //���̂̏Փ˂��������ꂽ��ړ����ĊJ
        Collision_Hit = false;
    }


    private void Start()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// �v���C���[�̉�](X���j
    /// </summary>
    /// <param name="mx"></param>
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

    //void Jump()
    //{
    //    if (jump_flag == true) return;
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        rb.AddForce(transform.up * jumppower, ForceMode.Impulse);
    //        jump_flag = true;
    //    }
    //}
    void Update()
    {

        //�ړ�������������
        move_x = 0; move_y = 0; animator.SetBool("Action", false);
        //�e�ړ������փA�j���[�V�����ω�
        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);

        if (Input.GetKey(KeyCode.W))//�����L�[������������Ă����ꍇ
        {
            target_y = 1.0f;//blend tree���� 
        }
        else if (Input.GetKey(KeyCode.S))
        {
            target_y = -1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            target_x = 1.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            target_x = -1.0f;
        }


        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            target_y = 0.0f;//blend tree�̐��l���f�t�H���g�̏�Ԃɖ߂�

        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            target_x = 0.0f;//blend tree�̐��l���f�t�H���g�̏�Ԃɖ߂�

        }



        //�����Ő��l����`��Ԃ��āA�Ȃ߂炩�ɂ���
        move_x = Mathf.Lerp(animator.GetFloat("Horizontal"), target_x, Time.deltaTime * 10f);
        move_y = Mathf.Lerp(animator.GetFloat("Vertical"), target_y, Time.deltaTime * 10f);
        //�A�j���[�^�[�̃p�����[�^�ɒl����
        animator.SetFloat("Horizontal", move_x);
        animator.SetFloat("Vertical", move_y);

        if (jump_flag && boost < boost_max && animator.GetFloat("IsDashing") != 1.0f)//�n�ʂɗ����Ă���Ƃ��u�[�X�g��
        {

            boost += 0.4f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jump_flag && boost >= 20.0f)//�n�ォ��̃W�����v
        {
            rb.linearVelocity = new Vector3(0, (jumppower * 3.0f), 0);
            boost -= 20.0f;
            jump_flag = false;
        }
        if (Input.GetKeyUp(KeyCode.Space) && !jump_flag)//�󒆂ŃX�y�[�X�L�[�𗣂�������
        {
            jump_second = true;
        }
        else if (Input.GetKey(KeyCode.Space) && boost > 0
        && jump_second && animator.GetFloat("IsDashing") != 1.0f)//�󒆃W�����v(�z�o�[�H�j
        {
            rb.linearVelocity = new Vector3(0, jumppower, 0);
            jump_flag = false;
            boost -= 0.3f;
        }
        if (animator.GetFloat("IsDashing") == 1.0f)
        {
            boost -= 0.1f;
            camera_Fovaway();
        }
        else
        {
            camera_Fovreturn();
        }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
          Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            if (jump_flag == false)
            {
                if (boost > 0.0f)
                {
                    AddForce_reset();
                    rb.useGravity = false;
                }

            }
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) ||
           Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
        {
            rb.useGravity = true;
        }



    }

    private void FixedUpdate()
    {
        //���͂��󂯎��
        float h = Input.GetAxis("Horizontal");//��
        float v = Input.GetAxis("Vertical");//�c
                                            //      Debug.Log(transform.forward);
        Vector3 move_dir = (transform.right * h + transform.forward * v).normalized;   //�����ݒ�
        Vector3 origin = transform.position + Vector3.up * 1.5f;                       //���S�_�̏���������
        float move_distance = move_speed * Time.fixedDeltaTime;                        //�ړ������ݒ�

        //�Փ˂��Ă��Ȃ��Ƃ����݈ړ����x��ݒ�
        if (!Collision_Hit)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                //�ʏ펞
                animator.SetFloat("IsDashing", 0.0f);
                input_direction = move_dir;

            }
            else
            {
                if (boost > 0.0f)
                {
                    //�_�b�V����
                    animator.SetFloat("IsDashing", 1.0f);//Animator���_�b�V���ɐ؂�ւ�
                    input_direction = move_dir * dash_speed;//�ړ��x�N�g����ݒ�

                }
                else
                {

                    //�ʏ펞
                    animator.SetFloat("IsDashing", 0.0f);
                    input_direction = move_dir;
                }

            }
            //�ړ�������ݒ�
            Vector3 move_offset = input_direction * move_speed * Time.deltaTime;
            rb.MovePosition(rb.position + move_offset);//RigidBody���̂̈ʒu���ړ�
        }
      
    }
    /// <summary>
    /// Addforce�̗͂�0�ɂ���
    /// </summary>
    void AddForce_reset()
    {

        rb.linearVelocity = Vector3.zero;

    }

    void camera_Fovaway()
    {
        if (maincam != null)
        {
            maincam.fieldOfView = Mathf.Clamp(maincam.fieldOfView + fov_changeamount*Time.deltaTime*10 , min_fov, max_fov);
            //maincam.fieldOfView = max_fov;
        }
    }
    void camera_Fovreturn()
    {
        if (maincam != null)
        {
            maincam.fieldOfView = Mathf.Clamp(maincam.fieldOfView - fov_changeamount * Time.deltaTime, min_fov, max_fov);
           
        }
    }
}