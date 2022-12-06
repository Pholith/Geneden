using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;

public class GatheringBuildings : MonoBehaviour
{
    private GatheringBuildingScript building;
    private List<GameObject> nodesInRangeList;
    [SerializeField]
    private ResourceNode TargetedNode;
    private ResourceManager resourceManager;
    private GridManager gridManager;
    private BuildingGeneric baseBuilding;
    private float secondToWaitBeforeGather;

    [SerializeField]
    private List<Collider2D> collidersInRangeList;
    private GameObject buildingRange;
    private BuildingManager buildingManager;
    // Start is called before the first frame update
    private void Start()
    {
        building = (GatheringBuildingScript)GetComponent<BuildingGeneric>().GetBuilding();
        nodesInRangeList = new List<GameObject>();
        collidersInRangeList = new List<Collider2D>();
        resourceManager = FindObjectOfType<ResourceManager>();
        gridManager = FindObjectOfType<GridManager>();
        buildingManager = FindObjectOfType<BuildingManager>();
        baseBuilding = GetComponent<BuildingGeneric>();
        CreateBuildingRange();
        TargetedNode = null;
        secondToWaitBeforeGather = 10.0f;
        StartCoroutine(GatherRoutine());
    }

    // Update is called once per frame
    private void Update()
    {
        GetNodes();
        BuildingIsSelected();
    }

    private void CreateBuildingRange()
    {
        buildingRange = Instantiate(buildingManager.buildingRangePrefab);
        buildingRange.transform.SetParent(transform);
        buildingRange.transform.localPosition = new Vector3(1f, 1f, 0);
        buildingRange.transform.localScale = new Vector3( building.gatheringRange * gridManager.MainGameGrid.cellSize.x, building.gatheringRange * gridManager.MainGameGrid.cellSize.y, 1);
        buildingRange.SetActive(false);
    }

    private void BuildingIsSelected()
    {
        if (baseBuilding.isSelectedBuilding())
            buildingRange.SetActive(true);
        else
            buildingRange.SetActive(false);

    }

    public void GetNodes()
    {
        if (TargetedNode == null)
        {

            collidersInRangeList = gridManager.GetResourcesInRange(gridManager.GetGridPos(transform.position + new Vector3(0.5f,0.5f,0f)), Mathf.RoundToInt((building.gatheringRange /2) * gridManager.MainGameGrid.cellSize.x));
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
                if(collider.gameObject.GetComponent<ResourceNode>() != null)
                {
                    if (building.gatherableRessource.Contains(collider.gameObject.GetComponent<ResourceNode>().GetResourceType()))
                    {
                        TargetedNode = collider.gameObject.GetComponent<ResourceNode>();
                        return;
                    }
                }      
            }
        }
        TargetedNode = null;
    }

    private IEnumerator GatherRoutine()
    {
        while (true)
        {
            //RessourceType randomRessource = (RessourceType)Random.Range(0, 7);
            //AddRessource(randomRessource, 100);
            //AddRessource(RessourceType.CivLevel, 5);
            GatherNode();
            yield return new WaitForSeconds(secondToWaitBeforeGather);
        }

    }

    public void GatherNode()
    {
        if (TargetedNode != null)
        {
            if (TargetedNode.DeleteResource(10))
            {
                resourceManager.AddRessource(TargetedNode.GetResourceType(), 10);
                secondToWaitBeforeGather = 10.0f;
            }
            else
            {
                TargetedNode = null;
                secondToWaitBeforeGather = 1.0f;
            }
        }
    }
}
