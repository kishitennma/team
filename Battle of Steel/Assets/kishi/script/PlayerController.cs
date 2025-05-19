using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;

    public GameObject Cam;
    public Animator animator; // キャラクターオブジェクトのAnimator
   

    public float moveSpeed = 30.0f; // キャラクターの移動速度

    public bool damaged;

    private int squat_cntb=0;
    private int squat_total=10;

    private float squatStartTime;
    private float squatEndTime;


    private void Start()
    {
        animator = GetComponent<Animator>();
        damaged = false;
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

    void Update()
    {

        float mx = Input.GetAxis("Mouse X");
        Screen_movement(mx);
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("ダッシュ");
            //プレイヤーの正面に向かって移動する
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetBool("BoostF", true);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            //プレイヤーの正面に向かって移動する
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetBool("BoostF", true);
        }
        else
            animator.SetBool("BoostF", false);
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("ダッシュ");
            //プレイヤーの正面に向かって移動する
            transform.position += transform.forward * -moveSpeed * Time.deltaTime;
            animator.SetBool("BoostB", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * -moveSpeed * Time.deltaTime;
            animator.SetBool("BoostB", true);
        }
        else
            animator.SetBool("BoostB", false);

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("ダッシュ");
            //プレイヤーの正面に向かって移動する
            transform.position += transform.right * -moveSpeed * Time.deltaTime;
            animator.SetBool("BoostR", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += transform.right * -moveSpeed * Time.deltaTime;
            animator.SetBool("BoostR", true);
        }
        else
            animator.SetBool("BoostR", false);

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("ダッシュ");
            //プレイヤーの正面に向かって移動する
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            animator.SetBool("BoostL", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            animator.SetBool("BoostL", true);
        }
        else
            animator.SetBool("BoostL", false);

    }
    }

