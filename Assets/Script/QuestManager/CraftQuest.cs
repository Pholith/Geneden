using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftQuest : Quest
{
    private string objectiveElement;
    public string ObjectiveElement {get => objectiveElement;}
    public CraftQuest(int objective, string elementName) : base(objective) {
        this.objectiveElement = elementName;
        this.description = "Découvrir l'élément " + elementName;
        //TODO: check RecipeSystem.AddRecipeToCraft
    }
        
}
