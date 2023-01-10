using UnityEngine;

public class OptionsSoundDesign : MonoBehaviour
{
    public AudioSource audioClose, audioGraphics;

    public void playButtonClose() {
        audioClose.Play();
    }

    public void playButtonGraphics() {
        audioGraphics.Play();
    }
    
}
