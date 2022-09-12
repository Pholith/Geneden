using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject victory_text;
    [SerializeField]
    private GameObject lose_text;
    [SerializeField]
    private GameObject game_UI;
    // Start is called before the first frame update

    public void ShowVictory()
    {
        background.SetActive(true);
        victory_text.SetActive(true);
        lose_text.SetActive(false);
    }

    public void ShowGameOver()
    {
        background.SetActive(true);
        lose_text.SetActive(true);
        victory_text.SetActive(false);
    }

    public void ShowGameUI()
    {
        game_UI.SetActive(true);
        background.SetActive(false);
        victory_text.SetActive(false);
        lose_text.SetActive(false);
    }

    public void HideAll()
    {
        game_UI.SetActive(false);
        background.SetActive(false);
        victory_text.SetActive(false);
        lose_text.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
