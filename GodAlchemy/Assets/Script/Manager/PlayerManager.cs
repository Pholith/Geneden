using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager>
{

    [SerializeField]
    private List<BuildingGeneric> ownedBuildingList;

    public List<UpgradesScriptableObject> Upgrades;
    public List<SpellScriptableObject> Spells;

    // Start is called before the first frame update

    protected override void InitManager()
    {
        ownedBuildingList = new List<BuildingGeneric>();
        Resources.LoadAll("Upgrades");
        Upgrades = new List<UpgradesScriptableObject>(Resources.FindObjectsOfTypeAll<UpgradesScriptableObject>());
        Upgrades.Sort();
        LockAllUpgrades();

        Resources.LoadAll("Spells");
        Spells = new List<SpellScriptableObject>(Resources.FindObjectsOfTypeAll<SpellScriptableObject>());
        Spells.Sort();
    }

    public void AddBuilding(BuildingGeneric building)
    {
        ownedBuildingList.Add(building);
    }

    private void LockAllUpgrades()
    {
        foreach(UpgradesScriptableObject upgrade in Upgrades)
        {
            upgrade.Unlocked = false;
        }
    }

    public void RemoveBuilding(BuildingGeneric building)
    {
        ownedBuildingList.Remove(building);
    }

    public bool IsPlayerBuilding(BuildingGeneric building)
    {
        return ownedBuildingList.Contains(building);
    }

    public UpgradesScriptableObject GetUpgradeByIndex(UpgradesScriptableObject upgrade)
    {
        return Upgrades[Upgrades.IndexOf(upgrade)];
    }

    public bool IsUpgradeUnlocked(UpgradesScriptableObject upgrade)
    {
        return GetUpgradeByIndex(upgrade).Unlocked;
    }

    public bool IsUpgradeUnlockedFromName(string upgradeName)
    {
        foreach(UpgradesScriptableObject upgrade in Upgrades)
        {
            if (upgrade.name == upgradeName) {
                return IsUpgradeUnlocked(upgrade);
            }
        }
        UnityEngine.Debug.Log("Error in PlayerManager.IsUpgradeUnlockedFromName, " + upgradeName + " does not exist.");
        return false;
    }

    public void UnlockUpgrade(UpgradesScriptableObject upgrade)
    {
        GetUpgradeByIndex(upgrade).Unlocked = true;
        UnityEngine.Debug.Log("upgraded " + upgrade.name);
        if(upgrade.type.Contains(UpgradesScriptableObject.UpgradeType.Divine))
        {
            if (upgrade.Bonus <= 1)
                ResourceManager.Instance.ReduceRefillDelayPercentage(upgrade.Bonus);
            else
                ResourceManager.Instance.AddMaxPower((Mathf.RoundToInt(upgrade.Bonus)));
        }
    }

    public UpgradesScriptableObject GetLastTierUpgrade(UpgradesScriptableObject upgrade)
    {
        if (IsUpgradeUnlocked(upgrade))
        {
            if(upgrade.UpgradeInto != null)
            {
                return GetLastTierUpgrade(upgrade.UpgradeInto);
            }
            else
            {
                return null;
            }
            
        }
        else
        {
            return upgrade;
        }

    }

    public SpellScriptableObject GetLastTierSpell(SpellScriptableObject spell)
    {
        if (spell.UpgradeInto != null)
        {
            if (IsUpgradeUnlocked(spell.UpgradeInto.NecessaryUpgrade))
            {
                return GetLastTierSpell(spell.UpgradeInto);
            }
        }

        return spell;
    }

    public float CheckEfficiencyUpgrades(BuildingsScriptableObject building,UpgradesScriptableObject.UpgradeType type)
    {
        float efficiencyBonus = 0;

        foreach(UpgradesScriptableObject upgrade in Upgrades)
        {
            if((upgrade.UpgradeApplication.Contains(building)) && (upgrade.type.Contains(type)) && IsUpgradeUnlocked(upgrade))
            {
                efficiencyBonus += upgrade.Bonus;
                Debug.Log(efficiencyBonus);
            }
        }

        return efficiencyBonus;
    }

    public List<ResourceManager.RessourceType> CheckGatherableRessources(GatheringBuildingScript building, UpgradesScriptableObject.UpgradeType type)
    {
        List<ResourceManager.RessourceType> gatherableRessources = building.gatherableRessource;

        foreach(UpgradesScriptableObject upgrade in Upgrades)
        {
            if ((upgrade.UpgradeApplication.Contains(building)) && (upgrade.type.Contains(type)) && IsUpgradeUnlocked(upgrade))
            {
                gatherableRessources = gatherableRessources.Concat(upgrade.AddedGatherableRessource).ToList();
            }
        }

        return gatherableRessources;

    }

    public void UpdateBuildingUpgrade()
    {
        foreach(BuildingGeneric building in ownedBuildingList)
        {
            building.UpdateBuildingUpgrades();
        }
    }

    public bool IsUpgradeSearchable(UpgradesScriptableObject upgrade)
    {
        Dictionary<ResourceManager.RessourceType, int> ressourcesForUpgrade = new Dictionary<ResourceManager.RessourceType, int>();
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Food, upgrade.FoodCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Wood, upgrade.WoodCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Stone, upgrade.RockCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Iron, upgrade.IronCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Silver, upgrade.SilverCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Gold, upgrade.GoldCost);

        if (!ResourceManager.Instance.HasEnoughRessource(ResourceManager.RessourceType.CivLevel, upgrade.RequiredCivilisationLvl))
            return false;

        foreach(ResourceManager.RessourceType type in ressourcesForUpgrade.Keys)
        {
            if (ResourceManager.Instance.HasEnoughRessource(type, ressourcesForUpgrade[type]))
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        foreach(BuildingGeneric building in ownedBuildingList)
        {
            if(building.IsSearchingUpgrade(upgrade))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsIndividualUpgradeSearchable(UpgradesScriptableObject upgrade)
    {
        Dictionary<ResourceManager.RessourceType, int> ressourcesForUpgrade = new Dictionary<ResourceManager.RessourceType, int>();
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Food, upgrade.FoodCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Wood, upgrade.WoodCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Stone, upgrade.RockCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Iron, upgrade.IronCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Silver, upgrade.SilverCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Gold, upgrade.GoldCost);

        if (!ResourceManager.Instance.HasEnoughRessource(ResourceManager.RessourceType.CivLevel, upgrade.RequiredCivilisationLvl))
            return false;

        foreach (ResourceManager.RessourceType type in ressourcesForUpgrade.Keys)
        {
            if (ResourceManager.Instance.HasEnoughRessource(type, ressourcesForUpgrade[type]))
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public bool IsSpellUsable(SpellScriptableObject spell)
    {
        Dictionary<ResourceManager.RessourceType, int> ressourceForSpell = new Dictionary<ResourceManager.RessourceType, int>();
        ressourceForSpell.Add(ResourceManager.RessourceType.Food, spell.FoodCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Wood, spell.WoodCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Stone, spell.RockCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Iron, spell.IronCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Silver, spell.SilverCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Gold, spell.GoldCost);

        if (!ResourceManager.Instance.HasEnoughRessource(ResourceManager.RessourceType.CivLevel, spell.RequiredCivilisationLvl))
            return false;

        if (spell.type.Contains(SpellScriptableObject.SpellType.Divine) && !ResourceManager.Instance.CanAddPower(Mathf.RoundToInt(spell.Bonus)))
            return false;

        foreach (ResourceManager.RessourceType type in ressourceForSpell.Keys)
        {
            if (ResourceManager.Instance.HasEnoughRessource(type, ressourceForSpell[type]))
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public bool IsUpgradeAlreadySearched(UpgradesScriptableObject upgrade)
    {
        foreach(BuildingGeneric building in ownedBuildingList)
        {
            if (building.IsSearchingUpgrade(upgrade))
                return true;
        }

        return false;
    }



    public void StartUpgrade(UpgradesScriptableObject upgrade)
    {
        Dictionary<ResourceManager.RessourceType, int> ressourcesForUpgrade = new Dictionary<ResourceManager.RessourceType, int>();
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Food, upgrade.FoodCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Wood, upgrade.WoodCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Stone, upgrade.RockCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Iron, upgrade.IronCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Silver, upgrade.SilverCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Gold, upgrade.GoldCost);

        foreach (ResourceManager.RessourceType type in ressourcesForUpgrade.Keys)
        {
            ResourceManager.Instance.AddRessource(type, -ressourcesForUpgrade[type]);
        }
    }

    public void StopUpgrade(UpgradesScriptableObject upgrade)
    {
        Dictionary<ResourceManager.RessourceType, int> ressourcesForUpgrade = new Dictionary<ResourceManager.RessourceType, int>();
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Food, upgrade.FoodCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Wood, upgrade.WoodCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Stone, upgrade.RockCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Iron, upgrade.IronCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Silver, upgrade.SilverCost);
        ressourcesForUpgrade.Add(ResourceManager.RessourceType.Gold, upgrade.GoldCost);

        foreach (ResourceManager.RessourceType type in ressourcesForUpgrade.Keys)
        {
            ResourceManager.Instance.AddRessource(type, ressourcesForUpgrade[type]);
        }
    }

    public void UseSpell(SpellScriptableObject spell)
    {
        Dictionary<ResourceManager.RessourceType, int> ressourceForSpell = new Dictionary<ResourceManager.RessourceType, int>();
        ressourceForSpell.Add(ResourceManager.RessourceType.Food, spell.FoodCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Wood, spell.WoodCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Stone, spell.RockCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Iron, spell.IronCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Silver, spell.SilverCost);
        ressourceForSpell.Add(ResourceManager.RessourceType.Gold, spell.GoldCost);

        if (spell.type.Contains(SpellScriptableObject.SpellType.Divine))
            ResourceManager.Instance.AddPower(Mathf.RoundToInt(spell.Bonus));

        foreach (ResourceManager.RessourceType type in ressourceForSpell.Keys)
        {
            ResourceManager.Instance.AddRessource(type, -ressourceForSpell[type]);
        }
    }


}
