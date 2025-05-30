using Unity.VisualScripting;
using UnityEngine;

public class Player_Move_Rigid : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] float move_speed = 1f;//キャラクターの移動速度
    [SerializeField] float dash_speed = 1.3f;//ダッシュ補正速度

    public Animator player_animator;//プレイヤーのアニメーションコントローラー

    private Rigidbody rb;//RigidBody
    private float move_x, move_y;//線形補完用
    private float target_x, target_y;//線形保管用
    private bool wall_flag;

    private Vector3 input_direction;//入力方向

    //UnityのRigidBodyを使った場合のプレイヤーの移動方法 :: 実験段階


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;
    }

    void Update()
    {
        move_x = 0;move_y = 0;
        float mx = Input.GetAxis("Mouse X");

        Screen_movement(mx);

        //押されているボタンによって、アニメーションを制御
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
            target_y = 0.0f;//blend treeの数値をデフォルトの状態に戻す

        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            target_x = 0.0f;//blend treeの数値をデフォルトの状態に戻す
        }

        //ここで数値を線形補間して、なめらかにする
        move_x = Mathf.Lerp(player_animator.GetFloat("Horizontal"), target_x, Time.deltaTime * 10f);
        move_y = Mathf.Lerp(player_animator.GetFloat("Vertical"), target_y, Time.deltaTime * 10f);
        //アニメーターのパラメータに値を代入
        player_animator.SetFloat("Horizontal", move_x);
        player_animator.SetFloat("Vertical", move_y);

    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("何かに当たった");
        if (collision.gameObject.CompareTag("Wall"))
        {
            
            //最終手段
            //wall_flag = true;
            //input_direction = Vector3.zero;
        }

    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            wall_flag = false;

    }
    private void FixedUpdate()
    {
        //入力を受け取る
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move_dir = (transform.right * h + transform.forward * v).normalized;
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        float move_distance = move_speed * Time.fixedDeltaTime;
        float radius = 0.4f;
        //移動方向にRigidBody持ちのオブジェクトがなかったら
        if (!Physics.SphereCast(origin,radius, move_dir,out RaycastHit hit,  move_distance + 0.1f) && wall_flag != true)
        {
            move_dir = Vector3.ProjectOnPlane(input_direction, hit.normal).normalized;
            //移動速度を設定
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                //通常時
                player_animator.SetFloat("IsDashing", 0.0f);
                input_direction = transform.right * h + transform.forward * v;
            }
            else
            {
                //ダッシュ時
                player_animator.SetFloat("IsDashing", 1.0f);//Animatorをダッシュに切り替え
                input_direction = (transform.right * h + transform.forward * v) * dash_speed;//移動ベクトルを設定
            }
            //移動方向を設定
            Vector3 move_offset = input_direction * move_speed * Time.deltaTime;
            rb.MovePosition(rb.position + move_offset);//RigidBody自体の位置を移動

        }
    }

    //以下。PlayerControllerの関数コピー
    void Screen_movement(float mx)
    {
        // X方向に一定量移動していれば横回転
        //0.0000001fは滑らかさ
        if (Mathf.Abs(mx) > 0.0000001f)
        {
            mx = mx * 5;

            // 回転軸はワールド座標のY軸
            rb.transform.RotateAround(rb.transform.position, Vector3.up, mx);
        }
    }
}