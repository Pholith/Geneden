using Fusion;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingGeneric : NetworkBehaviour
{
    [RequiredField]
    public BuildingsScriptableObject buildingScriptObj; //scriptableobject script
    public Sprite spriteTimeBuilding;
    [SerializeField]
    private bool isBuild;
    [ReadOnly]
    [SerializeField]
    private int hp;
    private ResourceManager resourceManager;
    private SpriteRenderer sr;

    //Upgrading
    private UpgradesScriptableObject PendingUpgrade = null;
    [SerializeField]
    private List<UpgradesScriptableObject> ListPendingUpgrade;
    private StopWatch UpgradeTimer;



    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
#if DEBUG
        buildingScriptObj.BuildingTime = 1;
#endif
        if (!buildingScriptObj)
        {
            Destroy(gameObject);
            Debug.LogWarning($"Component BuildingGeneric of {gameObject.name} doesn't have a script descriptor.");
            return;
        }
        // Test Condition sp�cial de construction
        if (buildingScriptObj.SpecialConditionRequired != null && buildingScriptObj.SpecialConditionRequired.Invoke())
        {
            // Test Condition Lvl Civilisation
            resourceManager = ResourceManager.Instance;
            if (resourceManager.GetCivLevel() >= buildingScriptObj.RequiredCivilisationLvl)
            {
                // Test Condition des ressources
                if (resourceManager.HasEnoughWood(buildingScriptObj.WoodCost) &&
                    resourceManager.HasEnoughStone(buildingScriptObj.RockCost) &&
                    resourceManager.HasEnoughIron(buildingScriptObj.IronCost) &&
                    resourceManager.HasEnoughSilver(buildingScriptObj.SilverCost) &&
                    resourceManager.HasEnoughGold(buildingScriptObj.GoldCost))
                {
                    // Consume ressources
                    resourceManager.ConsumeWood(buildingScriptObj.WoodCost);
                    resourceManager.ConsumeStone(buildingScriptObj.RockCost);
                    resourceManager.ConsumeIron(buildingScriptObj.IronCost);
                    resourceManager.ConsumeSilver(buildingScriptObj.SilverCost);
                    resourceManager.ConsumeGold(buildingScriptObj.GoldCost);

                    // buildingtime
                    sr.sprite = spriteTimeBuilding; // TODO Sprite construction;
                    new GameTimer(buildingScriptObj.BuildingTime, () => Build());
                }
                else
                {
                    Destroy(gameObject);
                    Debug.Log("Vous n'avez pas assez de ressources pour construire ce bat�ment.");
                    return;
                }
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("Vous n'avez pas atteint le niveau de civilisation nec�ssaire pour construire ce bat�ment.");
                return;
            }
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Vous ne pouvez pas encore construire ce bat�ment.");
            return;
        }
        hp = buildingScriptObj.MaxHealth;
        UpgradeTimer = this.AddComponent<StopWatch>();
        ListPendingUpgrade = new List<UpgradesScriptableObject>();
        ComputeCollider();
    }

    private void ComputeCollider()
    {
        // Génère un collider carré autour du sprite.
        BoxCollider2D collider = gameObject.AddOrGetComponent<BoxCollider2D>();
        Vector2 S = sr.sprite.bounds.size;
        collider.offset = new Vector2(1, 1);
        collider.size = new Vector3(sr.sprite.bounds.size.x / transform.lossyScale.x,
                                     sr.sprite.bounds.size.y / transform.lossyScale.y,
                                     sr.sprite.bounds.size.z / transform.lossyScale.z);
    }

    public BuildingsScriptableObject GetBuilding()
    {
        return buildingScriptObj;
    }

    public bool IsBuild()
    {
        return isBuild;
    }

    private void Build()
    {
        sr.sprite = buildingScriptObj.Sprite;
        isBuild = true;
        PlayerManager.Instance.AddBuilding(this);
        if (buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Gathering))
        {
            gameObject.AddComponent<GatheringBuildings>();
        }
        else if (buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.House))
        {
            HouseScriptableObject houseScript = (HouseScriptableObject)buildingScriptObj;
            ResourceManager.Instance.UpMaxPop(houseScript.AdditionalPopulation);
        }
        else if (buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Ressource))
        {
            ResourceNode _nodeBuilding = gameObject.AddComponent<ResourceNode>();
            switch (buildingScriptObj.name)
            {
                case "Ferme":
                    _nodeBuilding.SetMaxAmount(1000);
                    _nodeBuilding.SetGatheringSpeed(0.75f);
                    _nodeBuilding.SetRessourceType(ResourceManager.RessourceType.Food);
                    break;
            }
        }
        ComputeCollider();
    }

    private void OnBuildingDestroy()
    {
        SelectionManager _selectionManager = SelectionManager.Instance;
        if (_selectionManager.isSelected(this.GetComponent<SelectableObject>()))
        {
            _selectionManager.DeselectObject(this.GetComponent<SelectableObject>());
        }
        if(IsHouseBuilding())
        {
            HouseScriptableObject houseScript = (HouseScriptableObject)buildingScriptObj;
            ResourceManager.Instance.RemoveMaxPop(houseScript.AdditionalPopulation);
        }
        if (IsGatheringBuilding())
        {
            ResourceManager.Instance.AddRessource(ResourceManager.RessourceType.Population,this.GetComponent<GatheringBuildings>().GetWorker());
        }
        if(IsSearching())
        {
            StopUpgrade(PendingUpgrade);
            ListPendingUpgrade.Clear();
        }
        PlayerManager.Instance.RemoveBuilding(this);
    }

    private bool IsGatheringBuilding()
    {
        return buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Gathering);
    }

    private bool IsDivineBuilding()
    {
        return buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Divine);
    }

    private bool IsHouseBuilding()
    {
        return buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.House);
    }

    private void Update()
    {
        if (hp <= 0)
        {
            OnBuildingDestroy();
            Destroy(gameObject);
        }
        if(UpgradeTimer.IsActive())
        {
            if (UpgradeTimer.IsFinished())
                EndUpgrade();

        }
    }
    public void Damage(int damage)
    {
        hp -= damage;
    }

    public int GetHp()
    {
        return hp;
    }

    public bool CanAddUpgradeToPendingList()
    {
        return ListPendingUpgrade.Count + 1 < 5;
    }

    public void AddUpgradeToPendingList(UpgradesScriptableObject upgrade)
    {
        ListPendingUpgrade.Add(upgrade);
    }

    public void RemoveUpgradeToPendingList(UpgradesScriptableObject upgrade)
    {
        ListPendingUpgrade.Remove(upgrade);
    }

    public UpgradesScriptableObject GetUpgradeFromPendingList(UpgradesScriptableObject upgrade)
    {
        return ListPendingUpgrade[ListPendingUpgrade.IndexOf(upgrade)];
    }

    public List<UpgradesScriptableObject> GetPendingList()
    {
        return ListPendingUpgrade;
    }

    public bool IsPendingUpgradeListEmpty()
    {
        return ListPendingUpgrade.Count <= 0;
    }


    public void SearchUpgrade(UpgradesScriptableObject upgrade)
    {
        PendingUpgrade = upgrade;
        PlayerManager.Instance.StartUpgrade(upgrade);
        UpgradeTimer.SetEndTime(upgrade.ResearchTime);
        UpgradeTimer.StartCount();
    }

    public void SearchPendingListUpgrade(UpgradesScriptableObject upgrade)
    {
        PendingUpgrade = upgrade;
        UpgradeTimer.SetEndTime(upgrade.ResearchTime);
        UpgradeTimer.StartCount();
    }

    public void StopUpgrade(UpgradesScriptableObject upgrade)
    {
        PlayerManager.Instance.StopUpgrade(upgrade);
        PendingUpgrade = null;
        UpgradeTimer.StopCount();
        StartNextUpgradeInList();
        
    }

    private void StartNextUpgradeInList()
    {
        Debug.Log(!IsPendingUpgradeListEmpty());
        if (!IsPendingUpgradeListEmpty())
        {
            SearchPendingListUpgrade(ListPendingUpgrade[0]);
            ListPendingUpgrade.Remove(PendingUpgrade);
        }
        UpdateUpgradeUI();
    }
    public void EndUpgrade()
    {
        Debug.Log(PendingUpgrade.IsBuildingLevelUpUpgrade());
        if(PendingUpgrade.IsBuildingLevelUpUpgrade())
        {
            UpgradeTimer.StopCount();
            LevelUpBuilding();
            StartNextUpgradeInList();
            UpdateUpgradeUI();
        }
        else
        {
            PlayerManager.Instance.UnlockUpgrade(PendingUpgrade);
            UpgradeTimer.StopCount();
            PlayerManager.Instance.UpdateBuildingUpgrade();
            StartNextUpgradeInList();
            UpdateUpgradeUI();
        }
    }

    private void LevelUpBuilding()
    {
        
        if (buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.House))
        {
            HouseScriptableObject houseScript = (HouseScriptableObject)buildingScriptObj;
            HouseScriptableObject upgradeBuilding = (HouseScriptableObject)buildingScriptObj.UpgradeInto;
            ResourceManager.Instance.UpMaxPop(upgradeBuilding.AdditionalPopulation - houseScript.AdditionalPopulation);
            buildingScriptObj = upgradeBuilding;
        }
        else if(buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Gathering))
        {
            GatheringBuildings gatheringBuilding = this.GetComponent<GatheringBuildings>();
            GatheringBuildingScript upgradeBuilding = (GatheringBuildingScript)buildingScriptObj.UpgradeInto;
            gatheringBuilding.SetMaxWorker(upgradeBuilding.maxVillager);
            buildingScriptObj = upgradeBuilding;
            gatheringBuilding.SetBuildingScript(buildingScriptObj);
        }
        else
        {
            buildingScriptObj = buildingScriptObj.UpgradeInto;
        }
        hp = hp + (buildingScriptObj.MaxHealth - hp);
        sr.sprite = buildingScriptObj.Sprite;
        ComputeCollider();
        if(FindObjectOfType<BuildingInfosTable>(true).IsBuildingDisplayed(this))
            FindObjectOfType<BuildingInfosTable>(true).BuildingSelected(this);
    }

    public void UpdateUpgradeUI()
    {
        if (SelectionManager.Instance.isSelected(this.GetComponent<SelectableObject>()))
        {
            FindObjectOfType<BuildingInfosTable>(true).UpdateSearchingIcon();
            FindObjectOfType<BuildingInfosTable>(true).UpdateSearchingPendingList();
            FindObjectOfType<BuildingInfosTable>(true).UpdateUpgradesUI();
        }
    }

    public bool IsUpgradeInPendingList(UpgradesScriptableObject upgrade)
    {
        return (ListPendingUpgrade.Contains(upgrade));
    }

    public bool IsSearchingUpgrade(UpgradesScriptableObject upgrade)
    {
        return (((upgrade == PendingUpgrade) && (UpgradeTimer.IsActive())) || (IsUpgradeInPendingList(upgrade)));
    }

    public bool IsSearching()
    {
        return UpgradeTimer.IsActive();
    }

    public StopWatch GetTimer()
    {
        return UpgradeTimer;
    }

    public UpgradesScriptableObject GetPendingUpgrade()
    {
        return PendingUpgrade;
    }

    public void UpdateBuildingUpgrades()
    {
       if(IsGatheringBuilding())
       {
            gameObject.GetComponent<GatheringBuildings>().UpdateEfficiencyBonus();
            gameObject.GetComponent<GatheringBuildings>().UpdateGatherableRessources();
            
       }
    }



}
