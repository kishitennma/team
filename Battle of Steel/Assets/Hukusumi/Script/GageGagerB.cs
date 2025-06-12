using UnityEngine;
using UnityEngine.UI;

public class GageGagerB : MonoBehaviour
{
    private float boostG;
    private Image image;
    public PlayerController player;
    public bool Zero = false;
    float PassedTimes = 0;

    float max;

    // �_�ł�����Ώہi������Behaviour�ɕύX����Ă���j
    [SerializeField] private Behaviour _target;
    // �_�Ŏ���[s]
    [SerializeField] private float _cycle = 1;
    private void Start()
    {
        image = this.GetComponent<Image>();
        boostG = player.boost;
        max = boostG;

    }

    private void Update()
    {
        boostG = player.boost;
        if (boostG / max <= 0)
        {
            Zero = true;
        }
        else if (boostG / max >= 1)
        {
            Zero = false;
        }

        if (Zero)
        {
            PassedTimes += Time.deltaTime;//���Ԍo��

            var repeatValue = Mathf.Repeat((float)PassedTimes, _cycle);
            // ��������time�ɂ����閾�ŏ�Ԃ𔽉f
            _target.enabled = repeatValue >= _cycle * 0.5f;
            //boostG++;

        }
        else
        {
            PassedTimes = 1;
            _target.enabled = true;
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            boostG--;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            boostG++;
        }

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