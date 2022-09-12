using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Game);
    }

    public void Quit()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Quit);
    }

    public void SetUIVisible(bool isVis)
    {
        gameObject.SetActive(isVis);
    }
}
