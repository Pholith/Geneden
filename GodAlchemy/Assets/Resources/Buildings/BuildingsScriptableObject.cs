using Fusion;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New building", order = 2)]
public class BuildingsScriptableObject : ScriptableObject, IComparable<BuildingsScriptableObject>
{
    [Serializable]
    public enum BuildingType
    {
        Economic,
        Production,
        Military,
        Defensive,
        Technologic,
        Divine,
    }

    [Header("Informations basiques")]
    [TextArea(0, 8)]
    public string BuildingDescription;

    [SerializeField]
    public Sprite Sprite;

    [Min(0)]
    public int MaxHealth = 500;

    [Min(0)]
    [Tooltip("Temps en seconde de construction du batiment.")]
    public int BuildingTime;

    [Header("Coûts")]
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


    [Header("Gameplay")]
    [Range(1, 4)]
    public int Tier = 1;

    [SerializeField]
    public BuildingsScriptableObject UpgradeInto;

    [SerializeField]
    public List<GameObject> UnlockUnities;

    [SerializeField]
    public List<BuildingType> BuildingTags;

    [Header("Conditions")]
    [Range(0, 50)]
    public int RequiredCivilisationLvl;

    [Serializable]
    public class BuildingSpecialCondition : SerializableCallback<bool> { }

    [SerializeField]
    [Tooltip("A utiliser si le batiment nécessite une condition spéciale (exemple: une cathédrale est construite)\n" +
            "Il faut assigner l'objet lui même dans ce champ puis sélectionner la fonction voulue, elle doit être écrite dans BuildingsScriptableObject.cs (voir exemple)")]
    public BuildingSpecialCondition SpecialConditionRequired = null;

    public bool NoCondition()
    {
        return true;
    }
    public bool ConditionExample1()
    {
        return true;
    }


    public int CompareTo(BuildingsScriptableObject other)
    {
        return name.CompareTo(other.name);
    }

    [Header("Script")]
    [SerializeField]
    public UnityEvent UpdateFunction;

}