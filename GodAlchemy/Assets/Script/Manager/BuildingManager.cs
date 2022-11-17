using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : BaseManager<BuildingManager>
{
    protected override void InitManager()
    {
    }

    [SerializeField]
    private GameObject houseBuilding;
    public void buildHouse()
    {
        GameObject house = Instantiate(houseBuilding);
        house.transform.position = GameManager.GridManager.GetMouseGridPos();
    }
}