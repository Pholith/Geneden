using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private ItemSlot firstElement;
    [SerializeField] private ItemSlot secondElement;
    [SerializeField] private ItemSlot resultSlot;
    [SerializeField] public ElementScriptableObject[] recipeItemBook;

    //Crafting slot
    [SerializeField] private InventorySystem playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        firstElement = transform.Find("ItemSlot1").transform.gameObject.GetComponent<ItemSlot>();
        secondElement = transform.Find("ItemSlot2").transform.gameObject.GetComponent<ItemSlot>();
        resultSlot = transform.Find("ResultSlot").transform.gameObject.GetComponent<ItemSlot>();
        playerInventory = FindObjectOfType<InventorySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((firstElement.element != null) & (secondElement.element != null))
            checkRecipe();
        else
            resultSlot.suppItem(1);
    }

    public void checkRecipe()
    {

        foreach(ElementScriptableObject combinedElement in recipeItemBook)
        {
            if ((combinedElement.elementToCraft1.name == firstElement.element.name) & (combinedElement.elementToCraft2.name == secondElement.element.name))
            {
                resultSlot.addItem(combinedElement, 1);
                return;
            }
            else if((combinedElement.elementToCraft2.name == firstElement.element.name) & (combinedElement.elementToCraft1.name == secondElement.element.name))
            {
                resultSlot.addItem(combinedElement, 1);
                return;
            }
            else
            {
                resultSlot.element = null;
            }

        }
    }

    public void craftDone()
    {
        if(playerInventory.AddItem(resultSlot.element))
        {
            consumeElement();
        }
    }

    public void addRessource(ElementScriptableObject addedRessource)
    {
        if(firstElement.element == null)
        {
            firstElement.addItem(addedRessource, 1);
        }
        else if(secondElement.element == null)
        {
            secondElement.addItem(addedRessource, 1);
        }
    }

    public void consumeElement()
    {
        firstElement.suppItem(1);
        secondElement.suppItem(1);
        resultSlot.suppItem(1);
    }


}
