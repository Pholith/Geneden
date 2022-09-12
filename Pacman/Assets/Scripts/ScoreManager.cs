using System.Globalization;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    private GameObject gameUI;
    [SerializeField]
    private TextMeshProUGUI upScore;
    [SerializeField]
    private TextMeshProUGUI globalScore;

    [SerializeField]
    private int gScoreVar;
    [SerializeField]
    private int upScoreVar;

    // Fonction de test qui récupère la valeur au lancement du text
    // Plus tard le score sera à 0 et cette fonction va disparaître
    private void Awake()
    {
        string[] text = globalScore.text.Split(",");
        string number = "";
        foreach (string c in text)
        {
            number += c;
        }
        gScoreVar = int.Parse(number);

        text = upScore.text.Split(",");
        number = "";
        foreach (string c in text)
        {
            number += c;
        }
        upScoreVar = int.Parse(number);
    }

    public void AddUpScore(int points)
    {
        if (!(upScoreVar >= 30000))
        {
            upScoreVar += points;
        }
    }

    public void AddGlobalScore(int points)
    {
        if (!(gScoreVar >= 30000))
        {
            gScoreVar += points;
        }
    }

    private void Start()
    {

    }

    // Update is called once per frame
    // Update en temps réel du Score.
    private void Update()
    {
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        globalScore.text = gScoreVar.ToString("n0", nfi);
        upScore.text = upScoreVar.ToString("n0", nfi);
    }
}
