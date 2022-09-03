using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Static instance of our GameManager, can be easly accessed anywhere.
    public static GameManager gameManager;
    public UIManager uiManager;
    public MainMenu mainMenu;
    public GameObject map;
    public GameState state;

    // Function whenever a GameManager is instanciated.
    void Awake()
    {
        gameManager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            UpdateGameState(GameState.Lose);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            UpdateGameState(GameState.Victory);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            UpdateGameState(GameState.Game);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateGameState(GameState.MainMenu);
        }

    }

    public void UpdateGameState(GameState new_state)
    {
        state = new_state;

        switch(new_state)
        {
            case GameState.MainMenu:
                onMainMenu();
                break;
            case GameState.Game:
                onGame();
                break;
            case GameState.Victory:
                onVictory();
                break;
            case GameState.Lose:
                onLose();
                break;
            case GameState.Quit:
                onQuit();
                break;
        }
    }
    
    private void onMainMenu()
    {
        print("MainMenu is loaded!");
        map.SetActive(false);
        uiManager.hideAll();
        mainMenu.setUIVisible(true);
    }

    private void onGame()
    {
        print("Game Launched!");
        mainMenu.setUIVisible(false);
        map.SetActive(true);
        uiManager.showGameUI();

    }

    private void onVictory()
    {
        print("You won.");
        uiManager.showVictory();
    }

    private void onLose()
    {
        print("Game has been quitted.");
        uiManager.showGameOver();
    }

    private void onQuit()
    {
        Application.Quit();
        print("Game has been quitted.");
    }



    public enum GameState
    {
        MainMenu,
        Game,
        Victory,
        Lose,
        Quit,
    }
}
