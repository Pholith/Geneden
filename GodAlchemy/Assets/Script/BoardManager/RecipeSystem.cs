using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeSystem : MonoBehaviour
{
    // Start is called before the first frame update

    private ItemSlot[] recipeList;
    private CraftingSystem craftSystem;
    [SerializeField]
    private InventorySystem playerInventory;

    void Start()
    {
        recipeList = transform.GetComponentsInChildren<ItemSlot>();
        craftSystem = FindObjectOfType<CraftingSystem>();
        playerInventory = FindObjectOfType<InventorySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockRecipe(ElementScriptableObject element)
    {
        foreach(ItemSlot slot in recipeList)
        {
            if(element == slot.Element)
            {
                slot.UnlockRecipe();
          
            }
        }
    }

    private bool IsPrimaryElement(ElementScriptableObject element)
    {
        if (element.name == "Feu" || element.name == "Eau" || element.name == "Air" || element.name == "Terre")
            return true;
        return false;
    }

    public void AddRecipeToCraft(ElementScriptableObject element)
    {
        ElementScriptableObject[] elementForCraft = { element.ElementToCraft1, element.ElementToCraft2 };
        if(element.DifficultyToCraft > 2)
        {
            foreach(ElementScriptableObject craftElement in elementForCraft)
            {
                if (!CheckPlayerElement(craftElement))
                    return;
            }
            foreach(ElementScriptableObject craftElement in elementForCraft)
            {
                AddPlayerElement(craftElement);
            }
        }
        else
        {
            craftSystem.AddRessource(element.ElementToCraft1);
            craftSystem.AddRessource(element.ElementToCraft2);
        }    
    }

    private bool CheckPlayerElement(ElementScriptableObject element)
    {
        if (IsPrimaryElement(element))
        {
            return true;
        }
        else
        {
            ItemSlot _playerElement = playerInventory.SearchElement(element);
            if (_playerElement != null)
            {
                return true;
            }
        }
        return false;
    }

    private void AddPlayerElement(ElementScriptableObject element)
    {
        if (IsPrimaryElement(element))
        {
            craftSystem.AddRessource(element);
        }
        else
        {
            ItemSlot _playerElement = playerInventory.SearchElement(element);
            if (_playerElement != null)
            {
                craftSystem.AddItemInSlot(_playerElement.Element, _playerElement.IsElementPayed());
                _playerElement.Empty();
            }
        }
    }
}
