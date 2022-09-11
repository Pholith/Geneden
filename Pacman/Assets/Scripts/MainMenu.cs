using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject background;
    public GameObject title;
    public GameObject playButton;
    public GameObject quitButton;
    public GameObject namesText;

    public void Play()
    {
        GameManager.gameManager.UpdateGameState(GameManager.GameState.Game);
    }

    public void Quit()
    {
        GameManager.gameManager.UpdateGameState(GameManager.GameState.Quit);
    }

    public void setUIVisible(bool isVis)
    {
        background.SetActive(isVis);
        title.SetActive(isVis);
        playButton.SetActive(isVis);
        quitButton.SetActive(isVis);
        namesText.SetActive(isVis);
    }
}
