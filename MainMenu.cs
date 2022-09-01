using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject background;
    private GameObject title;
    private GameObject playButton;
    private GameObject quitButton;

    void Awake()
    {
        background = transform.Find("BackGround").gameObject;
        title = transform.Find("Title").gameObject;
        playButton = transform.Find("playButton").gameObject;
        quitButton = transform.Find("quitButton").gameObject;
    }

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
    }
}
