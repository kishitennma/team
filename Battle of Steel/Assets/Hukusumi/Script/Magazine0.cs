using UnityEngine;
using UnityEngine.UI;

public class Magazine0 : MonoBehaviour
{
    public bool ZERO = false;
    float PassedTimes = 0;

    // �_�ł�����Ώہi������Behaviour�ɕύX����Ă���j
    [SerializeField] private Behaviour _target;
    // �_�Ŏ���[s]
    [SerializeField] private float _cycle = 1;

    // Update is called once per frame
    void Update()
    {
        if (ZERO)
        {
            PassedTimes += Time.deltaTime;//���Ԍo��

            var repeatValue = Mathf.Repeat((float)PassedTimes, _cycle);
            // ��������time�ɂ����閾�ŏ�Ԃ𔽉f
            _target.enabled = repeatValue >= _cycle * 0.5f;

        }
        else
        {
            PassedTimes = 1;
            _target.enabled = true;
        }
    }
}
