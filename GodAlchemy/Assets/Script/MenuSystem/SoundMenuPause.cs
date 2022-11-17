using UnityEngine;

public class SoundMenuPause : MonoBehaviour
{
    public AudioSource audioOptions, audioQuit;

    public void playButtonOptions() {
        if (audioOptions != null) {
            audioOptions.Play();
        }
        
    }

    public void playButtonQuit() {
        if (audioQuit != null) {
            audioQuit.Play();
        }
        
    }
    
}
