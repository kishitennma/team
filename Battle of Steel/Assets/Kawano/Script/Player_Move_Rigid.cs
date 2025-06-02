using Unity.VisualScripting;
using UnityEngine;

public class Player_Move_Rigid : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] float move_speed;//キャラクターの移動速度
    [SerializeField] float dash_speed;//ダッシュ補正速度
    public Animator player_animator;//プレイヤーのアニメーションコントローラー

    private Rigidbody rb;//RigidBody
    private float move_x, move_y;//Animation
    private float target_x, target_y;
    private Vector3 input_direction;//入力方向

    //RigidBodyベースのプレイヤーの移動方法

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        move_x = 0; move_y = 0;//初期化
        //カメラ回転
        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);
        mx = 0;//初期化
   
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
    //移動処理をここに記述(ここの内容はFixedUpdate以外では挙動がバグる)
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
        if (Physics.SphereCast(origin,radius, move_dir,out RaycastHit hit,  move_distance + 0.1f))
        {
            move_dir = Vector3.ProjectOnPlane(input_direction, hit.normal).normalized / 12;//距離減衰かつ、滑りを計算
        }
        //移動速度を設定
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            //通常時
            player_animator.SetFloat("IsDashing", 0.0f);
            input_direction = move_dir;
        }
        else
        {
            //ダッシュ時
            player_animator.SetFloat("IsDashing", 1.0f);//Animatorをダッシュに切り替え
            input_direction = move_dir * dash_speed;//移動ベクトルを設定
        }
        //移動方向を設定
        Vector3 move_offset = input_direction * move_speed * Time.deltaTime;
        rb.MovePosition(rb.position + move_offset);//RigidBody自体の位置を移動

    }

   

    //PlayerControllerの関数コピー
    void Screen_movement(float mx)
    {
        // X方向に一定量移動していれば横回転
        //0.0000001fは滑らかさ
        if (Mathf.Abs(mx) > 0.0000001f)
        {
            mx = mx *10;

            // 回転軸はワールド座標のY軸
            rb.transform.RotateAround(rb.transform.position, Vector3.up, mx);
        }
        

    }
}