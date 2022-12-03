using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GatheringBuildings : MonoBehaviour, IPointerClickHandler
{
    private GatheringBuildingScript building;
    private List<GameObject> nodesInRangeList;
    private Node TargetedNode;
    private ResourceManager resourceManager;
    private BuildingGeneric baseBuilding;
    private float secondToWaitBeforeGather;

    private GameObject buildingRange;
    // Start is called before the first frame update
    void Start()
    {
        building = (GatheringBuildingScript)GetComponent<BuildingGeneric>().GetBuilding();
        buildingRange = transform.Find("Range").gameObject;
        buildingRange.SetActive(false);
        nodesInRangeList = new List<GameObject>();
        resourceManager = FindObjectOfType<ResourceManager>();
        baseBuilding = GetComponent<BuildingGeneric>();
        TargetedNode = null;
        secondToWaitBeforeGather = 10.0f;
        StartCoroutine(GatherRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetedNode == null)
        {
            GetNodesListInRange();
        }
               
    }

    public void GetNodesListInRange()
    {
        nodesInRangeList = new List<GameObject>();
        Vector3 BuildingPos = GameManager.GridManager.GetGridPos(transform.position);
        var colliders = Physics.OverlapSphere(transform.position, building.gatheringRange);
        if(colliders.Any())
        {
            foreach (var collider in colliders)
            {
                if (building.gatherableRessource.Contains(collider.gameObject.GetComponent<Node>().GetResourceType()))
                {
                    nodesInRangeList.Add(collider.gameObject);
                } 
            }
            TargetedNode = nodesInRangeList[0].GetComponent<Node>();
            return;
        }
        TargetedNode = null;   
    }

    public bool NodeListIsEmpty()
    {
        return nodesInRangeList.Any();
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
        if((TargetedNode != null) && baseBuilding.IsBuild())
        {
            if(TargetedNode.DeleteResource(10))
            {
                resourceManager.AddRessource(TargetedNode.GetResourceType(), 10);
                secondToWaitBeforeGather = 10.0f;
            }
            else
            {
                secondToWaitBeforeGather = 0.1f;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
            buildingRange.SetActive(true);
    }
}
