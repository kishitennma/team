using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    public Rigidbody rb;
    public GameObject Cam;
    public Animator animator; // キャラクターオブジェクトのAnimator
    private bool jump_flag = false;
    public float jumppower;


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
        rb = GetComponent < Rigidbody >();
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
            animator.SetBool("Run", true);

            float mx = Input.GetAxis("Mouse X");
            Screen_movement(mx);
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("ダッシュ");
                //プレイヤーの正面に向かって移動する
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                animator.SetBool("Run", true);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                //プレイヤーの正面に向かって移動する
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                animator.SetBool("Run", true);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += transform.forward * -moveSpeed * Time.deltaTime;
                animator.SetBool("Run", true);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += transform.right * -moveSpeed * Time.deltaTime;
                animator.SetBool("Run", true);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * moveSpeed * Time.deltaTime;
                //animator.SetBool("Run", true);
            }
            else
                animator.SetBool("Run", false);
     
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("ダッシュ");
                rb.AddForce(transform.up * jumppower, ForceMode.Acceleration);
                //jump_flag = true;
            }
        
       
    }
    
}

