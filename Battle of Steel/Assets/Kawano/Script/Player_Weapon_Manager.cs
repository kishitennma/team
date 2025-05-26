using UnityEngine;
using UnityEngine.UI;

public class Player_Weapon_Manager : MonoBehaviour
{
    [Header("武器情報")]
    [SerializeField] Transform set_weapon_point;//武器所持用ポイント
    [SerializeField] GameObject[] hold_weapons;//所持している武器
    [SerializeField] Text ammo_texts;//弾丸のUI情報
    public Animator player_animator;//プレイヤーのアニメーションコントローラ
    [Header("切り替えボタン")]
    [SerializeField] KeyCode set_key = KeyCode.Q;//切り替えボタン
    public WeaponSystem weapon_system;//weapon_system ammo_text変更用

    private bool hold_secondry_weapon = false;//サブ武器の所持状態
    private void Start()
    {
        if (hold_weapons[0] != null)
        {
            Set_Weapon_hand(hold_weapons[0], hold_weapons[1]);//メイン武器を手に持たせる
        }
        else
        {
            Debug.LogError("メイン武器がよみこまれませんでした");
        }
    }
    private void Update()
    {
        if (hold_weapons[1]!= null)
        {
            //サブ武器を所持している、手に持っているのがメイン武器の場合set_keyで変更
            if(!hold_secondry_weapon && Input.GetKeyDown(set_key))
            {
                Set_Weapon_hand(hold_weapons[1], hold_weapons[0]);
                hold_secondry_weapon = true;
            }
            else if(hold_secondry_weapon && Input.GetKeyDown(set_key))
            {
                Set_Weapon_hand(hold_weapons[0], hold_weapons[1]);
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
        weapon_system = change_weapon.GetComponent<WeaponSystem>();
        weapon_system.ammo_text = ammo_texts;
        change_weapon.SetActive(true);
        change_weapon.transform.position = set_weapon_point.transform.forward;//ポイントの正面の方向へ向かせる
        set_weapon.SetActive(false);
        Debug.Log("武器を交換しました");
    }
    //武器変更用アニメーションストップ
    public void Set_End_Change_Anim()
    {
        player_animator.SetBool("Change_Weapon", false);
    }
}