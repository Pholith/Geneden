using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject background;
    public GameObject victory_text;
    public GameObject lose_text;
    public GameObject game_UI;

    public GameObject health_bar;
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

    public void update_health()
    {
        foreach(Transform health_icon in health_bar.transform)
        {
            if((int.Parse(health_icon.name)) <= (FindObjectOfType<GameManager>().health))
            {
                health_icon.GetComponent<Image>().enabled = true;
            }
            else
            {
                health_icon.GetComponent<Image>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
