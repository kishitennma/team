using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    public Rigidbody rb;
    public GameObject Cam;
    public Animator animator; // �L�����N�^�[�I�u�W�F�N�g��Animator
    private bool jump_flag = false;
    private bool step_flag = true;
    public float jumppower;
    public float step_power;

    public int attack_power = 0;
    public int hp = 100;

    private float NormalizeTime;
    private float move_x, move_y;//�ړ�����
    private float target_x, target_y;//���`�ۊǗp


    public float moveSpeed = 30.0f; // �L�����N�^�[�̈ړ����x


    public bool damaged;

    /// <summary>
    /// �W�����v�̃t���O����
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (jump_flag == true)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                jump_flag = false;
            }
        }
    }


    private void Start()
    {
        animator = GetComponent<Animator>();
        damaged = false;
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
        
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))//�V�t�g�ƕ����L�[�������������ꂽ�Ƃ�
        {
            if (step_flag == true)//step_flag��true�̎��X�e�b�v�ړ����\�ɂ���
            {
                transform.Translate(0, 0, step_power);
                step_flag = false;//step_flag��flase�ɂ��ē����Ȃ��悤�ɂ���
            }
            animator.SetFloat("IsDashing", 1.0f);//blend tree��Dash�ɐ؂�ւ���
            target_y = 1.0f;//blend tree����
            transform.position += transform.forward * (moveSpeed * 2.0f) * Time.deltaTime;//�v���C���[�ړ�
            AddForce_reset();//�W�����v���Ă����ꍇAddforce�̗͂�0�ɂ���
           
        }
        else if (Input.GetKey(KeyCode.W))//�����L�[������������Ă����ꍇ
        {
            animator.SetFloat("IsDashing", 0.0f);//blend tree��Normal�ɂ���
            target_y = 1.0f;//blend tree����
            transform.position += transform.forward * (moveSpeed) * Time.deltaTime;//�v���C���[�ړ�
            AddForce_reset();//�W�����v���Ă����ꍇAddforce�̗͂�0�ɂ���
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            if (step_flag == true)
            {
                transform.Translate(0, 0, -step_power);
                step_flag = false;
            }
            animator.SetFloat("IsDashing", 1.0f);
            target_y = -1.0f;
            transform.position += transform.forward * -(moveSpeed * 2.0f) * Time.deltaTime;
            AddForce_reset();
           
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetFloat("IsDashing", 0.0f);
            target_y = -1.0f;
            transform.position += transform.forward * -(moveSpeed ) * Time.deltaTime;
            AddForce_reset();
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
        {
            if (step_flag == true)
            {
                transform.Translate(step_power, 0, 0);
                step_flag = false;
            }
            animator.SetFloat("IsDashing", 1.0f);
            target_x = 1.0f;
            transform.position += transform.right * (moveSpeed*2.0f) * Time.deltaTime;
            AddForce_reset();
          
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetFloat("IsDashing", 0.0f);
            target_x = 1.0f;
            transform.position += transform.right * (moveSpeed) * Time.deltaTime;
            AddForce_reset();
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        {
            if (step_flag == true)
            {
                transform.Translate(-step_power, 0, 0);
                step_flag = false;
            }
            animator.SetFloat("IsDashing", 1.0f);
            target_x = -1.0f;
            transform.position += transform.right * -(moveSpeed * 2.0f) * Time.deltaTime;
            AddForce_reset();
           
            
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetFloat("IsDashing", 0.0f);
            target_x = -1.0f;
            transform.position += transform.right * -(moveSpeed) * Time.deltaTime;
            AddForce_reset();
        }
       

        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            target_y = 0.0f;//blend tree�̐��l���f�t�H���g�̏�Ԃɖ߂�
           
        }
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            target_x = 0.0f;//blend tree�̐��l���f�t�H���g�̏�Ԃɖ߂�

        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            step_flag = true;//�V�t�g�𗣂����ꍇsstep_flag��true�ɂ���
        }


        //�����Ő��l����`��Ԃ��āA�Ȃ߂炩�ɂ���
        move_x = Mathf.Lerp(animator.GetFloat("Horizontal"), target_x, Time.deltaTime * 10f);
        move_y = Mathf.Lerp(animator.GetFloat("Vertical"), target_y, Time.deltaTime * 10f);
        //�A�j���[�^�[�̃p�����[�^�ɒl����
        animator.SetFloat("Horizontal", move_x);
        animator.SetFloat("Vertical", move_y);

        if (Input.GetKeyDown(KeyCode.Space))//�X�y�[�X�������ꂽ��W�����v����
        {
            Debug.Log("Jump");
            rb.AddForce(transform.up * jumppower, ForceMode.Impulse);//������ɗ͂�������
            jump_flag = true;//
        }

      



    }

 
    /// <summary>
    /// Addforce�̗͂�0�ɂ���
    /// </summary>
    void AddForce_reset()
    {
        if (jump_flag == true)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

}