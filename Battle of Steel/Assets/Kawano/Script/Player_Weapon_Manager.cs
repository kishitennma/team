using UnityEngine;
using UnityEngine.UI;

public class Player_Weapon_Manager : MonoBehaviour
{
    [Header("武器切り替えコンポーネント")]
    [Header("武器情報")]
    [SerializeField] Transform set_weapon_point;//武器所持用ポイント
    [SerializeField] Transform not_equip_weapon_point;//武器を保持するポイント
    [SerializeField] Transform hand_point;//武器を持つ手の位置
    public GameObject main_weapon;//所持しているメイン武器
    public GameObject sub_weapon;//所持しているサブ武器
    [SerializeField] Text ammo_texts;//弾丸のUI情報
    public Animator player_animator;//プレイヤーのアニメーションコントローラ
    [Header("ボタン入力")]
    [SerializeField] KeyCode set_key = KeyCode.Q;//切り替えボタン
    public Image Change_Image;
    public bool hold_secondry_weapon;//サブ武器の所持状態
    public static bool anim_end_flag=false;//アニメション終了フラグ
    private WeaponSystem weapon_system;//weapon_system ammo_text変更用
    private WeaponSystem not_equiped_weap_s;//使用しない武器

    private void Start()
    {
        if (main_weapon != null)
        {
            set_weapon_point.position = hand_point.position;
            main_weapon.transform.position = set_weapon_point.position;
            sub_weapon.transform.position = set_weapon_point.position;
            Set_Weapon_hand(main_weapon, sub_weapon);//メイン武器を手に持たせる
        }
        else
        {
            Debug.LogError("メイン武器がよみこまれませんでした");
        }
    }
    private void FixedUpdate()
    {
        Player_Status.Player_Attack_Damage = weapon_system.setting_attack_dmg;//攻撃力を入力
        Player_Status.Player_Put_Attack_Damage = weapon_system.setting_attack_dmg;//保持用の攻撃力を設定
        set_weapon_point.position = hand_point.position;
        if(hold_secondry_weapon)
        {
            main_weapon.transform.position = not_equip_weapon_point.position;
            sub_weapon.transform.position = set_weapon_point.position;
        }
        else
        {
            main_weapon.transform.position = set_weapon_point.position;
            sub_weapon.transform.position = not_equip_weapon_point.position;

        }

        Player_Status.Player_Put_Attack_Damage = weapon_system.setting_attack_dmg;//攻撃力を入力
        if (sub_weapon!= null)
        {
            if(anim_end_flag == false)
            {
                if (!hold_secondry_weapon && !WeaponSystem.Not_Changed_Weap &&Input.GetKey(set_key))
                {
                    Set_Weapon_hand(sub_weapon, main_weapon);
                    hold_secondry_weapon = true;
                }
                else if (hold_secondry_weapon && !WeaponSystem.Not_Changed_Weap && Input.GetKey(set_key))
                {
                    Set_Weapon_hand(main_weapon, sub_weapon);
                    hold_secondry_weapon = false;
                }
            }
        }
        else
        {
            Debug.LogError("サブ武器がよみこまれませんでした");
        }

    }

    private void Set_Weapon_hand(GameObject change_weapon,GameObject set_weapon)
    {
        anim_end_flag = true;
        player_animator.SetBool("Change_Weapon",true);
        weapon_system = change_weapon.GetComponent<WeaponSystem>();//WeaponSystemコンポーネント取得
        not_equiped_weap_s = set_weapon.GetComponent<WeaponSystem>();//WeaponSystemコンポーネント取得
        weapon_system.isEquiped = true;//変更した武器を持たせる
        not_equiped_weap_s.isEquiped = false;//使用しない武器から弾丸を発射しないようにする
        weapon_system.ammo_text = ammo_texts;
        Player_Status.Player_Attack_Damage = weapon_system.setting_attack_dmg;//攻撃力を入力
        Player_Status.Player_Put_Attack_Damage = weapon_system.setting_attack_dmg;//保持用の攻撃力を設定
        change_weapon.transform.position = set_weapon_point.transform.position;//位置を武器を持たせる位置に合わせる
        change_weapon.transform.rotation = set_weapon_point.transform.rotation;//位置を武器を持たせる位置に合わせる
        change_weapon.transform.localScale = set_weapon_point.transform.localScale;//位置を武器を持たせる位置に合わせる
        set_weapon.transform.position = not_equip_weapon_point.transform.position;//使わない武器の使用位置を背中に設定
        set_weapon.transform.rotation = not_equip_weapon_point.rotation;
        set_weapon.transform.localScale = not_equip_weapon_point.localScale;
        Change_Image.color = Color.red;
        Invoke(nameof(Set_End_Change_Anim), 0.2f);

    }
    //武器変更用アニメーションストップ
    public void Set_End_Change_Anim()
    {
        //変更後の武器出現
        Change_Image.color = Color.white;
        player_animator.SetBool("Change_Weapon", false);
        anim_end_flag = false;
    }
}