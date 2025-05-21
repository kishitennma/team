using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    public Rigidbody rb;
    public GameObject Cam;
    public Animator animator; // キャラクターオブジェクトのAnimator
    private bool jump_flag = false;
    public float jumppower;
    public float step_power;

    public int attack_power = 0;
    public int hp = 100;

    private float NormalizeTime;
    private float move_x, move_y;//移動方向
    private float target_x, target_y;//線形保管用


    public float moveSpeed = 30.0f; // キャラクターの移動速度


    public bool damaged;


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


    void Screen_movement(float mx)
    {
        // X方向に一定量移動していれば横回転
        //0.0000001fは滑らかさ
        if (Mathf.Abs(mx) > 0.0000001f)
        {
            mx = mx * 5;

            // 回転軸はワールド座標のY軸
            player.transform.RotateAround(player.transform.position, Vector3.up, mx);
        }
    }
    //アニメーション終了関数(Intを0にする)
    public void Anim_Set_End(string anim_name)
    {
        animator.SetInteger(anim_name, 0);
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
        //移動方向を初期化
        move_x = 0; move_y = 0; animator.SetBool("Action", false);
        //各移動方向へアニメーション変化
        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("IsDashing", 1.0f);
            target_y = 1.0f;
            transform.position += transform.forward * (moveSpeed * 2.0f) * Time.deltaTime;
            AddForce_reset();
            rb.AddForce(transform.forward * step_power, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            animator.SetFloat("IsDashing", 0.0f);
            target_y = 1.0f;
            transform.position += transform.forward * (moveSpeed) * Time.deltaTime;
            AddForce_reset();
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("IsDashing", 1.0f);
            target_y = -1.0f;
            transform.position += transform.forward * -(moveSpeed * 2.0f) * Time.deltaTime;
            AddForce_reset();
            rb.AddForce(transform.forward * -step_power, ForceMode.Impulse);
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
            animator.SetFloat("IsDashing", 1.0f);
            target_x = 1.0f;
            transform.position += transform.right * (moveSpeed*2.0f) * Time.deltaTime;
            AddForce_reset();
            rb.AddForce(transform.right * step_power, ForceMode.Impulse);
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
            animator.SetFloat("IsDashing", 1.0f);
            target_x = -1.0f;
            transform.position += transform.right * -(moveSpeed * 2.0f) * Time.deltaTime;
            AddForce_reset();
            rb.AddForce(transform.right * -step_power, ForceMode.Impulse);
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
            target_y = 0.0f;
        }
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            target_x = 0.0f;
        }


        //ここで数値を線形補間して、なめらかにする
        move_x = Mathf.Lerp(animator.GetFloat("Horizontal"), target_x, Time.deltaTime * 10f);
        move_y = Mathf.Lerp(animator.GetFloat("Vertical"), target_y, Time.deltaTime * 10f);
        //アニメーターのパラメータに値を代入
        animator.SetFloat("Horizontal", move_x);
        animator.SetFloat("Vertical", move_y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump");
            rb.AddForce(transform.up * jumppower, ForceMode.Impulse);
            jump_flag = true;
        }

      



    }

   public void anim_reset()
   {
        animator.SetBool("Action", false);
   }

    void AddForce_reset()
    {
        if (jump_flag == true)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

}