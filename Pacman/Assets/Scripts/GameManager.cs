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

        //Condition de victoire (si toutes les pi�ces sont mang�es)
        if (!hasCoin() & (state == GameState.Game))
        {
            UpdateGameState(GameState.Victory);
        }
        
        if((state == GameState.Game) & (health == 0 ) & (FindObjectOfType<PacmanController>(true).deathTimer <= 0))
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

        if ((state == GameState.Game))
        {
            uiManager.update_health();
            if ((FindObjectOfType<PacmanController>(true).isDead) & (FindObjectOfType<PacmanController>(true).deathTimer <= 0))
            {
                Ghost[] ghostlist = FindObjectsOfType<Ghost>(true);
                foreach (Ghost ghost in ghostlist)
                {
                    print(ghost);
                    ghost.gameObject.SetActive(true);
                    ghost.transform.localPosition = ghost.initial_pos;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UpdateGameState(GameState.MainMenu);
            }
        }

        if ((state == GameState.Lose) || (state == GameState.Victory))
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                UpdateGameState(GameState.MainMenu);
            }
        }
        

    }

    //Reset de la game
    // TODO : Revoir le reset du Pacman (Revoir la fa�on dont spawn et se d�place le Pacman?)
    public void setGame()
    {
        health = 3;
        Ghost[] ghostlist = FindObjectsOfType<Ghost>(true);
        Coin[] coinlist = FindObjectsOfType<Coin>(true);
        PacmanController _pacman = FindObjectOfType<PacmanController>(true);
        foreach (Ghost ghost in ghostlist)
        {
            ghost.gameObject.SetActive(true);
            ghost.GhostReset();
        }
        foreach (Coin coin in coinlist)
        {
            coin.gameObject.SetActive(true);
        }

        _pacman.transform.position = new Vector3(-0.5f, -9.50f, 0.0f);
        _pacman.Direction = new Vector2(0, 0);
        _pacman.NextDirection = new Vector2(0, 0);
        _pacman.gameObject.SetActive(true);
        mainMenu.setUIVisible(false);
        map.SetActive(true);
        scoreManager.gScoreVar = 0;
        uiManager.showGameUI();
    }

    //Action lorsqu'une pi�ce est mang�e
    // TODO : Ecrire la fonction pour le large Coin dans le m�me esprit.
    public void CoinEaten(Coin coin)
    {
        coin.gameObject.SetActive(false);
        scoreManager.addScore(coin.getValue());
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
        print("You die");
        PacmanController _pacman = FindObjectOfType<PacmanController>();
        _pacman.GetComponent<CircleCollider2D>().enabled = false;
        _pacman.isDead = true;
        health -= 1;

        Ghost[] ghostlist = FindObjectsOfType<Ghost>();
        foreach (Ghost ghost in ghostlist)
        {
            ghost.gameObject.SetActive(false);
        }
    }

    public void GhostEaten(Ghost ghost)
    {

        if(ghostPoint != 0)
        {
            ghostPoint = ghostPoint * 2;
            scoreManager.addScore(ghostPoint);
            
        }
        else
        {
            ghostPoint = ghost.point;
            scoreManager.addScore(ghost.point);
        }

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

    //M�thode d'affichage du Jeu lorsque le jeu passe � l'�tat Game
    private void onGame()
    {
        setGame();
        print("Game Launched!");
        

    }

    //M�thode d'affichage de l'�cran de Victoire lorsque le jeu passe � l'�tat Victory
    private void onVictory()
    {
        print("You won.");
        uiManager.showVictory();
        foreach (Ghost ghost in FindObjectsOfType<Ghost>(true))
        {
            ghost.gameObject.SetActive(false);
        }
        FindObjectOfType<PacmanController>(true).gameObject.SetActive(false);
    }

    //M�thode d'affichage de l'�cran de D�faite lorsque le jeu passe � l'�tat GameOver
    private void onLose()
    {
        uiManager.showGameOver();
        foreach (Ghost ghost in FindObjectsOfType<Ghost>(true))
        {
            ghost.gameObject.SetActive(false);
        }
        FindObjectOfType<PacmanController>(true).gameObject.SetActive(false);
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
