using UnityEngine;

public class FullScreenAudio : MonoBehaviour
{
    public AudioSource audioToggle;

    public void playToggleSound() {
        audioToggle.Play();
    }
    
}
