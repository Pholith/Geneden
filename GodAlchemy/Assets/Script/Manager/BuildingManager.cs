using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : BaseManager<BuildingManager>
{
    protected override void InitManager()
    {
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void SpawnObjectRPC(NetworkPrefabRef prefabRef, Vector3 position, BuildingsScriptableObject building)
    {
        var obj = GameManager.Instance.Runner.Spawn(prefabRef, position);
        obj.GetComponent<BuildingGeneric>().building = building;
    }

    [SerializeField]
    private NetworkPrefabRef maison;
    public void buildHouse(BuildingsScriptableObject building)
    {
        Instance.SpawnObjectRPC(maison, GameManager.GridManager.GetMouseGridPos(), building);
    }

}