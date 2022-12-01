using UnityEngine;

public class RecipeSystem : MonoBehaviour
{
    // Start is called before the first frame update

    private ItemSlot[] recipeList;
    private CraftingSystem craftSystem;
    [SerializeField]
    private InventorySystem playerInventory;

    private void Start()
    {
        recipeList = transform.GetComponentsInChildren<ItemSlot>();
        craftSystem = FindObjectOfType<CraftingSystem>();
        playerInventory = FindObjectOfType<InventorySystem>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void UnlockRecipe(ElementScriptableObject element)
    {
        foreach (ItemSlot slot in recipeList)
        {
            if (element == slot.Element)
            {
                slot.UnlockRecipe();

            }
        }
    }

    public void AddRecipeToCraft(ElementScriptableObject element)
    {
        ElementScriptableObject[] elementForCraft = { element.ElementToCraft1, element.ElementToCraft2 };
        foreach (ElementScriptableObject craftElement in elementForCraft)
        {
            if (!CheckPlayerElement(craftElement))
            {
                if (craftElement.IsPrimaryElement)
                {
                    craftSystem.AddRessource(craftElement);
                }
            }
            else
            {
                AddPlayerElement(craftElement);
            }
        }
    }

    private bool CheckPlayerElement(ElementScriptableObject element)
    {
        ItemSlot _playerElement = playerInventory.SearchElement(element);
        if (_playerElement != null)
        {
            return true;
        }
        return false;
    }

    private void AddPlayerElement(ElementScriptableObject element)
    {
        if (element.IsPrimaryElement)
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
