using UnityEngine;
using UnityEngine.UI;

public class GageGagerB : MonoBehaviour
{
    private float boostG;//ブーストゲージ現在値取得
    private Image image;
    public PlayerController player;//プレイヤー取得
    public bool Zero = false;//ヒート確認
    float PassedTimes = 0;//点滅用秒数

    float max;//最大値

    // 点滅させる対象（ここがBehaviourに変更されている）
    [SerializeField] private Behaviour _target;
    // 点滅周期[s]
    [SerializeField] private float _cycle = 1;
    private void Start()
    {
        image = this.GetComponent<Image>();
        boostG = player.boost;
        max = boostG;

    }

    private void Update()
    {
        //完全回復まで点滅
        boostG = player.boost;
        if (boostG / max <= 0)
        {
            Zero = true;
        }
        else if (boostG / max >= 1)
        {
            Zero = false;
        }

        //点滅プログラム
        if (Zero)
        {
            PassedTimes += Time.deltaTime;//時間経過

            var repeatValue = Mathf.Repeat((float)PassedTimes, _cycle);
            // 内部時刻timeにおける明滅状態を反映
            _target.enabled = repeatValue >= _cycle * 0.5f;
            //boostG++;

        }
        else
        {
            PassedTimes = 1;
            _target.enabled = true;
        }

        //旧デバック
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    boostG--;
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    boostG++;
        //}

        //ゲージ管理
        image.fillAmount = boostG / max;
        if (boostG / max < 0)
        {
            image.fillAmount = 0.0f;
            boostG = 0.0f;
        }
        else if (boostG / max > 1.0f)
        {
            image.fillAmount = 1.0f;
            boostG = max;
        }
    }
}