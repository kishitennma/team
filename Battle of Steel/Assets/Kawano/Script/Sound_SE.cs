using UnityEngine;

public class Sound_SE : MonoBehaviour
{
    public AudioSource ASound;//SE

    public void Playing_Sound(AudioClip clip)
    {
        ASound.resource = clip;
        ASound.Play();
    }
}