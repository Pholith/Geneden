using UnityEngine;

public class SoundMenu : MonoBehaviour
{
    public AudioSource audioResume, audioOptions, audioQuit;

    public void playButtonResume() {
        audioResume.Play();
    }

    public void playButtonOptions() {
        audioOptions.Play();
    }

    public void playButtonQuit() {
        audioQuit.Play();
    }
    
}
