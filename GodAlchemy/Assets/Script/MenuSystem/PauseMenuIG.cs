using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuIG : MonoBehaviour
{

    public string mainMenuScene;
    public GameObject pauseMenu;
    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                resumeGame();
            } else
            {
                isPaused = true;
                pauseMenu.SetActive(true);

            }
        }
    }

    public void resumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void openOptionsGames()
    {
        Debug.Log("Open Options !");
    }

    public void closeOptionsGames()
    {

    }

    public void quitGame()
    {
        //Abandon de la game
        SceneManager.LoadScene(mainMenuScene);

        Debug.Log("Quitting game !");
    }
}
