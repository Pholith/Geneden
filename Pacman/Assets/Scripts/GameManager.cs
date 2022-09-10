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
    public ScoreManager scoreManager;
    public GameObject map;
    public GameState state;

    public Transform coins;
    public Transform pacman;

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

        //Condition de victoire (si toutes les pi�ces sont mang�es)
        if(!hasCoin() & (state == GameState.Game))
        {
            UpdateGameState(GameState.Victory);
        }

    }

    //Reset de la game
    // TODO : Revoir le reset du Pacman (Revoir la fa�on dont spawn et se d�place le Pacman?)
    public void setGame()
    {
        foreach (Transform pacman in this.pacman)
        {
            pacman.gameObject.transform.position = new Vector3(-0.5f,-9.50f,0.0f);
        }
        foreach (Transform coin in this.coins)
        {
            coin.gameObject.SetActive(true);
        }
        mainMenu.setUIVisible(false);
        map.SetActive(true);
        uiManager.showGameUI();
    }

    //Action lorsqu'une pi�ce est mang�e
    // TODO : Ecrire la fonction pour le large Coin dans le m�me esprit.
    public void CoinEaten(Coin coin)
    {
        coin.gameObject.SetActive(false);
        scoreManager.addUpScore(coin.getValue());
        scoreManager.addglobalScore(coin.getValue());
    }

    //M�thode pour v�rifier s'il reste des pi�ces sur le terrain
    public bool hasCoin()
    {
        foreach(Transform coin in this.coins)
        {
            if(coin.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    //Fonction g�rant les transitions entre les diff�rents �tats du jeu.
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
    
    //M�thode d'affichage du MainMenu lorsque le jeu passe � l'�tat MainMenu
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

    //M�thode d'affichage de l'�cran de D�faite lorsque le jeu passe � l'�tat GameOver
    private void onLose()
    {
        print("Game has been quitted.");
        uiManager.showGameOver();
    }

    //M�thode lorsque le bouton Quit est press�
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
