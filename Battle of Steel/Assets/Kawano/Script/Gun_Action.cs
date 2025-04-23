using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Gun_Action : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform shoot_point;//発射する
    [SerializeField] GameObject bullet;//弾丸
    
    [Header("GunProrerty")]
    [SerializeField] float shoot_force;       //発射速度
    [SerializeField] float time_bet_shooting; //発射レート(連射用)
    [SerializeField] float time_bet_shots;//発射レート(ショットガン用)
    [SerializeField] float spread; //散弾具合(ショットガン用)
    [SerializeField] float reload_time;//リロード用時間
    [SerializeField] int magazine_size;//マガジンのサイズ
    [SerializeField] int bullet_par_tap;//一発あたりの玉の数
    [SerializeField] bool allow_bullet_hold;//連射か単発のフラグ
    [SerializeField] LayerMask ignore_layer;//無視可能レイヤー

    GameObject player_cam;

    int bullets_shot, bullets_left;
    bool shooting, ready_to_shot;
    public bool reloading;
    public bool allow_invoke = true;

    void Start()
    {
        player_cam = GameObject.Find("Main Camera");
        bullets_left = magazine_size;
        ready_to_shot = true;
    }
    void Update()
    {
        InputHandler();
    }

    /// <summary>
    /// 入力制御用関数
    /// </summary>
    private void InputHandler()
    {
        //長押しできるかできないか
        if (allow_bullet_hold)
            shooting = Input.GetMouseButton(0);
        else
            shooting = Input.GetMouseButtonDown(0);

        //撃てる状態をチェック
        if(ready_to_shot && shooting && !reloading && bullets_left > 0)
        {
            bullets_shot = 0;
            Shoot();
        }

        //リロード
        if(Input.GetKeyDown(KeyCode.R) && bullets_left < magazine_size && !reloading)
        {
            Reload();
        }
    }
    private void Shoot()
    {
        ready_to_shot = false;

        Ray ray = player_cam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        Vector3 target_point;
        if (Physics.Raycast(ray, out hit, 1000f, ~ignore_layer))//レイに何が当たったかチェック
            target_point = hit.point;
        else
            target_point = ray.GetPoint(10);//何にも当たらなかったら長さを強制変更
        //銃口から見たターゲットの方向を取得
        Vector3 direction_without_spread = target_point - shoot_point.position;

        //散弾の幅
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction_with_spread = direction_without_spread + new Vector3(x, y, 0);

        //弾の生成
        GameObject current_bullet = Instantiate(bullet, shoot_point.position, Quaternion.identity);
        //弾を前方に向かせる
        current_bullet.transform.forward = direction_with_spread.normalized;
        //弾に力を加える
        current_bullet.GetComponent<Rigidbody>().AddForce(direction_with_spread.normalized * shoot_force,ForceMode.Impulse);
        bullets_left--;
        bullets_shot++;

        //弾と弾に間隔を加える
        if(allow_invoke)
        {
            Invoke("ResetShot", time_bet_shooting);
            allow_invoke = false;
        }
        //一度に出す弾
        if(bullets_shot < bullet_par_tap && bullets_left > 0)
        {
            Invoke("Shoot", time_bet_shots);
        }

    }
    /// <summary>
    /// 撃てる状態にする関数
    /// </summary>
    private void ResetShot()
    {
        ready_to_shot = true;
        allow_invoke = true;
    }
    /// <summary>
    /// リロード関数
    /// </summary>
    private void Reload()
    {
        reloading = true;
        
        Invoke(nameof(ReloadFinished), reload_time);
    }
    /// <summary>
    /// リロード完了関数
    /// </summary>
    private void ReloadFinished()
    {
        bullets_left = magazine_size;
        reloading = false;
    }
}