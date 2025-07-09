
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
    public GameObject cam;//カメラ追従用
    public Camera maincam;//メインカメラ
    public Animator animator; // キャラクターオブジェクトのAnimator
  

    public bool jump_end = false;//ジャンプ終了フラグ
    public float jumppower;

   

   
    private float anim_x, anim_y;//アニメーション切り替え用
    private float target_x, target_y;//線形保管用
  
   
    public float boost = 100.0f;//ブースト残量
    private float boost_max;//ブーストの上限
    public bool boost_empty = false;

    public float fov_changeamount = 10.0f;
    public float min_fov = 60.0f;//fov最小値
    public float max_fov = 90.0f;//fov最大値
    public TrailRenderer[] Trail;//トレイル


    //テスト/////////////////////////////////////////////


    [SerializeField] float move_speed;//キャラクターの移動速度
    [SerializeField] float dash_speed;//ダッシュ補正速度

    Vector3 move_dir;//移動方向設定用
    Vector3 move;//現在の移動速度
    Vector3 pos;//プレイヤーの座標保存用

    //プレイヤーの通常移動時の移動速度保存用
    float walk_x;
    float walk_z;
    //プレイヤーのダッシュ時の移動速度保存用
    float dash_x;
    float dash_z;
    
   
    public bool IsJump = false;//空中判定
    public bool IsDash = false;//空中判定






  



   
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
      
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJump = false;
        }

        //地面に埋まっていたら位置を上げる
        if (collision.gameObject.CompareTag("Ground"))
            rb.MovePosition(new Vector3(rb.transform.position.x, rb.transform.position.y + 0.1f, rb.transform.position.z));
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

        //接地した状態で通常移動している場合boostを回復する
        if(!IsJump && !IsDash && boost < boost_max)
        {
            boost += 0.4f;
        }

        //ダッシュ移動
        if (move.x != 0 && move.z != 0 && Input.GetKey(KeyCode.LeftShift)&&boost_empty)
        {
            boost -= 0.5f;
            Dash_Trail();
            camera_Fovaway();
            IsDash = true;
        }
        else if(move.x != 0 && move.z != 0)//通常移動
        {
            Walk_Trail();
            IsDash = false;
        }

        //地上でジャンプを押した場合
        if (Input.GetKeyDown(KeyCode.Space) && !boost_empty
            && boost >= 20 && !IsJump)//ジャンプ
        {
            pos = transform.position;//ジャンプする前のプレイヤーの座標を保存
            rb.linearVelocity = new Vector3(move.x, jumppower * 4.0f, move.z);//上方向に移動する
            IsJump = true;//空中判定
            boost -= 20.0f; //ブーストを減らす
        }

          //プレイヤーの高さが一定以上の場合
        if (transform.position.y - pos.y >= 5f)
        {
            IsJump = true;//空中判定
            if(move.x != 0 && move.z != 0 && //空中でダッシュ移動をした場合
             Input.GetKey(KeyCode.LeftShift) && !boost_empty)
            {
                rb.linearVelocity = new Vector3(move.x, 0, move.z);//上方向のベクトルを0にする
                rb.useGravity = false;//落下しないようにする
            }
            else
            {
                rb.useGravity = true;//重力を元に戻す
            }
        }

        if (boost <= 0.0f)
            boost_empty = true;
        if (boost >= boost_max)
            boost_empty = false;


    }

    private void FixedUpdate()
    {
        //ダッシュ時に移動速度を変更
        if (Input.GetKey(KeyCode.LeftShift)&&!boost_empty)
        {
            rb.linearVelocity = new Vector3(dash_x, move.y, dash_z);
          
        }
        else//通常移動時の移動速度にする
        {
            rb.linearVelocity = new Vector3(walk_x, move.y, walk_z);
            camera_Fovreturn();
          
        }
        

    }
    
    /// <summary>
    /// カメラのFOVを90に上げる
    /// </summary>
    void camera_Fovaway()
    {
        if (maincam != null)
        {
            maincam.fieldOfView = Mathf.Clamp(maincam.fieldOfView + fov_changeamount * Time.deltaTime * 5, min_fov, max_fov);
            //maincam.fieldOfView = max_fov;
        }
    }
    /// <summary>
    /// カメラのFOVを60にする
    /// </summary>
    void camera_Fovreturn()
    {
        if (maincam != null)
        {
            maincam.fieldOfView = Mathf.Clamp(maincam.fieldOfView - fov_changeamount * Time.deltaTime, min_fov, max_fov);

        }
    }
    /// <summary>
    /// 移動速度設定
    /// </summary>
    void move_set()
    {
        walk_x = move_dir.x * move_speed;
        walk_z = move_dir.z * move_speed;
        dash_x = (move_dir.x * move_speed) * dash_speed;
        dash_z = (move_dir.z * move_speed) * dash_speed;
    }
    /// <summary>
    /// ダッシュ時のTrailの設定を変える
    /// </summary>
    void Dash_Trail()
    {
        Trail[0].time = 0.5f;
        Trail[1].time = 0.5f;
        Trail[0].material.color = Color.red;
        Trail[1].material.color = Color.red;
    }
    /// <summary>
    /// 通常時のTrailを設定する
    /// </summary>
    void Walk_Trail()
    {
        Trail[0].time = 0.1f;
        Trail[1].time = 0.1f;
        Trail[0].material.color = Color.white;
        Trail[1].material.color = Color.white;
    }
}