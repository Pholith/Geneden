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
    [ReadOnly]
    private int hp;
    private ResourceManager resourceManager;
    private SpriteRenderer sr;

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
        ComputeCollider();
    }

    private void ComputeCollider()
    {
        // Génère un collider carré autour du sprite.
        BoxCollider2D collider = gameObject.AddOrGetComponent<BoxCollider2D>();
        Vector2 S = sr.sprite.bounds.size;
        collider.offset = new Vector2(0, 0);
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
    private void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Damage(int damage)
    {
        hp -= damage;
    }
}
