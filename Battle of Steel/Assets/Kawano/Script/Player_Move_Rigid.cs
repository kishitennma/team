using Unity.VisualScripting;
using UnityEngine;

public class Player_Move_Rigid : MonoBehaviour
{
    [Header("�ړ����x")]
    [SerializeField] float move_speed;//�L�����N�^�[�̈ړ����x
    [SerializeField] float dash_speed;//�_�b�V���␳���x
    public Animator player_animator;//�v���C���[�̃A�j���[�V�����R���g���[���[

    private Rigidbody rb;//RigidBody
    private float move_x, move_y;//Animation
    private float target_x, target_y;
    private Vector3 input_direction;//���͕���

    //RigidBody�x�[�X�̃v���C���[�̈ړ����@

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        move_x = 0; move_y = 0;//������
        //�J������]
        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);
        mx = 0;//������
   
        //������Ă���{�^���ɂ���āA�A�j���[�V�����𐧌�
        if (Input.GetKey(KeyCode.W))
            target_y = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            target_y = -1.0f;
        if (Input.GetKey(KeyCode.D))
            target_x = 1.0f;
        else if (Input.GetKey(KeyCode.A))
            target_x = -1.0f;


        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            target_y = 0.0f;//blend tree�̐��l���f�t�H���g�̏�Ԃɖ߂�

        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            target_x = 0.0f;//blend tree�̐��l���f�t�H���g�̏�Ԃɖ߂�
        }

        //�����Ő��l����`��Ԃ��āA�Ȃ߂炩�ɂ���
        move_x = Mathf.Lerp(player_animator.GetFloat("Horizontal"), target_x, Time.deltaTime * 10f);
        move_y = Mathf.Lerp(player_animator.GetFloat("Vertical"), target_y, Time.deltaTime * 10f);
        //�A�j���[�^�[�̃p�����[�^�ɒl����
        player_animator.SetFloat("Horizontal", move_x);
        player_animator.SetFloat("Vertical", move_y);

    }
    //�ړ������������ɋL�q(�����̓��e��FixedUpdate�ȊO�ł͋������o�O��)
    private void FixedUpdate()
    {
        //���͂��󂯎��
        float h = Input.GetAxis("Horizontal");//��
        float v = Input.GetAxis("Vertical");//�c

        Vector3 move_dir = (transform.right * h + transform.forward * v).normalized;   //�����ݒ�
        Vector3 origin = transform.position + Vector3.up * 1.5f;                       //���S�_�̏���������
        float move_distance = move_speed * Time.fixedDeltaTime;                        //�ړ������ݒ�
        float radius = 0.8f;                                                           //SpeheCast�p�ɔ��a�ݒ�

        //�ړ�������RigidBody�����̃I�u�W�F�N�g����������
        if (Physics.SphereCast(origin,radius, move_dir,out RaycastHit hit,  move_distance + 0.1f))
        {
            move_dir = Vector3.ProjectOnPlane(input_direction, hit.normal).normalized / 12;//�����������A������v�Z
        }
        //�ړ����x��ݒ�
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            //�ʏ펞
            player_animator.SetFloat("IsDashing", 0.0f);
            input_direction = move_dir;
        }
        else
        {
            //�_�b�V����
            player_animator.SetFloat("IsDashing", 1.0f);//Animator���_�b�V���ɐ؂�ւ�
            input_direction = move_dir * dash_speed;//�ړ��x�N�g����ݒ�
        }
        //�ړ�������ݒ�
        Vector3 move_offset = input_direction * move_speed * Time.deltaTime;
        rb.MovePosition(rb.position + move_offset);//RigidBody���̂̈ʒu���ړ�

    }

   

    //PlayerController�̊֐��R�s�[
    void Screen_movement(float mx)
    {
        // X�����Ɉ��ʈړ����Ă���Ή���]
        //0.0000001f�͊��炩��
        if (Mathf.Abs(mx) > 0.0000001f)
        {
            mx = mx *10;

            // ��]���̓��[���h���W��Y��
            rb.transform.RotateAround(rb.transform.position, Vector3.up, mx);
        }
        

    }
}