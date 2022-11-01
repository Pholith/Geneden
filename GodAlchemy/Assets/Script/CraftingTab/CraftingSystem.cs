using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private ItemSlot firstElementSlot;
    [SerializeField] private ItemSlot secondElementSlot;
    [SerializeField] private ItemSlot resultSlot;
    [SerializeField] public ElementScriptableObject[] recipeItemBook;

    //Crafting slot
    [SerializeField] private InventorySystem playerInventory;
    [SerializeField] private RessourceManager ressourceManager;

    private void Start()
    {
        firstElementSlot = transform.Find("ItemSlot1").transform.gameObject.GetComponent<ItemSlot>();
        secondElementSlot = transform.Find("ItemSlot2").transform.gameObject.GetComponent<ItemSlot>();
        resultSlot = transform.Find("ResultSlot").transform.gameObject.GetComponent<ItemSlot>();
        playerInventory = FindObjectOfType<InventorySystem>();
        ressourceManager = FindObjectOfType<RessourceManager>();
    }

    private void Update()
    {
        if (!firstElementSlot.IsEmpty() && !secondElementSlot.IsEmpty())
            CheckRecipe();
        else
            resultSlot.SuppItem();
    }

    public void CheckRecipe()
    {
        if (!firstElementSlot.IsEmpty() && !secondElementSlot.IsEmpty())
        {
            if (resultSlot.IsEmpty())
            {
                foreach (ElementScriptableObject combinedElement in recipeItemBook)
                {
                    if (combinedElement.ElementToCraft1.name == firstElementSlot.Element.name && combinedElement.ElementToCraft2.name == secondElementSlot.Element.name)
                    {
                        resultSlot.AddItem(combinedElement);
                        return;
                    }
                    else if (combinedElement.ElementToCraft2.name == firstElementSlot.Element.name && combinedElement.ElementToCraft1.name == secondElementSlot.Element.name)
                    {
                        resultSlot.AddItem(combinedElement);
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


    // Ces 2 méthodes c'est le même code à 90% ?! Faut factoriser
    public void CraftDoneClick()
    {
        int _amount = firstElementSlot.GetSlotCost() + secondElementSlot.GetSlotCost() + resultSlot.GetSlotCost();
        if (ressourceManager.HasEnoughPower(_amount))
        {
            if (playerInventory.AddItem(resultSlot.Element))
            {
                ConsumeElement(_amount);
            }
        }

    }

    public void CraftDoneDrop(ItemSlot finalSlot)
    {
        int _amount = firstElementSlot.GetSlotCost() + secondElementSlot.GetSlotCost() + resultSlot.GetSlotCost();
        if (ressourceManager.HasEnoughPower(_amount))
        {
            if (finalSlot.AddItem(resultSlot.Element))
            {
                ConsumeElement(_amount);
            }
        }
    }

    public void AddRessource(ElementScriptableObject addedRessource)
    {
        if (firstElementSlot.IsEmpty()) firstElementSlot.AddItem(addedRessource);
        else if (secondElementSlot.IsEmpty()) secondElementSlot.AddItem(addedRessource);
    }

    public void ConsumeElement(int divinePower)
    {
        ConsumeDivinePower(divinePower);
        firstElementSlot.SuppItem();
        secondElementSlot.SuppItem();
        resultSlot.SuppItem();
    }

    public void ConsumeDivinePower(int amount)
    {
        ressourceManager.SubstractDivinePower(amount);
    }


}
