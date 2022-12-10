using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New Upgrade", order = 6)]
public class UpgradesScriptableObject : ScriptableObject, IComparable<UpgradesScriptableObject>
{
    [Serializable]
    public enum UpgradeType
    {
        Efficiency,
        Value,
        Technologie,
        Divine,
        UnlockRessource,
        LevelUpSpell,
        LevelUpBuilding
    }

    [Header("Informations basiques")]
    [TextArea(0, 8)]
    public string UpgradeDescription;

    [SerializeField]
    public Sprite Sprite;

    [Min(0)]
    [Tooltip("Temps en seconde de recherche de l'upgrade.")]
    public int ResearchTime;

    [Header("Coûts")]
    [Min(0)]
    public int FoodCost;
    [Min(0)]
    public int WoodCost;
    [Min(0)]
    public int IronCost;
    [Min(0)]
    public int RockCost;
    [Min(0)]
    public int SilverCost;
    [Min(0)]
    public int GoldCost;

    [Header("Bonus")]
    public float Bonus;
    public List<ResourceManager.RessourceType> AddedGatherableRessource;
    [Header("LevelingUp")]
    public BuildingsScriptableObject LevelUpBuilding;
    public SpellScriptableObject LevelUpSpell;

    [Header("Etat")]
    public bool Unlocked;

    [Header("Gameplay")]
    [Range(1, 4)]
    public int Tier = 1;

    [Header("Type de Technologie")]
    public List<UpgradeType> type;

    [SerializeField]
    public UpgradesScriptableObject UpgradeInto;

    [SerializeField]
    public List<BuildingsScriptableObject> UpgradeApplication;

    [Header("Conditions")]
    [Range(0, 50)]
    public int RequiredCivilisationLvl;

    public int CompareTo(UpgradesScriptableObject other)
    {
        return name.CompareTo(other.name);
    }

    public bool IsBuildingLevelUpUpgrade()
    {
        return LevelUpBuilding != null;
    }

}