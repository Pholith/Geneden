using Fusion;
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
        PlayerManager.Instance.RemoveBuilding(this);
    }

    private bool IsGatheringBuilding()
    {
        return buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Gathering);
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

    public void SearchUpgrade(UpgradesScriptableObject upgrade)
    {
        PendingUpgrade = upgrade;
        PlayerManager.Instance.StartUpgrade(upgrade);
        UpgradeTimer.SetEndTime(upgrade.ResearchTime);
        UpgradeTimer.StartCount();
    }

    public void StopUpgrade(UpgradesScriptableObject upgrade)
    {
        PlayerManager.Instance.StopUpgrade(upgrade);
        PendingUpgrade = null;
        UpgradeTimer.StopCount();
        
    }

    public void EndUpgrade()
    {
        PlayerManager.Instance.UnlockUpgrade(PendingUpgrade);
        UpgradeTimer.StopCount();
        PlayerManager.Instance.UpdateBuildingUpgrade();
        if (SelectionManager.Instance.isSelected(this.GetComponent<SelectableObject>()))
            FindObjectOfType<BuildingInfosTable>().UpdateUpgradesUI();
            
    }

    public bool IsSearchingUpgrade(UpgradesScriptableObject upgrade)
    {
        return ((upgrade == PendingUpgrade) && (UpgradeTimer.IsActive()));
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
