using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Gun_Action
{
    [Header("Reference")]
    [SerializeField] Transform shootpoint;
    [SerializeField] GameObject bullet;

    [Header("GunProrerty")]
    [SerializeField] float shoot_force; //���˂̈З�
    [SerializeField] float time_bet_shooting;//���˃��[�g(�A�˗p)
    [SerializeField] float time_bet_shots;//���˃��[�g(�V���b�g�K���p)
    [SerializeField] float spread; //�U�e�(�V���b�g�K���p)
    [SerializeField] float reload_time;//�����[�h�p����
    [SerializeField] int magazine_size;//�}�K�W���̃T�C�Y
    [SerializeField] int bullet_par_tap;//�ꔭ������̋ʂ̐�
    [SerializeField] bool allow_bullet_hold;//�A�˂��P���̃t���O

    [SerializeField] LayerMask ignore_mask;//�����\���C���[

    GameObject playerCam;

    int bullets_shot, bullets_left;
}
