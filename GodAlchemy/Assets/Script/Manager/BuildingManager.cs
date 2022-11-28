using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : BaseManager<BuildingManager>
{
    protected override void InitManager()
    {
        buildingsScriptableObjects = new List<BuildingsScriptableObject>(Resources.LoadAll<BuildingsScriptableObject>("/Buildings"));
        buildingsScriptableObjects.Sort();
    }

    /// <summary>
    /// Ces fonctions servent à convertir un buildingScriptableObject en int pour être envoyé sur le réseau comme référence
    /// </summary>
    [SerializeField]
    private List<BuildingsScriptableObject> buildingsScriptableObjects;
    private int GetBuildingIndex(BuildingsScriptableObject buildingsScriptableObject)
    {
        if (buildingsScriptableObject == null) return -1;
        return buildingsScriptableObjects.IndexOf(buildingsScriptableObject);
    }
    private BuildingsScriptableObject GetBuildingFromIndex(int buildingType)
    {
        if (buildingType == -1) return null;
        return buildingsScriptableObjects[buildingType];
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void SpawnObjectRPC(NetworkPrefabRef prefabRef, Vector3 position, int scriptableObjectIndex)
    {
        NetworkObject obj = GameManager.Instance.Runner.Spawn(prefabRef, position);
        obj.GetComponent<BuildingGeneric>().building = GetBuildingFromIndex(scriptableObjectIndex);
    }

    [SerializeField]
    private NetworkPrefabRef genericBuildingPrefab;
    public void BuildBuilding(BuildingsScriptableObject scriptableObject)
    {
        Instance.SpawnObjectRPC(genericBuildingPrefab, GameManager.GridManager.GetMouseGridPos(), GetBuildingIndex(scriptableObject));
    }

}