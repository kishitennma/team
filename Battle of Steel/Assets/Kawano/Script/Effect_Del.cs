using UnityEngine;

public class Effect_Del : MonoBehaviour
{
    public ParticleSystem Effects;//�p�[�e�B�N���I�u�W�F�N�g

    // Update is called once per frame
    void Update()
    {
        //�p�[�e�B�N������A
        if(Effects != null && !Effects.IsAlive())
        {
            Destroy(gameObject);//�Q�[���I�u�W�F�N�g�I��
        }
    }
}
