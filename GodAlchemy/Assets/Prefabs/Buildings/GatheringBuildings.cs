using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatheringBuildings : MonoBehaviour
{
    private GatheringBuildingScript building;
    [SerializeField]
    private ResourceNode TargetedNode;
    private ResourceManager resourceManager;
    private GridManager gridManager;
    private BuildingGeneric baseBuilding;

    //Nodes
    [SerializeField]
    private List<Collider2D> collidersInRangeList;
    private GameObject buildingRange;
    private BuildingManager buildingManager;

    //Gathering Stuff
    [SerializeField]
    private List<ResourceManager.RessourceType> gatherableRessources;
    [SerializeField]
    private int workers;
    [SerializeField]
    private int maxWorkers;
    [SerializeField]
    private int gatheringPerCycle;
    [SerializeField]
    private float effeciencyBonus;
    [SerializeField]
    private float secondToWaitBeforeGather;

    private StopWatch gatheringTimer;
    public const float GATHERINGTIMER = 15.0f;
    public const float WAITINGTIMER = 1.0f;

    // Start is called before the first frame update
    private void Start()
    {
        building = (GatheringBuildingScript)GetComponent<BuildingGeneric>().GetBuilding();
        //Gathering Stuff
        gatherableRessources = building.gatherableRessource;
        workers = 0;
        maxWorkers = building.maxVillager;
        gatheringPerCycle = 10;
        effeciencyBonus = 0 + PlayerManager.Instance.CheckEfficiencyUpgrades(building, UpgradesScriptableObject.UpgradeType.Efficiency);
        gatheringTimer = gameObject.AddComponent<StopWatch>();
        secondToWaitBeforeGather = WAITINGTIMER;



        //Other

        collidersInRangeList = new List<Collider2D>();
        resourceManager = FindObjectOfType<ResourceManager>();
        gridManager = FindObjectOfType<GridManager>();
        buildingManager = FindObjectOfType<BuildingManager>();
        baseBuilding = GetComponent<BuildingGeneric>();
        CreateBuildingRange();
        TargetedNode = null;
        //StartCoroutine(GatherRoutine());
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsGathering())
            gatheringTimer.StopCount();
        if(!IsThereNoWorker())
            GetNodes();
        if (gatheringTimer.IsFinished())
            GatherNode();
    }

    public void UpdateEfficiencyBonus()
    {
        effeciencyBonus = 0 + PlayerManager.Instance.CheckEfficiencyUpgrades(building, UpgradesScriptableObject.UpgradeType.Efficiency);
        if (IsGathering())
        {
            secondToWaitBeforeGather = (GATHERINGTIMER / TargetedNode.GetGatheringSpeed()) - ((GATHERINGTIMER / TargetedNode.GetGatheringSpeed()) * effeciencyBonus);
            gatheringTimer.SetEndTime(secondToWaitBeforeGather);
        }

    }

    public void UpdateGatherableRessources()
    {
        gatherableRessources = PlayerManager.Instance.CheckGatherableRessources(building, UpgradesScriptableObject.UpgradeType.UnlockRessource);
    }

    private void CreateBuildingRange()
    {
        buildingRange = Instantiate(buildingManager.buildingRangePrefab);
        buildingRange.transform.SetParent(transform);
        buildingRange.transform.localPosition = new Vector3(1f, 1f, 0);
        buildingRange.transform.localScale = new Vector3(building.gatheringRange * gridManager.MainGameGrid.cellSize.x, building.gatheringRange * gridManager.MainGameGrid.cellSize.y, 1);
        buildingRange.SetActive(false);
    }

    public void ShowRange(bool show)
    {
        buildingRange.SetActive(show);
    }

    public void GetNodes()
    {
        if (!IsGathering())
        {

            collidersInRangeList = gridManager.GetResourcesInRange(gridManager.GetGridPos(transform.position + new Vector3(0.5f, 0.5f, 0f)), Mathf.RoundToInt((building.gatheringRange / 2) * gridManager.MainGameGrid.cellSize.x));
        }
        else
        {
            collidersInRangeList = new List<Collider2D>();
            return;
        }
        if (collidersInRangeList.Any())
        {
            foreach (Collider2D collider in collidersInRangeList)
            {
                if (collider.gameObject.GetComponent<ResourceNode>() != null)
                {
                    if (gatherableRessources.Contains(collider.gameObject.GetComponent<ResourceNode>().GetResourceType()))
                    {
                        TargetedNode = collider.gameObject.GetComponent<ResourceNode>();
                        secondToWaitBeforeGather = (GATHERINGTIMER / TargetedNode.GetGatheringSpeed()) - ((GATHERINGTIMER / TargetedNode.GetGatheringSpeed()) * effeciencyBonus);
                        gatheringTimer.SetEndTime(secondToWaitBeforeGather);
                        gatheringTimer.StartCount();
                        return;
                    }
                }
            }
        }
        TargetedNode = null;
    }

    /*private IEnumerator GatherRoutine()
    {
        while (true)
        {
            //RessourceType randomRessource = (RessourceType)Random.Range(0, 7);
            //AddRessource(randomRessource, 100);
            //AddRessource(RessourceType.CivLevel, 5);
            GatherNode();
            yield return new WaitForSeconds(secondToWaitBeforeGather);
        }

    }*/

    public bool IsGathering()
    {
        return (TargetedNode != null);
    }

    public void GatherNode()
    {
        if (IsGathering())
        {
            int nodeAmount = TargetedNode.GetCurrentAmout();
            if (TargetedNode.DeleteResource(gatheringPerCycle * workers))
            {
                resourceManager.AddRessource(TargetedNode.GetResourceType(), gatheringPerCycle * workers);
                secondToWaitBeforeGather = (GATHERINGTIMER / TargetedNode.GetGatheringSpeed()) - ((GATHERINGTIMER / TargetedNode.GetGatheringSpeed()) * effeciencyBonus);
                gatheringTimer.SetEndTime(secondToWaitBeforeGather);
                gatheringTimer.StartCount();
            }
            else if (TargetedNode.DeleteResource(TargetedNode.GetCurrentAmout()))
            {
                resourceManager.AddRessource(TargetedNode.GetResourceType(), nodeAmount);
                TargetedNode = null;
                gatheringTimer.StopCount();
            }
        }
        else
        {
            TargetedNode = null;
            gatheringTimer.StopCount();
        }
    }

    public void AddWorker(int number)
    {
        if (workers + number <= maxWorkers)
        {
            ResourceManager.Instance.ConsumePop(number);
            workers += number;
        }
    }

    public void RemoveWorker(int number)
    {
        if (workers - number >= 0)
        {
            ResourceManager.Instance.AddRessource(ResourceManager.RessourceType.Population, number);
            workers -= number;
        }
    }

    public int GetWorker()
    {
        return workers;
    }

    public int GetMaxWorker()
    {
        return maxWorkers;
    }

    public void SetMaxWorker(int workers)
    {
        maxWorkers = workers;
    }

    public StopWatch GetTimer()
    {
        return gatheringTimer;
    }

    public bool IsThereNoWorker()
    {
        if (workers == 0)
        {
            gatheringTimer.StopCount();
            TargetedNode = null;
            return true;
        }
        return false;

    }

    public void SetBuildingScript(BuildingsScriptableObject newScript)
    {
        building = (GatheringBuildingScript)newScript;
    }
}
