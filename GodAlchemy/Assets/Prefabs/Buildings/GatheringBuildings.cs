using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatheringBuildings : MonoBehaviour
{
    private GatheringBuildingScript building;
    private List<GameObject> nodesInRangeList;
    private ResourceNode TargetedNode;
    private ResourceManager resourceManager;
    private BuildingGeneric baseBuilding;
    private float secondToWaitBeforeGather;

    // Start is called before the first frame update
    private void Start()
    {
        building = (GatheringBuildingScript)GetComponent<BuildingGeneric>().GetBuilding();
        nodesInRangeList = new List<GameObject>();
        resourceManager = FindObjectOfType<ResourceManager>();
        baseBuilding = GetComponent<BuildingGeneric>();
        TargetedNode = null;
        secondToWaitBeforeGather = 10.0f;
        StartCoroutine(GatherRoutine());
    }

    // Update is called once per frame
    private void Update()
    {
        if (TargetedNode == null)
        {
            GetNodesListInRange();
        }

    }

    public void GetNodesListInRange()
    {
        nodesInRangeList = new List<GameObject>();
        Vector3 BuildingPos = GameManager.GridManager.GetGridPos(transform.position);
        Collider[] colliders = Physics.OverlapSphere(transform.position, building.gatheringRange);
        if (colliders.Any())
        {
            foreach (Collider collider in colliders)
            {
                nodesInRangeList.Add(collider.gameObject);
            }
            TargetedNode = nodesInRangeList[0].GetComponent<ResourceNode>();
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
        if ((TargetedNode != null) && baseBuilding.IsBuild())
        {
            if (TargetedNode.DeleteResource(10))
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
}
