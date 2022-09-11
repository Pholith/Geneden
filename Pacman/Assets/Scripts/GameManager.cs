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
    public Transform ghosts;

    public float multiplierTimer = 0.0f;
    public int ghostPoint = 0;

    public int health = 3;

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

        //Condition de victoire (si toutes les pièces sont mangées)
        if(!hasCoin() & (state == GameState.Game))
        {
            UpdateGameState(GameState.Victory);
        }

        if(health == 0)
        {
            UpdateGameState(GameState.Lose);
        }
        if(multiplierTimer > 0)
        {
            multiplierTimer -= Time.smoothDeltaTime;
        }
        else
        {
            ghostPoint = 0;
        }

    }

    //Reset de la game
    // TODO : Revoir le reset du Pacman (Revoir la façon dont spawn et se déplace le Pacman?)
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
        foreach (Transform ghost in this.ghosts)
        {
            ghost.gameObject.SetActive(true);
        }
        mainMenu.setUIVisible(false);
        map.SetActive(true);
        uiManager.showGameUI();
    }

    //Action lorsqu'une pièce est mangée
    // TODO : Ecrire la fonction pour le large Coin dans le même esprit.
    public void CoinEaten(Coin coin)
    {
        coin.gameObject.SetActive(false);
        scoreManager.addUpScore(coin.getValue());
        scoreManager.addglobalScore(coin.getValue());
    }

    public void LargeCoinEaten(Large_Coin large_coin)
    {
        CoinEaten(large_coin);
        Ghost[] ghostlist = FindObjectsOfType<Ghost>();
        foreach (Ghost ghost in ghostlist)
        {
            ghost.isDebuff = true;
            ghost.debuffTime = 6.0f;
        }
        multiplierTimer = 6.0f;
    }

    public void PacmanDie()
    {
        FindObjectOfType<PacmanController>().isDead = true;
        health -= 1;
    }

    public void GhostEaten(Ghost ghost)
    {

        if(ghostPoint != 0)
        {
            ghostPoint = ghostPoint * 2;
            scoreManager.addUpScore(ghostPoint);
            scoreManager.addglobalScore(ghostPoint);
            
        }
        else
        {
            ghostPoint = ghost.point;
            scoreManager.addUpScore(ghost.point);
            scoreManager.addglobalScore(ghost.point);
        }

    }


    //Méthode pour vérifier s'il reste des pièces sur le terrain
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

    //Fonction gérant les transitions entre les différents états du jeu.
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
    
    //Méthode d'affichage du MainMenu lorsque le jeu passe à l'état MainMenu
    private void onMainMenu()
    {
        print("MainMenu is loaded!");
        map.SetActive(false);
        uiManager.hideAll();
        mainMenu.setUIVisible(true);

    }

    //Méthode d'affichage du Jeu lorsque le jeu passe à l'état Game
    private void onGame()
    {
        setGame();
        print("Game Launched!");
        

    }

    //Méthode d'affichage de l'écran de Victoire lorsque le jeu passe à l'état Victory
    private void onVictory()
    {
        print("You won.");
        uiManager.showVictory();
    }

    //Méthode d'affichage de l'écran de Défaite lorsque le jeu passe à l'état GameOver
    private void onLose()
    {
        print("Game has been quitted.");
        uiManager.showGameOver();
    }

    //Méthode lorsque le bouton Quit est pressé
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
