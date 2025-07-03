using UnityEngine;
using UnityEngine.UI;

public class GageGagerB : MonoBehaviour
{
    private Image image;//image取得
    public PlayerController player;//プレイヤー取得
    public bool Zero = false;//ヒート確認
    float PassedTimes = 0;//点滅用秒数
    //player.boost=ブーストゲージ現在値取得
    float max;//最大値

    // 点滅させる対象（ここがBehaviourに変更されている）
    [SerializeField] private Behaviour _target;
    // 点滅周期[s]
    [SerializeField] private float _cycle = 1;
    private void Start()
    {
        image = this.GetComponent<Image>();
        max = player.boost;

    }

    private void Update()
    {
        //完全回復まで点滅
        if (player.boost / max <= 0)
        {
            Zero = true;
        }
        else if (player.boost / max >= 1)
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

        }
        else
        {
            PassedTimes = 0;
            _target.enabled = true;//表示
        }
        //リカバリー
        if (max < player.boost)
        {
            max = player.boost;
        }

        //ゲージ管理
        image.fillAmount = player.boost / max;
    }
}