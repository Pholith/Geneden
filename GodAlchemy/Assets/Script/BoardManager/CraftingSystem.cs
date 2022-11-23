using System;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private ItemSlot firstElementSlot;
    [SerializeField] private ItemSlot secondElementSlot;
    [SerializeField] private ItemSlot resultSlot;

    //Crafting slot
    [SerializeField] private InventorySystem playerInventory;
    [SerializeField] private RecipeSystem recipeSystem;
    [SerializeField] private Text resultSlotText;

    private void Start()
    {
        firstElementSlot = transform.Find("ItemSlot1").transform.gameObject.GetComponent<ItemSlot>();
        secondElementSlot = transform.Find("ItemSlot2").transform.gameObject.GetComponent<ItemSlot>();
        resultSlot = transform.Find("ResultSlot").transform.gameObject.GetComponent<ItemSlot>();
        playerInventory = FindObjectOfType<InventorySystem>();
        recipeSystem = FindObjectOfType<RecipeSystem>();
    }

    private void Update()
    {
        if (!firstElementSlot.IsEmpty() && !secondElementSlot.IsEmpty())
        {
            CheckRecipe();
            resultSlotText.text = "Fabrication : " + (ComputeCraftCost()).ToString();
        }
        else
        {
            resultSlot.Empty();
            resultSlotText.text = "";
        }
    }

    private void CheckRecipe()
    {
        if (!firstElementSlot.IsEmpty() && !secondElementSlot.IsEmpty())
        {
            if (resultSlot.IsEmpty())
            {
                foreach (ElementScriptableObject combinedElement in GameManager.ElementManager.Elements)
                {
                    if (combinedElement.ElementToCraft1 == null || combinedElement.ElementToCraft2 == null) continue;
                    if (combinedElement.ElementToCraft1.name == firstElementSlot.Element.name && combinedElement.ElementToCraft2.name == secondElementSlot.Element.name)
                    {
                        resultSlot.AddItem(combinedElement);
                        recipeSystem.UnlockRecipe(combinedElement);
                        return;
                    }
                    else if (combinedElement.ElementToCraft2.name == firstElementSlot.Element.name && combinedElement.ElementToCraft1.name == secondElementSlot.Element.name)
                    {
                        resultSlot.AddItem(combinedElement);
                        recipeSystem.UnlockRecipe(combinedElement);
                        return;
                    }
                    else
                    {
                        resultSlot.Empty();
                    }

                }
            }
        }
        else
        {
            resultSlot.Empty();
        }
    }

    public int ComputeCraftCost()
    {
        return firstElementSlot.GetSlotCost() + secondElementSlot.GetSlotCost() + resultSlot.GetSlotCost();
    }
    public bool HasEnoughPower()
    {
        return GameManager.ResourceManager.HasEnoughPower(ComputeCraftCost());
    }

    /// <summary>
    /// Diminue la barre de power du co�t du craft et vide les slots.
    /// </summary>
    /// <exception cref="InvalidOperationException"> si le craft coute plus cher que la barre de power n'est rempli </exception>
    public void ConsumePower()
    {
        if (!HasEnoughPower()) throw new InvalidOperationException();
        GameManager.ResourceManager.ConsumePower(ComputeCraftCost());
        firstElementSlot.Empty();
        secondElementSlot.Empty();
        resultSlot.Empty();
    }

    public void AddRessource(ElementScriptableObject addedRessource)
    {
        if (firstElementSlot.IsEmpty()) firstElementSlot.AddItem(addedRessource);
        else if (secondElementSlot.IsEmpty()) secondElementSlot.AddItem(addedRessource);
    }

    public void AddItemInSlot(ElementScriptableObject addedRessource, bool isPayed)
    {
        if (firstElementSlot.IsEmpty()) firstElementSlot.AddItem(addedRessource, isPayed);
        else if (secondElementSlot.IsEmpty()) secondElementSlot.AddItem(addedRessource, isPayed);
    }


}
