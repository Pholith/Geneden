using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ResourceManager : BaseManager<ResourceManager>
{

    protected override void InitManager()
    {
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

    //Divine Power
    [SerializeField] private int currentPower;
    [SerializeField] private int maxPower;

    //Divine Power bar
    [SerializeField] private Image divinePowerBar;
    [SerializeField] private TextMeshProUGUI midPowerText;
    [SerializeField] private TextMeshProUGUI topPowerText;

    [SerializeField]
    [Range(0, 10)]
    private float resourceRefillDelay = 1f;
    private float resourceRefill = 0;


    // Start is called before the first frame update
    private void Start()
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
        divinePowerBar = gameUI.transform.Find("Canvas").transform.Find("DivinPower").transform.Find("PowerFill").transform.gameObject.GetComponent<Image>();
        midPowerText = gameUI.transform.Find("Canvas").transform.Find("DivinPower").transform.Find("MidText").transform.gameObject.GetComponent<TextMeshProUGUI>();
        topPowerText = gameUI.transform.Find("Canvas").transform.Find("DivinPower").transform.Find("TopText").transform.gameObject.GetComponent<TextMeshProUGUI>();
        maxPower = 100;
        currentPower = 80;
#if DEBUG
        maxPower = 500;
        currentPower = 500;
#endif

    }

    public void Update()
    {
        resourceRefill += Time.deltaTime;
        if (resourceRefill > resourceRefillDelay)
        {
            resourceRefill = 0f;
            RegenDivinPower();
        }
        SetCurrentFill();
        UpdateUI();
    }

    private void RegenDivinPower()
    {
        RessourceType randomRessource = (RessourceType)UnityEngine.Random.Range(0, 7);
        AddRessource(randomRessource, 100);
        AddRessource(RessourceType.CivLevel, 1);
        AddDivinePower(2);
    }

    public void AddRessource(RessourceType type, int amount)
    {
        switch (type)
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
                if (civLevel + amount <= 25)
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
        midPowerText.text = (maxPower / 2).ToString();
        topPowerText.text = maxPower.ToString();
    }

    public void SetCurrentFill()
    {
        float _fillAmount = currentPower / (float)maxPower;
        divinePowerBar.fillAmount = _fillAmount;
    }

    public void AddDivinePower(int amount)
    {
        if (!((currentPower + amount) > maxPower))
        {
            currentPower += amount;
        }
        else
        {
            currentPower = maxPower;
        }
    }

    public void SetMaxDivinePower(int max)
    {
        maxPower = max;
    }

    public int GetMaxDivinePower()
    {
        return maxPower;
    }

    public void ConsumePower(int amount)
    {
        currentPower -= amount;
    }

    public bool HasEnoughPower(int substraction)
    {
        int _current = currentPower;
        if (!((_current - substraction) < 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int getCivLevel()
    {
        return civLevel;
    }


}
