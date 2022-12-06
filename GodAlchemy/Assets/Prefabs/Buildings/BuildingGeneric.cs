using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingGeneric : NetworkBehaviour
{
    [RequiredField]
    public BuildingsScriptableObject buildingScriptObj; //scriptableobject script
    public Sprite spriteTimeBuilding;
    [SerializeField]
    private bool isBuild;
    //[ReadOnly]
    [SerializeField]
    private int hp;
    private ResourceManager resourceManager;
    private SpriteRenderer sr;

    //BuildingSelection
    [SerializeField]
    private BuildingManager buildingManager;
    [SerializeField]
    private BuildingInfosTable buildingInfosTable;
    private bool isSelected;



    private void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>(true);
        buildingInfosTable = FindObjectOfType<BuildingInfosTable>(true);
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
        isSelected = false;
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
        if(buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Economic))
        {
            gameObject.AddComponent<GatheringBuildings>();
        }
        ComputeCollider();
    }

    private void OnMouseDown()
    {
        OnSelectBuilding();
    }

    private void OnSelectBuilding()
    {
        if(buildingManager.selectedBuilding == null)
        {
            if (isSelected)
            {
                UnselectBuilding();
            }
            else
            {
                SelectBuilding();
            }
        }
        else
        {
            if(buildingManager.selectedBuilding != gameObject)
            {
                buildingManager.selectedBuilding.GetComponent<BuildingGeneric>().UnselectBuilding();
                SelectBuilding();
            }
            else
            {
                UnselectBuilding();
            }
        }
        
        buildingInfosTable.BuildingClicked();
    }

    private void UnselectBuilding()
    {
            isSelected = false;
            buildingManager.selectedBuilding = null;
    }

    private void SelectBuilding()
    {
        isSelected = true;
        buildingManager.selectedBuilding = gameObject;
    }

    private void OnBuildingDestroy()
    {
        if(buildingManager.selectedBuilding == gameObject)
        {
            UnselectBuilding();
            buildingInfosTable.BuildingClicked();
        }
    }
    public bool isSelectedBuilding()
    {
        return isSelected;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }
    private void Update()
    {
        if (hp <= 0)
        {
            OnBuildingDestroy();
            Destroy(gameObject);
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
}
