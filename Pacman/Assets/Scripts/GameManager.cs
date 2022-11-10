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
    private Cherry cherry;

    [SerializeField]
    private List<Ghost> ghosts;

    [SerializeField]
    private int health = 3;

    public AudioSource munch1;
    public AudioSource munch2;
    public int currentMunch = 0;

    public AudioSource siren_bg;
    public AudioSource eat_ghost;
    public AudioSource eat_fruit;

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

        //Condition de victoire (si toutes les pi�ces sont mang�es)
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
    // TODO : Revoir le reset du Pacman (Revoir la fa�on dont spawn et se d�place le Pacman?)
    public void StartGame()
    {
        pacman.gameObject.transform.position = new Vector3(-0.5f, -9.50f, 0.0f);
        cherry.GameStart();
        mainMenu.SetUIVisible(false);
        map.SetActive(true);
        uiManager.ShowGameUI();
    }

    //Action lorsqu'une pi�ce est mang�e
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

    //M�thode pour v�rifier s'il reste des pi�ces sur le terrain
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

    //Fonction g�rant les transitions entre les diff�rents �tats du jeu.
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

    //M�thode d'affichage du MainMenu lorsque le jeu passe � l'�tat MainMenu
    private void OnMainMenu()
    {
        map.SetActive(false);
        uiManager.HideAll();
        mainMenu.SetUIVisible(true);

    }

    //M�thode d'affichage du Jeu lorsque le jeu passe � l'�tat Game
    private void OnGame()
    {
        StartGame();
        this.siren_bg.Play();

    }

    //M�thode d'affichage de l'�cran de Victoire lorsque le jeu passe � l'�tat Victory
    private void OnVictory()
    {
        uiManager.ShowVictory();
        this.siren_bg.Stop();
    }

    //M�thode d'affichage de l'�cran de D�faite lorsque le jeu passe � l'�tat GameOver
    private void OnLose()
    {
        uiManager.ShowGameOver();
        this.siren_bg.Stop();
    }

    //M�thode lorsque le bouton Quit est press�
    private void OnQuit()
    {
        Application.Quit();
        this.siren_bg.Stop();
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
