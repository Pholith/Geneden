using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject background;
    public GameObject victory_text;
    public GameObject lose_text;
    public GameObject game_UI;
    // Start is called before the first frame update

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
