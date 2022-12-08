using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildQuest : Quest
{
    private string objectiveBuild;
    public string ObjectiveBuild {get => objectiveBuild;}

    public BuildQuest(int objective, string buildName) : base(objective){
        this.description = "Construire le bâtiment " + buildName + ".";
        this.objectiveBuild = buildName;
    }

}
