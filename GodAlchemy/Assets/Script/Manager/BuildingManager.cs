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
    private void SpawnObjectRPC(NetworkPrefabRef prefabRef, Vector3 position)
    {
        GameManager.Instance.Runner.Spawn(prefabRef, position);
    }

    [SerializeField]
    private NetworkPrefabRef houseBuildingPrefab;
    public void buildHouse()
    {
        Instance.SpawnObjectRPC(houseBuildingPrefab, GameManager.GridManager.GetMouseGridPos());    
    }

}