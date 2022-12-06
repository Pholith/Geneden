using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gathering", menuName = "ScriptableObjects/New gathering building", order = 4)]
public class GatheringBuildingScript : BuildingsScriptableObject
{
    [Header("Resources")]
    [SerializeField]
    public List<ResourceManager.RessourceType> gatherableRessource;
    [Range(1, 25)]
    public int gatheringRange = 10;

    [Header("Villagers")]
    [Range(1, 5)]
    public int maxVillager = 3;
}
