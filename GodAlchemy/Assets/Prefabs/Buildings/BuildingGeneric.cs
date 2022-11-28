using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildingsScriptableObject;
using static ResourceManager;
using static GameTimer;
using Unity.VisualScripting;
using Fusion;

public class BuildingGeneric : NetworkBehaviour
{
    public BuildingsScriptableObject building; //scriptableobject script
    private bool isBuild;
    private ResourceManager resourceManager;
    private SpriteRenderer sr;
    public Sprite spriteTimeBuilding;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Test Condition sp�cial de construction
        if (building.SpecialConditionRequired.Invoke())
        {
            // Test Condition Lvl Civilisation
            resourceManager = ResourceManager.Instance;
            if (resourceManager.GetCivLevel() >= this.building.RequiredCivilisationLvl)
            {
                // Test Condition des ressources
                if (resourceManager.HasEnoughWood(building.WoodCost) &&
                    resourceManager.HasEnoughStone(building.RockCost) &&
                    resourceManager.HasEnoughIron(building.IronCost) &&
                    resourceManager.HasEnoughSilver(building.SilverCost) &&
                    resourceManager.HasEnoughGold(building.GoldCost))
                {
                    // Consume ressources
                    resourceManager.ConsumeWood(building.WoodCost);
                    resourceManager.ConsumeStone(building.RockCost);
                    resourceManager.ConsumeIron(building.IronCost);
                    resourceManager.ConsumeSilver(building.SilverCost);
                    resourceManager.ConsumeGold(building.GoldCost);

                    // buildingtime
                    sr.sprite = spriteTimeBuilding; // TODO Sprite construction;
                    new GameTimer(10, () => sr.sprite = this.building.Sprite);
                }
                else
                {
                    Destroy(gameObject);
                    Debug.Log("Vous n'avez pas assez de ressources pour construire ce bat�ment.");
                }
            } 
            else
            {
                Destroy(gameObject);
                Debug.Log("Vous n'avez pas atteint le niveau de civilisation nec�ssaire pour construire ce bat�ment.");
            }
        }
        else
        {
            Destroy(gameObject);
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

        switch (building.GetType().ToString())
        {
            case "GatheringBuildingScript":
                this.AddComponent<GatheringBuildings>();
                break;
        }
    }

}
