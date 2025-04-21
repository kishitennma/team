using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Gun_Action
{
    [Header("Reference")]
    [SerializeField] Transform shootpoint;
    [SerializeField] GameObject bullet;

    [Header("GunProrerty")]
    [SerializeField] float shoot_force; //発射の威力
    [SerializeField] float time_bet_shooting;//発射レート(連射用)
    [SerializeField] float time_bet_shots;//発射レート(ショットガン用)
    [SerializeField] float spread; //散弾具合(ショットガン用)
    [SerializeField] float reload_time;//リロード用時間
    [SerializeField] int magazine_size;//マガジンのサイズ
    [SerializeField] int bullet_par_tap;//一発あたりの玉の数
    [SerializeField] bool allow_bullet_hold;//連射か単発のフラグ

    [SerializeField] LayerMask ignore_mask;//無視可能レイヤー

    GameObject playerCam;

    int bullets_shot, bullets_left;
}
