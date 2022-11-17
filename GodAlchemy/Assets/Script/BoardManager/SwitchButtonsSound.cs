using UnityEngine;

public class SwitchButtonsSound : MonoBehaviour
{
    public AudioSource buildingButton, ressourcesButton;

    public void playButtonBuilding() {
        if (buildingButton != null) {
            buildingButton.Play();
        }
    }

    public void playButtonRessources() {
        if (ressourcesButton != null) {
            ressourcesButton.Play();
        }
    }
}
