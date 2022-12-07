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

    // Start is called before the first frame update

    protected override void InitManager()
    {
        ownedBuildingList = new List<BuildingGeneric>();
        Resources.LoadAll("Upgrades");
        Upgrades = new List<UpgradesScriptableObject>(Resources.FindObjectsOfTypeAll<UpgradesScriptableObject>());
        Upgrades.Sort();
        LockAllUpgrades();
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

    public void UnlockUpgrade(UpgradesScriptableObject upgrade)
    {
        GetUpgradeByIndex(upgrade).Unlocked = true;
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

    public float CheckEfficiencyUpgrades(BuildingsScriptableObject building,UpgradesScriptableObject.UpgradeType type)
    {
        float efficiencyBonus = 0;

        foreach(UpgradesScriptableObject upgrade in Upgrades)
        {
            Debug.Log(upgrade);
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


}
