using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementQuest : Quest
{
    private string objectiveElement;
    public string ObjectiveElement {get => objectiveElement;}
    
    public ElementQuest(int objective, string elementName) : base(objective){
        this.objectiveElement = elementName;
        this.description = "Lancer le sort " + elementName + " sur Aurus";
    }
}
