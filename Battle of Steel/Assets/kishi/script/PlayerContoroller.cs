using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    public Rigidbody rb;
    public GameObject Cam;
    public Animator animator; // キャラクターオブジェクトのAnimator
    private bool jump_flag = false;
    private bool step_flag = true;
    public float jumppower;
    public float step_power;

    public int attack_power = 0;
    public int hp = 100;

    private float NormalizeTime;
    private float move_x, move_y;//移動方向
    private float target_x, target_y;//線形保管用


    public float moveSpeed = 30.0f; // キャラクターの移動速度


    public bool damaged;

    /// <summary>
    /// ジャンプのフラグ制御
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
        
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))//シフトと方向キーが同時押しされたとき
        {
            if (step_flag == true)//step_flagがtrueの時ステップ移動を可能にする
            {
                transform.Translate(0, 0, step_power);
                step_flag = false;//step_flagをflaseにして動かないようにする
            }
            animator.SetFloat("IsDashing", 1.0f);//blend treeをDashに切り替える
            target_y = 1.0f;//blend tree制御
            transform.position += transform.forward * (moveSpeed * 2.0f) * Time.deltaTime;//プレイヤー移動
            AddForce_reset();//ジャンプしていた場合Addforceの力を0にする
           
        }
        else if (Input.GetKey(KeyCode.W))//方向キーだけが押されていた場合
        {
            animator.SetFloat("IsDashing", 0.0f);//blend treeをNormalにする
            target_y = 1.0f;//blend tree制御
            transform.position += transform.forward * (moveSpeed) * Time.deltaTime;//プレイヤー移動
            AddForce_reset();//ジャンプしていた場合Addforceの力を0にする
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
            target_y = 0.0f;//blend treeの数値をデフォルトの状態に戻す
           
        }
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            target_x = 0.0f;//blend treeの数値をデフォルトの状態に戻す

        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            step_flag = true;//シフトを離した場合sstep_flagをtrueにする
        }


        //ここで数値を線形補間して、なめらかにする
        move_x = Mathf.Lerp(animator.GetFloat("Horizontal"), target_x, Time.deltaTime * 10f);
        move_y = Mathf.Lerp(animator.GetFloat("Vertical"), target_y, Time.deltaTime * 10f);
        //アニメーターのパラメータに値を代入
        animator.SetFloat("Horizontal", move_x);
        animator.SetFloat("Vertical", move_y);

        if (Input.GetKeyDown(KeyCode.Space))//スペースを押されたらジャンプする
        {
            Debug.Log("Jump");
            rb.AddForce(transform.up * jumppower, ForceMode.Impulse);//上方向に力を加える
            jump_flag = true;//
        }

      



    }

 
    /// <summary>
    /// Addforceの力を0にする
    /// </summary>
    void AddForce_reset()
    {
        if (jump_flag == true)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

}