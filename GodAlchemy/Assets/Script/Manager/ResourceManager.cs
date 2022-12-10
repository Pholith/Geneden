using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        MaxPopulation,
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
    [SerializeField] private int realPop;
    [SerializeField] private int maxPop;
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
    [RequiredField]
    [SerializeField]
    private DivinPowerBar powerBar;

    
    private float baseResourceRefillDelay = 1f;
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
        powerBar.PowerMax = 100;
        powerBar.CurrentPower = 80;
#if DEBUG
        powerBar.PowerMax = 200;
        powerBar.CurrentPower = 180;
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
        UpdateUI();
    }

    private void RegenDivinPower()
    {
        //RessourceType randomRessource = (RessourceType)UnityEngine.Random.Range(0, 7);
        //AddRessource(randomRessource, 100);
        //AddRessource(RessourceType.CivLevel, 1);
        powerBar.CurrentPower += 2;
    }

    public void AddMaxPower(int addition)
    {
        powerBar.PowerMax += addition;
        AddPower(addition);


    }

    public void AddPower(int addition)
    {
        if (powerBar.CurrentPower + addition <= powerBar.PowerMax)
            powerBar.CurrentPower += addition;
        else
            powerBar.CurrentPower += powerBar.PowerMax - powerBar.CurrentPower;
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

    public int GetRessourcePerType(RessourceType type)
    {
        switch (type)
        {
            case RessourceType.Food:
                return foodScore;
            case RessourceType.Wood:
                return woodScore;
            case RessourceType.Stone:
                return stoneScore;
            case RessourceType.Iron:
                return ironScore;
            case RessourceType.Silver:
                return silverScore;
            case RessourceType.Gold:
                return goldScore;
            case RessourceType.Population:
                return popScore;
            case RessourceType.CivLevel:
                return civLevel;
            case RessourceType.MaxPopulation:
                return maxPop;
        }

        return 0;
    }

    public void UpMaxPop(int amount)
    {
        if (maxPop >= realPop)
        {
            maxPop += amount;
            realPop += amount;
            popScore += amount;
        }
        else
        {
            maxPop += amount;
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
        popText.text = popScore.ToString() + "/" + maxPop.ToString();
        civText.text = civLevel.ToString();
    }

    public bool HasEnoughRessource(ResourceManager.RessourceType type, int amount)
    {
        return GetRessourcePerType(type) - amount >= 0;
    }

    public void ToggleShowCost(int cost = -1)
    {
        powerBar.ToggleShowCost(cost);
    }
    public void ConsumePower(int cost)
    {
        powerBar.CurrentPower -= cost;
    }

    public bool HasEnoughPower(int substraction)
    {
        return powerBar.CurrentPower - substraction > 0;
    }

    public bool CanAddPower(int addition)
    {
        return powerBar.CurrentPower + addition <= powerBar.PowerMax;
    }
    public int GetCivLevel()
    {
        return civLevel;
    }

    public void ConsumeWood(int cost)
    {
        woodScore -= cost;
    }
    public bool HasEnoughWood(int substraction)
    {
        return woodScore - substraction >= 0;
    }

    public void ConsumeIron(int cost)
    {
        ironScore -= cost;
    }
    public bool HasEnoughIron(int substraction)
    {
        return ironScore - substraction >= 0;
    }

    public void ConsumeStone(int cost)
    {
        stoneScore -= cost;
    }
    public bool HasEnoughStone(int substraction)
    {
        return stoneScore - substraction >= 0;
    }

    public void ConsumeSilver(int cost)
    {
        silverScore -= cost;
    }
    public bool HasEnoughSilver(int substraction)
    {
        return silverScore - substraction >= 0;
    }

    public void ConsumeGold(int cost)
    {
        goldScore -= cost;
    }
    public bool HasEnoughGold(int substraction)
    {
        return goldScore - substraction >= 0;
    }

    public void ConsumePop(int pop)
    {
        popScore -= pop;
    }
    public bool HasEnoughPop(int substraction)
    {
        return popScore - substraction >= 0;
    }

    public void AddMaxPop(int pop)
    {
        maxPop += pop;
    }

    public void RemoveMaxPop(int pop)
    {
        maxPop -= pop;
    }

    public float GetRefillDelay()
    {
        return resourceRefillDelay;
    }

    public void ReduceRefillDelayPercentage(float bonusPerCent)
    {
        resourceRefillDelay -= baseResourceRefillDelay * bonusPerCent;
        Debug.Log(baseResourceRefillDelay - (baseResourceRefillDelay * bonusPerCent));
    }
}
