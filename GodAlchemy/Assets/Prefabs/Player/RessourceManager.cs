using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RessourceManager : MonoBehaviour
{

    //Virtual ressources scores
    [SerializeField] private int foodScore;
    [SerializeField] private int woodScore;
    [SerializeField] private int stoneScore;
    [SerializeField] private int ironScore;
    [SerializeField] private int silverScore;
    [SerializeField] private int goldScore;
    [SerializeField] private int popScore;
    [SerializeField] private int civLevel;

    //UI score text
    [SerializeField] private GameUI gameUI;
    [SerializeField] private TextMeshProUGUI foodText;
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI ironText;
    [SerializeField] private TextMeshProUGUI silverText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI popText;
    [SerializeField] private TextMeshProUGUI civText;

    //Timer test
    public float timer = 2;


    // Start is called before the first frame update
    void Start()
    {
        gameUI = FindObjectOfType<GameUI>();
        foodText = gameUI.transform.Find("Canvas").transform.Find("Ressource Panel").transform.Find("FoodScore").transform.gameObject.GetComponent<TextMeshProUGUI>();
        woodText = gameUI.transform.Find("Canvas").transform.Find("Ressource Panel").transform.Find("WoodScore").transform.gameObject.GetComponent<TextMeshProUGUI>();
        stoneText = gameUI.transform.Find("Canvas").transform.Find("Ressource Panel").transform.Find("StoneScore").transform.gameObject.GetComponent<TextMeshProUGUI>();
        ironText = gameUI.transform.Find("Canvas").transform.Find("Ressource Panel").transform.Find("IronScore").transform.gameObject.GetComponent<TextMeshProUGUI>();
        silverText = gameUI.transform.Find("Canvas").transform.Find("Ressource Panel").transform.Find("SilverScore").transform.gameObject.GetComponent<TextMeshProUGUI>();
        goldText = gameUI.transform.Find("Canvas").transform.Find("Ressource Panel").transform.Find("GoldScore").transform.gameObject.GetComponent<TextMeshProUGUI>();
        popText = gameUI.transform.Find("Canvas").transform.Find("Ressource Panel").transform.Find("PopScore").transform.gameObject.GetComponent<TextMeshProUGUI>();
        civText = gameUI.transform.Find("Canvas").transform.Find("Ressource Panel").transform.Find("CivScore").transform.gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            RessourceType randomRessource = (RessourceType)Random.Range(0, 7);
            AddRessource(randomRessource, 100);
            AddRessource(RessourceType.CivLevel, 5);
            timer = 2;
        }
        if(civLevel > 25)
        {
            civLevel = 25;
        }
        UpdateUI();
    }

    public void AddRessource(RessourceType type, int amount)
    {
        switch(type)
        {
            case RessourceType.Food:
                foodScore += amount;
                break;
            case RessourceType.Wood:
                woodScore += amount;
                break;
            case RessourceType.Stone:
                stoneScore += amount;
                break;
            case RessourceType.Iron:
                ironScore += amount;
                break;
            case RessourceType.Silver:
                silverScore += amount;
                break;
            case RessourceType.Gold:
                goldScore += amount;
                break;
            case RessourceType.Population:
                popScore += amount;
                break;
            case RessourceType.CivLevel:
                civLevel += amount;
                break;
        }
            
    }

    public void UpdateUI()
    {
        foodText.text = foodScore.ToString();
        woodText.text = woodScore.ToString();
        stoneText.text = stoneScore.ToString();
        ironText.text = ironScore.ToString();
        silverText.text = silverScore.ToString();
        goldText.text = goldScore.ToString();
        popText.text = popScore.ToString();
        civText.text = civLevel.ToString();
    }


    public enum RessourceType
    {
        Food,
        Wood,
        Stone,
        Iron,
        Silver,
        Gold,
        Population,
        CivLevel
    };
}
