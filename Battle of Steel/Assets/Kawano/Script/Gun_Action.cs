using UnityEngine;
using UnityEngine.UI;

public class Gun_Action : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject bullet;//�e��

    [Header("GunProrerty")]
    [SerializeField] Transform nozzle_transform;//�m�Y���̂����擾
    [SerializeField] float shoot_force;       //���ˑ��x
    [SerializeField] float time_bet_shooting; //���˃��[�g(�A�˗p)
    [SerializeField] float time_bet_shots;//���˃��[�g(�V���b�g�K���p)
    [SerializeField] float spread; //�U�e�(�V���b�g�K���p)
    [SerializeField] float reload_time;//�����[�h�p����
    [SerializeField] int magazine_size;//�}�K�W���̃T�C�Y
    [SerializeField] int bullet_par_tap;//�ꔭ������̒e�̐�
    [SerializeField] bool allow_bullet_hold;//�A�����P���̃t���O
    [SerializeField] LayerMask ignore_layer;//�����\���C���[
    [Header("�e�ۏ��e�L�X�g")]
    [SerializeField] private Text ammoText;
    int bullets_shot, bullets_left;
    bool shooting, ready_to_shot;
    public bool reloading;
    public bool allow_invoke = true;

    void Start()
    {
        bullets_left = magazine_size;
        ready_to_shot = true;
    }
    void Update()
    {
        InputHandler();
        UpdateAmmoDisplay();
    }
    //�e�ۏ��\��
    private void UpdateAmmoDisplay()
    {
        if (ammoText != null)
        {
            ammoText.text = $"�e��: {bullets_left} / {magazine_size}";
        }
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
        if (ready_to_shot && shooting && !reloading && bullets_left > 0)
        {
            bullets_shot = 0;
            Shoot();
        }

        //�����[�h
        if (bullets_left < 0 && !reloading)
        {
            Reload();
        }
    }
    private void Shoot()
    {
        ready_to_shot = false;

        //�e�����猩���^�[�Q�b�g�̕������擾
        Vector3 direction_without_spread = nozzle_transform.forward;

        //�U�e�̕�
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction_with_spread = direction_without_spread + nozzle_transform.TransformDirection(new Vector3(x, y, 0));

        //�e�̐���
        GameObject current_bullet = Instantiate(bullet, nozzle_transform.position, nozzle_transform.rotation);
        //�e��O���Ɍ�������
        current_bullet.transform.forward = direction_with_spread.normalized;
        //�e�ɗ͂�������
        current_bullet.GetComponent<Rigidbody>().AddForce(direction_with_spread.normalized * shoot_force, ForceMode.Impulse);
        bullets_left--;
        bullets_shot++;

        //�e�ƒe�ɊԊu��������
        if (allow_invoke)
        {
            Invoke(nameof(ResetShot), time_bet_shooting);
            allow_invoke = false;
        }
        //��x�ɏo���e
        if (bullets_shot < bullet_par_tap && bullets_left > 0)
        {
            Invoke(nameof(Shoot), time_bet_shots);
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