using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceQuest : Quest
{
    private ResourceManager.RessourceType ressourceType;
    public ResourceManager.RessourceType RessourceType {get {return ressourceType;}}

    public ResourceQuest(int objective, ResourceManager.RessourceType type) : base(objective){
        this.description = "Obtenir une population de " + objective.ToString() + " habitants.";
        this.ressourceType = type;
    }


}
