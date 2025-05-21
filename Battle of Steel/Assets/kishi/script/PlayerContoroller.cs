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
    public bool Anim_start = false;
    public bool Anim_end = false;


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
        if (Input.GetKey(KeyCode.W))
        {
            target_y = 1.0f;
            transform.position += transform.forward * (moveSpeed * 2.0f) * Time.deltaTime;
            
            animator.SetBool("Action", true);
        }
       
        else if (Input.GetKey(KeyCode.S))
        {
            target_y = -1.0f;
            transform.position += transform.forward * -(moveSpeed * 2.0f) * Time.deltaTime;
           
            animator.SetBool("Action", true);
        }
       
        if (Input.GetKey(KeyCode.D))
        {
            target_x = 1.0f;
            transform.position += transform.right * moveSpeed * Time.deltaTime;
           
            animator.SetBool("Action", true);
        }
      
        else if (Input.GetKey(KeyCode.A))
        {
            target_x = -1.0f;
            transform.position += transform.right * -(moveSpeed * 2.0f) * Time.deltaTime;
          
            animator.SetBool("Action", true);
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

        //AnimatorStateInfo aninfo = animator.GetCurrentAnimatorStateInfo(0);
        //NormalizeTime = aninfo.normalizedTime % 1;

        //if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        //{
        //    transform.position += transform.forward * (moveSpeed * 2.0f) * Time.deltaTime;
        //    if (Anim_start == false)
        //    {
        //        Anim_start = true;
        //        animator.SetInteger("Boost_F", 1);
        //    }
        //    if (NormalizeTime >= 0.9f && Anim_start)
        //    {
        //        Debug.Log("再生終了");
        //        animator.SetInteger("Boost_F", 2);
        //    }

        //}
        //else if (Input.GetKey(KeyCode.W))
        //{
        //    transform.position += transform.forward * moveSpeed * Time.deltaTime;
        //    if (Anim_start == false)
        //    {
        //        Anim_start = true;
        //        animator.SetInteger("Boost_F", 1);
        //        Debug.Log("押された＿W");
        //    }
        //    if (NormalizeTime >= 0.9f && Anim_start)
        //    {
        //        animator.SetInteger("Boost_F", 2);
        //    }

        //}
        //else if (Input.GetKeyUp(KeyCode.W))
        //{
        //    animator.SetInteger("Boost_F", 3);
        //    if (NormalizeTime >= 0.9f)
        //    {

        //    }
        //}
        //if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        //{

        //    transform.position += transform.forward * -(moveSpeed * 2.0f) * Time.deltaTime;
        //    if (Anim_start == false)
        //    {
        //        Anim_start = true;
        //        animator.SetInteger("Boost_B", 1);
        //    }
        //    if (NormalizeTime >= 0.9f && Anim_start)
        //    {
        //        animator.SetInteger("Boost_B", 2);
        //    }

        //}
        //else if (Input.GetKey(KeyCode.S))
        //{

        //    transform.position += transform.forward * -moveSpeed * Time.deltaTime;
        //    if (Anim_start == false)
        //    {
        //        Anim_start = true;
        //        animator.SetInteger("Boost_B", 1);
        //        Debug.Log("押された＿S");
        //    }
        //    if (NormalizeTime >= 0.9f && Anim_start)
        //    {
        //        animator.SetInteger("Boost_B", 2);
        //    }
        //}
        //else if (Input.GetKeyUp(KeyCode.S))
        //{
        //    animator.SetInteger("Boost_B", 3);
        //    if (NormalizeTime >= 0.9f)
        //    {
        //        animator.SetInteger("Boost_B", 0);
        //    }
        //}
        //if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        //{

        //    transform.position += transform.right * -(moveSpeed * 2.0f) * Time.deltaTime;
        //    if (Anim_start == false)
        //    {
        //        Anim_start = true;
        //        animator.SetInteger("Boost_L", 1);
        //    }
        //    if (NormalizeTime >= 0.9f && Anim_start)
        //    {
        //        animator.SetInteger("Boost_L", 2);
        //    }

        //}
        //else if (Input.GetKey(KeyCode.A))
        //{

        //    transform.position += transform.right * -moveSpeed * Time.deltaTime;
        //    if (Anim_start == false)
        //    {
        //        Anim_start = true;
        //        animator.SetInteger("Boost_L", 1);
        //        Debug.Log("押された＿A");
        //    }
        //    if (NormalizeTime >= 0.9f && Anim_start)
        //    {
        //        animator.SetInteger("Boost_L", 2);
        //    }

        //}
        //else if (Input.GetKeyUp(KeyCode.S))
        //{
        //    animator.SetInteger("Boost_L", 3);
        //    if (NormalizeTime >= 0.9f)
        //    {
        //        animator.SetInteger("Boost_L", 0);
        //    }
        //}
        //if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
        //{

        //    transform.position += transform.right * (moveSpeed + 2.0f) * Time.deltaTime;
        //    if (Anim_start == false)
        //    {
        //        Anim_start = true;
        //        animator.SetInteger("Boost_R", 1);
        //    }
        //    if (NormalizeTime >= 0.9f && Anim_start)
        //    {
        //        animator.SetInteger("Boost_R", 2);
        //    }

        //}
        //else if (Input.GetKey(KeyCode.D))
        //{

        //    transform.position += transform.right * moveSpeed * Time.deltaTime;
        //    if (Anim_start == false)
        //    {
        //        Anim_start = true;
        //        animator.SetInteger("Boost_R", 1);
        //        Debug.Log("押された＿D");
        //    }
        //    if (NormalizeTime >= 0.9f && Anim_start)
        //    {
        //        animator.SetInteger("Boost_R", 2);
        //    }
        //}
        //else if (Input.GetKeyUp(KeyCode.S))
        //{
        //    animator.SetInteger("Boost_R", 3);
        //    if (NormalizeTime >= 0.9f)
        //    {
        //        animator.SetInteger("Boost_R", 0);
        //    }
        //}


        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("ダッシュ");
        //    rb.AddForce(transform.up * jumppower, ForceMode.Impulse);
        //    //jump_flag = true;
        //}


    }

public void anim_reset()
       {
            target_y = 0;
            target_x = 0;
       }

}