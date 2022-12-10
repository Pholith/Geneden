using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New Spell", order = 7)]
public class SpellScriptableObject : ScriptableObject, IComparable<SpellScriptableObject>
{
    [Serializable]
    public enum SpellType
    {
        Ressource,
        Divine
    }

    [Header("Informations basiques")]
    [TextArea(0, 8)]
    public string SpellDescription;

    [SerializeField]
    public Sprite Sprite;

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

    [Header("Gameplay")]
    [Range(1, 4)]
    public int Tier = 1;

    [Header("Type de Technologie")]
    public List<SpellType> type;

    [SerializeField]
    public SpellScriptableObject UpgradeInto ;
    [SerializeField]
    public UpgradesScriptableObject NecessaryUpgrade;

    [Header("Conditions")]
    [Range(0, 50)]
    public int RequiredCivilisationLvl;

    public int CompareTo(SpellScriptableObject other)
    {
        return name.CompareTo(other.name);
    }

}