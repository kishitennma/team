using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class ChracterController : MonoBehaviour
{
    [SerializeField] GameObject player;//キャラクターオブジェクト
    public Rigidbody rb;//キャラクターオブジェクトのRigidBody
    public GameObject cam;
 
    public Camera maincam;
    public Animator animator; // キャラクターオブジェクトのAnimator
    public MotionBlur motionBlur;

   
    public bool jump_second = false;//空中でのジャンンプフラグ
    public bool jump_end = false;//ジャンプ終了フラグ
    public float jumppower;
    public bool move_flag = false;

    [SerializeField] float move_speed;//キャラクターの移動速度
    [SerializeField] float dash_speed;//ダッシュ補正速度

    private float NormalizeTime;
    private float anim_x, anim_y;//移動方向
    private float target_x, target_y;//線形保管用
    private Vector3 input_direction;//入力方向

    public int attack_power;
    public float boost = 100.0f;//ブースト残量
    private float boost_max;//ブーストの上限
    public bool boost_empty = false;

    public float target_fov;
    public float fov_changeamount = 10.0f;
    public float min_fov = 60.0f;
    public float max_fov = 90.0f;
    public TrailRenderer TrailLeft;
    public TrailRenderer TrailRight;

    //テスト/////////////////////////////////////////////
  
  
    public float speed;
    Vector3 move_dir;
    Vector3 move;//現在の移動速度
    float walk_x;
    float walk_z;
    float dash_x;
    float dash_z;
   
    public bool IsJump = false;//空中判定
    public bool IsDash = false;//空中判定






    private bool Collision_Hit = false;




    /// <summary>
    /// ジャンプのフラグ制御
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            rb.MovePosition(new Vector3(rb.transform.position.x, rb.transform.position.y + 0.03f, rb.transform.position.z));
            if (jump_end == true)
            {
                Vector3 amgles = transform.eulerAngles;
                amgles.x = 0;
                transform.eulerAngles = amgles;
                jump_end = false;
            }
        }

       
            if (other.gameObject.CompareTag("Ground"))
            {
                IsJump = false;
            }
        
    }
    private void OnCollisionStay(Collision collision)
    {
        //地面に埋まっていたら位置を上げる
        if (collision.gameObject.CompareTag("Ground"))
            rb.MovePosition(new Vector3(rb.transform.position.x, rb.transform.position.y + 0.03f, rb.transform.position.z));

        Collision_Hit = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        //物体の衝突が解消されたら移動を再開
        Collision_Hit = false;
    }


    private void Start()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

  

        //ブースト初期値反映H
        boost_max = boost;
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


    void Update()
    {

        //各移動方向へアニメーション変化
        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);

        //入力を受け取る
        float h = Input.GetAxis("Horizontal");//横
        float v = Input.GetAxis("Vertical");//縦
        move_dir = (transform.right * h + transform.forward * v).normalized;   //方向設定
        move_set();
        move = rb.linearVelocity;

        Debug.Log(move);
        //アニメージョン移動方向を初期化
        anim_x = 0; anim_y = 0; animator.SetBool("Action", false);


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


        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            target_y = 0.0f;//blend treeの数値をデフォルトの状態に戻す

        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            target_x = 0.0f;//blend treeの数値をデフォルトの状態に戻す

        }



        //ここで数値を線形補間して、なめらかにする
        anim_x = Mathf.Lerp(animator.GetFloat("Horizontal"), target_x, Time.deltaTime * 10f);
        anim_y = Mathf.Lerp(animator.GetFloat("Vertical"), target_y, Time.deltaTime * 10f);
        //アニメーターのパラメータに値を代入
        animator.SetFloat("Horizontal", anim_x);
        animator.SetFloat("Vertical", anim_y);

        if(!IsJump && !IsDash && boost < boost_max)
        {
            boost += 0.4f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && boost_empty == false
            && boost >= 20 && IsJump == false)//地上からのジャンプ
        {
            rb.linearVelocity = new Vector3(move.x, jumppower*4.0f, move.z);
            IsJump = true;
            boost -= 20.0f;
        }
        //if(rb.linearVelocity != new Vector3(0,move.y, 0) && IsJump == true)
        //{
        //    rb.linearVelocity = new Vector3(move.x, 0, move.z);
        //    rb.useGravity = false;
        //}
        //else
        //{
        //    rb.useGravity = true; ;
        //}


    }

    private void FixedUpdate()
    {




        // Debug.Log(rb.linearVelocity.y);

       


        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.linearVelocity = new Vector3(dash_x, move.y, dash_z);
            IsDash = true;
        }
        else
        {
            rb.linearVelocity = new Vector3(walk_x, move.y, walk_z);
            IsDash = false;
        }
        
      




        if (Collision_Hit)
        {
        }
        //衝突していないとき移動速度を設定
        else if (!Collision_Hit)
        {
        }

    }
    /// <summary>
    /// Addforceの力を0にする
    /// </summary>
    void AddForce_reset()
    {

        rb.linearVelocity = Vector3.zero;

    }

    void camera_Fovaway()
    {
        if (maincam != null)
        {
            maincam.fieldOfView = Mathf.Clamp(maincam.fieldOfView + fov_changeamount * Time.deltaTime * 5, min_fov, max_fov);
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

    void move_set()
    {
        walk_x = move_dir.x * speed;
        walk_z = move_dir.z * speed;
        dash_x = (move_dir.x * speed) * dash_speed;
        dash_z = (move_dir.z * speed) * dash_speed;

      

    }
}