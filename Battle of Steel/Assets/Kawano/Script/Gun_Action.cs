using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Gun_Action : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform shoot_point;//���˂���
    [SerializeField] GameObject bullet;//�e��
    
    [Header("GunProrerty")]
    [SerializeField] float shoot_force;       //���ˑ��x
    [SerializeField] float time_bet_shooting; //���˃��[�g(�A�˗p)
    [SerializeField] float time_bet_shots;//���˃��[�g(�V���b�g�K���p)
    [SerializeField] float spread; //�U�e�(�V���b�g�K���p)
    [SerializeField] float reload_time;//�����[�h�p����
    [SerializeField] int magazine_size;//�}�K�W���̃T�C�Y
    [SerializeField] int bullet_par_tap;//�ꔭ������̋ʂ̐�
    [SerializeField] bool allow_bullet_hold;//�A�˂��P���̃t���O
    [SerializeField] LayerMask ignore_layer;//�����\���C���[

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
    /// ���͐���p�֐�
    /// </summary>
    private void InputHandler()
    {
        //�������ł��邩�ł��Ȃ���
        if (allow_bullet_hold)
            shooting = Input.GetMouseButton(0);
        else
            shooting = Input.GetMouseButtonDown(0);

        //���Ă��Ԃ��`�F�b�N
        if(ready_to_shot && shooting && !reloading && bullets_left > 0)
        {
            bullets_shot = 0;
            Shoot();
        }

        //�����[�h
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
        if (Physics.Raycast(ray, out hit, 1000f, ~ignore_layer))//���C�ɉ��������������`�F�b�N
            target_point = hit.point;
        else
            target_point = ray.GetPoint(10);//���ɂ�������Ȃ������璷���������ύX
        //�e�����猩���^�[�Q�b�g�̕������擾
        Vector3 direction_without_spread = target_point - shoot_point.position;

        //�U�e�̕�
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction_with_spread = direction_without_spread + new Vector3(x, y, 0);

        //�e�̐���
        GameObject current_bullet = Instantiate(bullet, shoot_point.position, Quaternion.identity);
        //�e��O���Ɍ�������
        current_bullet.transform.forward = direction_with_spread.normalized;
        //�e�ɗ͂�������
        current_bullet.GetComponent<Rigidbody>().AddForce(direction_with_spread.normalized * shoot_force,ForceMode.Impulse);
        bullets_left--;
        bullets_shot++;

        //�e�ƒe�ɊԊu��������
        if(allow_invoke)
        {
            Invoke("ResetShot", time_bet_shooting);
            allow_invoke = false;
        }
        //��x�ɏo���e
        if(bullets_shot < bullet_par_tap && bullets_left > 0)
        {
            Invoke("Shoot", time_bet_shots);
        }

    }
    /// <summary>
    /// ���Ă��Ԃɂ���֐�
    /// </summary>
    private void ResetShot()
    {
        ready_to_shot = true;
        allow_invoke = true;
    }
    /// <summary>
    /// �����[�h�֐�
    /// </summary>
    private void Reload()
    {
        reloading = true;
        
        Invoke(nameof(ReloadFinished), reload_time);
    }
    /// <summary>
    /// �����[�h�����֐�
    /// </summary>
    private void ReloadFinished()
    {
        bullets_left = magazine_size;
        reloading = false;
    }
}