using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildingsScriptableObject;
using static ResourceManager;
using static GameTimer;

public class BuildingGeneric : MonoBehaviour
{
    public BuildingsScriptableObject building; //scriptableobject script
    
    private ResourceManager resourceManager;
    private SpriteRenderer sr;
    public Sprite spriteTimeBuilding;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Test Condition spécial de construction
        if (building.SpecialConditionRequired.Invoke())
        {

            // Test Condition Lvl Civilisation
            resourceManager = ResourceManager.Instance;
            if (resourceManager.GetCivLevel() >= this.building.RequiredCivilisationLvl)
            {
                // buildingtime
                sr.sprite = spriteTimeBuilding; // TODO Sprite construction;
                new GameTimer(10, () => sr.sprite = this.building.Sprite);
            }
            else
            {
                Debug.Log("Vous n'avez pas atteint le niveau de civilisation necéssaire pour construire ce batîment.");
            }
        }
        else
        {
            Debug.Log("Vous ne pouvez pas encore construire ce batîment.");
        }
    }

}
