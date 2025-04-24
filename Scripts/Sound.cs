using UnityEngine.Audio;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource jumpAudioSource;

    public void PlayJumpSound(){
        jumpAudioSource.Play();
    }
}
