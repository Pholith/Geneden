using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildingsScriptableObject;
using static ResourceManager;
using static GameTimer;
using Unity.VisualScripting;

public class BuildingGeneric : MonoBehaviour
{
    public BuildingsScriptableObject building; //scriptableobject script
    private bool isBuild;
    private ResourceManager resourceManager;
    private SpriteRenderer sr;
    public Sprite spriteTimeBuilding;

    void Start()
    {
        isBuild = false;
        sr = GetComponent<SpriteRenderer>();
        // Test Condition sp�cial de construction
        if (building.SpecialConditionRequired.Invoke())
        {

            // Test Condition Lvl Civilisation
            resourceManager = ResourceManager.Instance;
            if (resourceManager.GetCivLevel() >= this.building.RequiredCivilisationLvl)
            {
                // buildingtime
                sr.sprite = spriteTimeBuilding; // TODO Sprite construction;
                new GameTimer(10, () => Build());
            }
            else
            {
                Debug.Log("Vous n'avez pas atteint le niveau de civilisation nec�ssaire pour construire ce bat�ment.");
            }
        }
        else
        {
            Debug.Log("Vous ne pouvez pas encore construire ce bat�ment.");
        }
    }

    public BuildingsScriptableObject GetBuilding()
    {
        return building;
    }

    public bool IsBuild()
    {
        return isBuild;
    }

    private void Build()
    {
        sr.sprite = building.Sprite;
        isBuild = true;

        switch(building.GetType().ToString())
        {
            case "GatheringBuildingScript":
                this.AddComponent<GatheringBuildings>();
                break;
        }
    }
}
