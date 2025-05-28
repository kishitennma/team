using UnityEngine;
using UnityEngine.UI;

public class Player_Weapon_Manager : MonoBehaviour
{
    [Header("武器切り替えコンポーネント")]
    [Header("武器情報")]
    [SerializeField] Transform set_weapon_point;//武器所持用ポイント
    [SerializeField] GameObject main_weapon;//所持しているメイン武器
    [SerializeField] GameObject sub_weapon;//所持しているサブ武器
    [SerializeField] Text ammo_texts;//弾丸のUI情報
    public Animator player_animator;//プレイヤーのアニメーションコントローラ
    public WeaponSystem weapon_system;//weapon_system ammo_text変更用
    [Header("切り替えボタン")]
    [SerializeField] KeyCode set_key = KeyCode.Q;//切り替えボタン

    private bool hold_secondry_weapon;//サブ武器の所持状態
    private void Start()
    {
        if (main_weapon != null)
        {
            Set_Weapon_hand(main_weapon, sub_weapon);//メイン武器を手に持たせる
        }
        else
        {
            Debug.LogError("メイン武器がよみこまれませんでした");
        }
    }
    private void Update()
    {
        if (sub_weapon!= null)
        {
            //サブ武器を所持している、手に持っているのがメイン武器の場合set_keyで変更
            if(!hold_secondry_weapon && Input.GetKeyDown(set_key))
            {
                Set_Weapon_hand(sub_weapon, main_weapon);
                hold_secondry_weapon = true;
            }
            else if(hold_secondry_weapon && Input.GetKeyDown(set_key))
            {
                Set_Weapon_hand(main_weapon, sub_weapon);
                hold_secondry_weapon = false;
            }
        }
        else
        {
            Debug.LogError("サブ武器がよみこまれませんでした");
        }


    }
    private void Set_Weapon_hand(GameObject change_weapon,GameObject set_weapon)
    {
        player_animator.SetBool("Change_Weapon",true);
        weapon_system = change_weapon.GetComponent<WeaponSystem>();//WeaponSystemコンポーネント取得
        weapon_system.ammo_text = ammo_texts;
        change_weapon.SetActive(true);//変更後の武器を出現
        change_weapon.transform.position = set_weapon_point.transform.position;//位置を武器を持たせる位置に合わせる
        set_weapon.SetActive(false);//変更前の武器を消す
        Debug.Log("武器を交換しました");
    }
    //武器変更用アニメーションストップ
    public void Set_End_Change_Anim()
    {
        player_animator.SetBool("Change_Weapon", false);
    }
}