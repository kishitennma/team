using UnityEngine;
using UnityEngine.UI;

public class Magazine0 : MonoBehaviour
{
    public bool ZERO = false;
    float PassedTimes = 0;

    // 点滅させる対象（ここがBehaviourに変更されている）
    [SerializeField] private Behaviour _target;
    // 点滅周期[s]
    [SerializeField] private float _cycle = 1;

    // Update is called once per frame
    void Update()
    {
        if (ZERO)
        {
            PassedTimes += Time.deltaTime;//時間経過

            var repeatValue = Mathf.Repeat((float)PassedTimes, _cycle);
            // 内部時刻timeにおける明滅状態を反映
            _target.enabled = repeatValue >= _cycle * 0.5f;

        }
        else
        {
            PassedTimes = 1;
            _target.enabled = true;
        }
    }
}
