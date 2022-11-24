using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : BaseManager<BuildingManager>
{
    protected override void InitManager()
    {
    }

    [SerializeField]
    private GameObject maison;
    public void buildHouse()
    {
        GameObject house = Instantiate(maison);
        house.transform.position = GameManager.GridManager.GetMouseGridPos();
    }

    [SerializeField]
    private GameObject maisonCommune;
    public void buildCommonHouse()
    {
        GameObject house = Instantiate(maisonCommune);
        house.transform.position = GameManager.GridManager.GetMouseGridPos();
    }
}