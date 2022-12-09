using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceQuest : Quest
{
    private ResourceManager.RessourceType ressourceType;
    public ResourceManager.RessourceType RessourceType {get {return ressourceType;}}

    public ResourceQuest(int objective, ResourceManager.RessourceType type) : base(objective){
        switch (type) {
            case (ResourceManager.RessourceType.MaxPopulation):
                this.description = "Obtenir une population de " + objective.ToString() + " habitants.";
                break;
            case (ResourceManager.RessourceType.Food):
                this.description = "Avoir plus de " + objective.ToString() + " de nourriture.";
                break;
            case (ResourceManager.RessourceType.Wood):
                this.description = "Avoir plus de " + objective.ToString() + " de bois.";
                break;
            case (ResourceManager.RessourceType.Stone):
                this.description = "Avoir plus de " + objective.ToString() + " de pierre.";
                break;
            case (ResourceManager.RessourceType.Iron):
                this.description = "Avoir plus de " + objective.ToString() + " de fer.";
                break;
            case (ResourceManager.RessourceType.Silver):
                this.description = "Avoir plus de " + objective.ToString() + " d'argent.";
                break;
            case (ResourceManager.RessourceType.Gold):
                this.description = "Avoir plus de " + objective.ToString() + " d'or.";
                break;
            default:
                this.description = "N/A";
                break;
        }
       
        this.ressourceType = type;
    }


}
