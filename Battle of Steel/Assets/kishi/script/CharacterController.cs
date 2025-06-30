using UnityEditor;
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

    public bool jump_flag = true;//地上でのジャンプフラグ
    public bool jump_second = false;//空中でのジャンンプフラグ
    public bool jump_end = false;//ジャンプ終了フラグ
    public float jumppower;
    public bool move = false;

    [SerializeField] float move_speed;//キャラクターの移動速度
    [SerializeField] float dash_speed;//ダッシュ補正速度

    private float NormalizeTime;
    private float move_x, move_y;//移動方向
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
    Transform tr;
    Vector3 characterfoward;
    Vector3 movevecter;
    float vertical;
    float horizontal;
    public float speed;
    public float rotatespeed;




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

        tr = this.transform;

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

        //移動方向を初期化
        move_x = 0; move_y = 0; animator.SetBool("Action", false);


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
        move_x = Mathf.Lerp(animator.GetFloat("Horizontal"), target_x, Time.deltaTime * 10f);
        move_y = Mathf.Lerp(animator.GetFloat("Vertical"), target_y, Time.deltaTime * 10f);
        //アニメーターのパラメータに値を代入
        animator.SetFloat("Horizontal", move_x);
        animator.SetFloat("Vertical", move_y);

      

    }

    private void FixedUpdate()
    {

        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
         if(horizontal > 0)
         {
            transform.Rotate(0, rotatespeed * Time.fixedDeltaTime, 0);
         }
         if(horizontal < 0)
         {
            transform.Rotate(0, (-1) * rotatespeed * Time.fixedDeltaTime, 0);
         }

        movevecter = tr.forward * vertical * Time.fixedDeltaTime * speed;
        rb.linearVelocity = new Vector3(movevecter.x, rb.linearVelocity.y, movevecter.z);














        //各移動方向へアニメーション変化
        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);
        // Debug.Log(rb.linearVelocity.y);
        //入力を受け取る
        float h = Input.GetAxis("Horizontal");//横
        float v = Input.GetAxis("Vertical");//縦
        Vector3 move_dir = (transform.right * h + transform.forward * v).normalized;   //方向設定

        if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
        {
            move_speed = 20f;//キーを離したら速度を戻す
            Player_Status.Player_Attack_Damage = Player_Status.Player_Put_Attack_Damage;//攻撃力を元の値に戻す
        }
        if (Collision_Hit)
        {
            move_dir /= 10;//何かに当たったら移動距離を減らす
            move_speed = 20f;
            Player_Status.Player_Attack_Damage = Player_Status.Player_Put_Attack_Damage;//攻撃力を元の値に戻す
        }
        else
        {
            move_speed += 0.1f;//キーが押されている間は数値を加算
            if (move_speed >= 80.0f)
            {
                move_speed = 80.0f;
            }
            else if (move_speed >= 70.0f)
            {
                Player_Status.Player_Attack_Damage++;
                if (Player_Status.Player_Attack_Damage > 80)
                {
                    Player_Status.Player_Attack_Damage = 80;
                }
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            //通常時
            animator.SetFloat("IsDashing", 0.0f);
            input_direction = move_dir;
            TrailLeft.time = 0.1f;
            TrailRight.time = 0.1f;
            TrailLeft.material.color = Color.white;
            TrailRight.material.color = Color.white;


        }
        else
        {
            if (!boost_empty)
            {
                //ダッシュ時
                animator.SetFloat("IsDashing", 1.0f);//Animatorをダッシュに切り替え
                input_direction = move_dir * dash_speed;//移動ベクトルを設定
                TrailLeft.time = 0.5f;
                TrailRight.time = 0.5f;
                TrailLeft.material.color = Color.cyan;
                TrailRight.material.color = Color.cyan;

            }
            else
            {

                //通常時
                animator.SetFloat("IsDashing", 0.0f);
                input_direction = move_dir;
                TrailLeft.time = 0.1f;
                TrailRight.time = 0.1f;
                TrailLeft.material.color = Color.white;
                TrailRight.material.color = Color.white;
            }

        }
        //移動方向を設定
        Vector3 move_offset = input_direction * move_speed * Time.deltaTime;
        rb.MovePosition(rb.position + move_offset);//RigidBody自体の位置を移動


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
}