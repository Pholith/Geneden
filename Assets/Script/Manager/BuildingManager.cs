using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BuildingsScriptableObject;

public class BuildingManager : BaseManager<BuildingManager>
{
    [SerializeField]
    private QuestSystem questSystem;

    protected override void InitManager()
    {
        Resources.LoadAll("Buildings");
        buildingsScriptableObjects = new List<BuildingsScriptableObject>(Resources.FindObjectsOfTypeAll<BuildingsScriptableObject>());
        buildingsScriptableObjects.Sort();
        
        Resources.LoadAll("Tags");
        tagsSpriteList = new List<TagScriptableObject>(Resources.FindObjectsOfTypeAll<TagScriptableObject>());
    }

    public GameObject buildingRangePrefab;

    [SerializeField]
    public List<TagScriptableObject> tagsSpriteList;

    /// <summary>
    /// Ces fonctions servent � convertir un buildingScriptableObject en int pour �tre envoy� sur le r�seau comme r�f�rence
    /// </summary>
    [SerializeField]
    public List<BuildingsScriptableObject> buildingsScriptableObjects { get; private set; }
    private int GetBuildingIndex(BuildingsScriptableObject buildingsScriptableObject)
    {
        Debug.Log(buildingsScriptableObject);
        if (buildingsScriptableObject == null) return -1;
        return buildingsScriptableObjects.IndexOf(buildingsScriptableObject);
    }
    private BuildingsScriptableObject GetBuildingFromIndex(int buildingType)
    {
        if (buildingType == -1) return null;
        return buildingsScriptableObjects[buildingType];
    }

    public BuildingsScriptableObject GetBuildingByName(string name)
    {
        foreach(BuildingsScriptableObject building in buildingsScriptableObjects)
        {
            if(building.name == name)
            {
                return building;
            }
        }

        return null;
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void SpawnObjectRPC(NetworkPrefabRef prefabRef, Vector3 position, int scriptableObjectIndex)
    {
        NetworkObject obj = GameManager.Instance.Runner.Spawn(prefabRef, position);
        obj.GetComponent<BuildingGeneric>().buildingScriptObj = GetBuildingFromIndex(scriptableObjectIndex);
    }

    [SerializeField]
    private NetworkPrefabRef genericBuildingPrefab;
    public void BuildBuilding(BuildingsScriptableObject scriptableObject)
    {
        questSystem.Built(scriptableObject.name);
        Instance.SpawnObjectRPC(genericBuildingPrefab, GameManager.GridManager.GetMouseGridPos(), GetBuildingIndex(scriptableObject));
    }

}