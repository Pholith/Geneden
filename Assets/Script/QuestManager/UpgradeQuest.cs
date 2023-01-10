using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeQuest : Quest
{
    private string upgradeName;
    public string UpgradeName {get {return upgradeName;}}

    public UpgradeQuest(int objective, string name) : base(objective){
        this.description = "Débloquer l'amélioration " + name;
        upgradeName = name;
    }
}
