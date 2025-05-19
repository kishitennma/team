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
    private KeyCode key;


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
        Input.GetKey(key);
        switch (key)
        {
            case KeyCode.W:

                if (Anim_start == false)
                {
                    Anim_start = true;
                    animator.SetInteger("Boost_F", 1);
                }
                if (NormalizeTime >= 0.9f && Anim_start)
                {
                    animator.SetInteger("Boost_F", 2);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    if (Anim_start == false)
                    {
                        Anim_start = true;
                        animator.SetInteger("Boost_F", 1);
                    }

                    animator.SetInteger("Boost_F", 2);

                }
                else if (Input.GetKeyUp(KeyCode.W))
                {
                    animator.SetInteger("Boost_F", 3);
                    if (NormalizeTime >= 0.9f)
                    {
                        animator.SetInteger("Boost_F", 0);
                    }
                }

                break;
            case KeyCode.A:
                if (Anim_start == false)
                {
                    Anim_start = true;
                    animator.SetInteger("Boost_L", 1);
                }
                if (NormalizeTime >= 0.9f && Anim_start)
                {
                    animator.SetInteger("Boost_L", 2);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    if (Anim_start == false)
                    {
                        Anim_start = true;
                        animator.SetInteger("Boost_L", 1);
                    }

                    animator.SetInteger("Boost_L", 2);

                }
                else if (Input.GetKeyUp(KeyCode.W))
                {
                    animator.SetInteger("Boost_L", 3);
                    if (NormalizeTime >= 0.9f)
                    {
                        animator.SetInteger("BoLst_F", 0);
                    }
                }

                break;
            case KeyCode.S:
                if (Anim_start == false)
                {
                    Anim_start = true;
                    animator.SetInteger("Boost_B", 1);
                }
                if (NormalizeTime >= 0.9f && Anim_start)
                {
                    animator.SetInteger("Boost_B", 2);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    if (Anim_start == false)
                    {
                        Anim_start = true;
                        animator.SetInteger("Boost_B", 1);
                    }

                    animator.SetInteger("Boost_B", 2);

                }
                else if (Input.GetKeyUp(KeyCode.W))
                {
                    animator.SetInteger("Boost_B", 3);
                    if (NormalizeTime >= 0.9f)
                    {
                        animator.SetInteger("BoLst_B", 0);
                    }
                }


                break;
            case KeyCode.D:
                if (Anim_start == false)
                {
                    Anim_start = true;
                    animator.SetInteger("Boost_R", 1);
                }
                if (NormalizeTime >= 0.9f && Anim_start)
                {
                    animator.SetInteger("Boost_R", 2);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    if (Anim_start == false)
                    {
                        Anim_start = true;
                        animator.SetInteger("Boost_R", 1);
                    }

                    animator.SetInteger("Boost_R", 2);

                }
                else if (Input.GetKeyUp(KeyCode.W))
                {
                    animator.SetInteger("Boost_R", 3);
                    if (NormalizeTime >= 0.9f)
                    {
                        animator.SetInteger("BoLst_R", 0);
                    }
                }


                break;
        }


        AnimatorStateInfo aninfo = animator.GetCurrentAnimatorStateInfo(0);
        NormalizeTime = aninfo.normalizedTime % 1;
        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += transform.forward * (moveSpeed * 2.0f) * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.W))
        {

            transform.position += transform.forward * moveSpeed * Time.deltaTime;


        }



        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {

            transform.position += transform.forward * -(moveSpeed * 2.0f) * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.S))
        {

            transform.position += transform.forward * -moveSpeed * Time.deltaTime;

        }


        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        {

            transform.position += transform.right * -(moveSpeed * 2.0f) * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.A))
        {

            transform.position += transform.right * -moveSpeed * Time.deltaTime;

        }


        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
        {

            transform.position += transform.right * (moveSpeed + 2.0f) * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.D))
        {

            transform.position += transform.right * moveSpeed * Time.deltaTime;

        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("ダッシュ");
            rb.AddForce(transform.up * jumppower, ForceMode.Impulse);
            //jump_flag = true;
        }


    }






}
