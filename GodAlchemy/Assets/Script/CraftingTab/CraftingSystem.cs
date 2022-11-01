using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private ItemSlot firstElement;
    [SerializeField] private ItemSlot secondElement;
    [SerializeField] private ItemSlot resultSlot;
    [SerializeField] public ElementScriptableObject[] recipeItemBook;

    //Crafting slot
    [SerializeField] private InventorySystem playerInventory;
    [SerializeField] private RessourceManager ressourceManager;

    // Start is called before the first frame update
    private void Start()
    {
        firstElement = transform.Find("ItemSlot1").transform.gameObject.GetComponent<ItemSlot>();
        secondElement = transform.Find("ItemSlot2").transform.gameObject.GetComponent<ItemSlot>();
        resultSlot = transform.Find("ResultSlot").transform.gameObject.GetComponent<ItemSlot>();
        playerInventory = FindObjectOfType<InventorySystem>();
        ressourceManager = FindObjectOfType<RessourceManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if ((firstElement.element != null) & (secondElement.element != null))
            checkRecipe();
        else
            resultSlot.suppItem(1);
    }

    public void checkRecipe()
    {
        if ((firstElement.element != null) && (secondElement.element != null))
        {
            if(resultSlot.element == null)
            {
                foreach (ElementScriptableObject combinedElement in recipeItemBook)
                {
                    if ((combinedElement.elementToCraft1.name == firstElement.element.name) & (combinedElement.elementToCraft2.name == secondElement.element.name))
                    {
                        resultSlot.addItem(combinedElement, 1);
                        return;
                    }
                    else if ((combinedElement.elementToCraft2.name == firstElement.element.name) & (combinedElement.elementToCraft1.name == secondElement.element.name))
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
        }
        else
        {
            resultSlot.element = null;
        }
        
    }

    public void craftDoneClick()
    {
        int _amount = (firstElement.getSlotCost() + secondElement.getSlotCost() + resultSlot.getSlotCost());
        if (ressourceManager.HasEnoughPower(_amount))
        {
            if (playerInventory.AddItem(resultSlot.element))
            {
                Debug.Log("Added Item");
                consumeElement(_amount);
            }
        }
        
    }

    public void craftDoneDrop(ItemSlot finalSlot)
    {
        int _amount = (firstElement.getSlotCost() + secondElement.getSlotCost() + resultSlot.getSlotCost());
        if (ressourceManager.HasEnoughPower(_amount))
        {
            if (finalSlot.addItem(resultSlot.element,1))
            {
                finalSlot.elementIsPayed = true;
                consumeElement(_amount);
            }
        }
    }

    public void addRessource(ElementScriptableObject addedRessource)
    {
        if (firstElement.element == null)
        {
            firstElement.addItem(addedRessource, 1);
        }
        else if (secondElement.element == null)
        {
            secondElement.addItem(addedRessource, 1);
        }
    }

    public void consumeElement(int divinePower)
    {
        consumeDivinePower(divinePower);
        firstElement.suppItem(1);
        secondElement.suppItem(1);
        resultSlot.suppItem(1);
    }

    public void consumeDivinePower(int amount)
    {
        ressourceManager.SubstractDivinePower(amount);
    }


}
