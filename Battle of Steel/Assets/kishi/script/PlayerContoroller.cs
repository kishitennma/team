using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    public Rigidbody rb;
    public GameObject Cam;
    public Animator animator; // キャラクターオブジェクトのAnimator
    public bool jump_flag = true;
    public bool jump_second = false;
    public float jumppower;

    [SerializeField] float move_speed;//キャラクターの移動速度
    [SerializeField] float dash_speed;//ダッシュ補正速度

    private float NormalizeTime;
    private float move_x, move_y;//移動方向
    private float target_x, target_y;//線形保管用
    private Vector3 input_direction;//入力方向

    public int attack_power;
    public float boost = 100.0f;
    public float boost_max = 100.0f;


  

    /// <summary>
    /// ジャンプのフラグ制御
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (jump_flag == false)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                jump_flag = true;
                jump_second = false;
            }
        }
    }


    private void Start()
    {
        animator = GetComponent<Animator>();
      
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// プレイヤーの回転(X軸）
    /// </summary>
    /// <param name="mx"></param>
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
       
        //移動方向を初期化
        move_x = 0; move_y = 0; animator.SetBool("Action", false);
        //各移動方向へアニメーション変化
        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);

        if (Input.GetKey(KeyCode.W))//方向キーだけが押されていた場合
        {
            target_y = 1.0f;//blend tree制御 
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
       

        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            target_y = 0.0f;//blend treeの数値をデフォルトの状態に戻す
           
        }
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            target_x = 0.0f;//blend treeの数値をデフォルトの状態に戻す

        }
      


        //ここで数値を線形補間して、なめらかにする
        move_x = Mathf.Lerp(animator.GetFloat("Horizontal"), target_x, Time.deltaTime * 10f);
        move_y = Mathf.Lerp(animator.GetFloat("Vertical"), target_y, Time.deltaTime * 10f);
        //アニメーターのパラメータに値を代入
        animator.SetFloat("Horizontal", move_x);
        animator.SetFloat("Vertical", move_y);

        if(jump_flag && boost < boost_max && animator.GetFloat("IsDashing") != 1.0f)//地面に立っているときブースト回復
        {
            
            boost+=0.4f;
        }

        if(Input.GetKeyDown(KeyCode.Space) && jump_flag && boost >= 20.0f)//地上からのジャンプ
        {
            rb.linearVelocity = new Vector3(0, (jumppower*3.0f), 0);
            boost -= 20.0f;
            jump_flag = false;
        }
        if(Input.GetKeyUp(KeyCode.Space)&& !jump_flag)//空中でスペースキーを離した判定
        {
            jump_second = true;
        }
        else if (Input.GetKey(KeyCode.Space) && boost > 0 
        && jump_second && animator.GetFloat("IsDashing") != 1.0f)//空中ジャンプ(ホバー？）
        {
            rb.linearVelocity = new Vector3(0, jumppower, 0);
            jump_flag = false;
            boost -= 0.3f;
        }
        if(animator.GetFloat("IsDashing") == 1.0f)
        {
            boost -= 0.1f;
        }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
          Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            if (jump_flag == false )
            {
                if( boost > 0.0f)
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
        //入力を受け取る
        float h = Input.GetAxis("Horizontal");//横
        float v = Input.GetAxis("Vertical");//縦

        Vector3 move_dir = (transform.right * h + transform.forward * v).normalized;   //方向設定
        Vector3 origin = transform.position + Vector3.up * 1.5f;                       //中心点の少し上を取る
        float move_distance = move_speed * Time.fixedDeltaTime;                        //移動距離設定
        float radius = 0.8f;                                                           //SpeheCast用に半径設定

        //移動方向にRigidBody持ちのオブジェクトがあったら
        if (Physics.SphereCast(origin, radius, move_dir, out RaycastHit hit, move_distance + 0.1f))
        {
            move_dir = Vector3.ProjectOnPlane(input_direction, hit.normal).normalized / 6;//距離減衰かつ、滑りを計算
        }
        //移動速度を設定
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            //通常時
            animator.SetFloat("IsDashing", 0.0f);
            input_direction = move_dir;
           
        }
        else
        {
            if (boost > 0.0f)
            {
                //ダッシュ時
                animator.SetFloat("IsDashing", 1.0f);//Animatorをダッシュに切り替え
                input_direction = move_dir * dash_speed;//移動ベクトルを設定

            }
            else
            {

                //通常時
                animator.SetFloat("IsDashing", 0.0f);
                input_direction = move_dir;
            }

        }


        //移動方向を設定
        Vector3 move_offset = input_direction * move_speed * Time.deltaTime;
        rb.MovePosition(rb.position + move_offset);//RigidBody自体の位置を移動
       
        

    }


    /// <summary>
    /// Addforceの力を0にする
    /// </summary>
    void AddForce_reset()
    {
       
            rb.linearVelocity = Vector3.zero;
        
    }

}