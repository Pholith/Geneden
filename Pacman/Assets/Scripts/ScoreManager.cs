using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public GameObject gameUI;
    public TextMeshProUGUI upScore;
    public TextMeshProUGUI globalScore;

    public int gScoreVar;
    public int upScoreVar;
    // Fonction de test qui récupère la valeur au lancement du text
    // Plus tard le score sera à 0 et cette fonction va disparaître
    void Awake()
    {
        string[] text = (globalScore.text).Split(",");
        string number = "";
        foreach( string c in text)
        {
            number += c;
        }
        gScoreVar = int.Parse(number);

        text = (upScore.text).Split(",");
        number = "";
        foreach (string c in text)
        {
            number += c;
        }
        upScoreVar = int.Parse(number);
    }

    public void addUpScore(int points)
    {
        if (!(upScoreVar >= 30000))
        {
            upScoreVar += points;
        }
    }

    public void addglobalScore(int points)
    {
        if (!(gScoreVar >= 30000))
        {
            gScoreVar += points;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    // Update en temps réel du Score.
    void Update()
    {
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        globalScore.text = gScoreVar.ToString("n0",nfi);
        upScore.text = upScoreVar.ToString("n0", nfi);
    }
}
