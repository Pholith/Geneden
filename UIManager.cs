using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject background;
    private GameObject victory_text;
    private GameObject lose_text;
    private GameObject game_UI;
    // Start is called before the first frame update
    void Awake()
    {
        background = transform.Find("BackGround").gameObject;
        victory_text = transform.Find("VictoryText").gameObject;
        lose_text = transform.Find("GameOverText").gameObject;
        game_UI = transform.Find("GameUI").gameObject;
    }

    public void showVictory()
    {
        background.SetActive(true);
        victory_text.SetActive(true);
        lose_text.SetActive(false);
    }

    public void showGameOver()
    {
        background.SetActive(true);
        lose_text.SetActive(true);
        victory_text.SetActive(false);
    }

    public void showGameUI()
    {
        game_UI.SetActive(true);
        background.SetActive(false);
        victory_text.SetActive(false);
        lose_text.SetActive(false);
    }

    public void hideAll()
    {
        game_UI.SetActive(false);
        background.SetActive(false);
        victory_text.SetActive(false);
        lose_text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
