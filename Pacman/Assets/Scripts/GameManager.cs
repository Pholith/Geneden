using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Static instance of our GameManager, can be easly accessed anywhere
    public static GameManager Instance;

    [Header("Les autres managers")]

    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    public ScoreManager ScoreManager;
    [SerializeField]
    private GameObject map;
    [SerializeField]
    private GameState state;

    [SerializeField]
    private Transform coins;
    [SerializeField]
    private Transform pacman;

    [SerializeField]
    private List<Ghost> ghosts;

    [SerializeField]
    private int health = 3;

    // Function whenever a GameManager is instanciated.
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdateGameState(GameState.MainMenu);
    }

    // Update is called once per frame
    private void Update()
    {

        //Condition de victoire (si toutes les pièces sont mangées)
        if (!HasCoin() & (state == GameState.Game))
        {
            UpdateGameState(GameState.Victory);
        }

        if (health == 0)
        {
            UpdateGameState(GameState.Lose);
        }
    }

    //Reset de la game
    // TODO : Revoir le reset du Pacman (Revoir la façon dont spawn et se déplace le Pacman?)
    public void StartGame()
    {
        foreach (Transform pacman in pacman)
        {
            pacman.gameObject.transform.position = new Vector3(-0.5f, -9.50f, 0.0f);
        }
        mainMenu.SetUIVisible(false);
        map.SetActive(true);
        uiManager.ShowGameUI();
    }

    //Action lorsqu'une pièce est mangée
    // TODO : Ecrire la fonction pour le large Coin dans le même esprit.
    public void EatCoin(Coin coin)
    {
        ScoreManager.AddUpScore(coin.GetValue());
        ScoreManager.AddGlobalScore(coin.GetValue());
        coin.gameObject.SetActive(false);
    }

    public void EatLargeCoin(LargeCoin large_coin)
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.DebuffGhost();
        }
        EatCoin(large_coin);
    }

    public void PacmanDie()
    {
        FindObjectOfType<PacmanController>().isDead = true;
        health -= 1;
    }

    //Méthode pour vérifier s'il reste des pièces sur le terrain
    public bool HasCoin()
    {
        foreach (Transform coin in coins)
        {
            if (coin.gameObject.activeSelf)
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

        switch (new_state)
        {
            case GameState.MainMenu:
                OnMainMenu();
                break;
            case GameState.Game:
                StartGame();
                break;
            case GameState.Victory:
                OnVictory();
                break;
            case GameState.Lose:
                OnLose();
                break;
            case GameState.Quit:
                OnQuit();
                break;
        }
    }

    //Méthode d'affichage du MainMenu lorsque le jeu passe à l'état MainMenu
    private void OnMainMenu()
    {
        map.SetActive(false);
        uiManager.HideAll();
        mainMenu.SetUIVisible(true);

    }

    //Méthode d'affichage du Jeu lorsque le jeu passe à l'état Game
    private void OnGame()
    {
        StartGame();

    }

    //Méthode d'affichage de l'écran de Victoire lorsque le jeu passe à l'état Victory
    private void OnVictory()
    {
        uiManager.ShowVictory();
    }

    //Méthode d'affichage de l'écran de Défaite lorsque le jeu passe à l'état GameOver
    private void OnLose()
    {
        uiManager.ShowGameOver();
    }

    //Méthode lorsque le bouton Quit est pressé
    private void OnQuit()
    {
        Application.Quit();
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
