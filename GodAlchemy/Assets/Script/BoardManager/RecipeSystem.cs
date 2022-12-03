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
    [SerializeField]
    private GameObject recipeSlotPrefab;

    void Start()
    {
        
        CreateRecipeList();
        recipeList = transform.GetComponentsInChildren<ItemSlot>();
        craftSystem = FindObjectOfType<CraftingSystem>();
        playerInventory = FindObjectOfType<InventorySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CreateRecipeList()
    {   
        int _i = 0;
        int _xPos = -88;
        int _yPos = 355;
        
        ItemSlot _recipeSlotScript = recipeSlotPrefab.GetComponent<ItemSlot>();
        GameObject _contentPanel = transform.Find("ScrollArea").gameObject.transform.Find("Content").gameObject;
        foreach (ElementScriptableObject element in GameManager.ElementManager.Elements)
        {
            if(element.IsPrimaryElement)
            {
                continue;
            }
            Debug.Log(element);
            _recipeSlotScript.Element = element;
            GameObject _recipeSlot = Instantiate(recipeSlotPrefab);
            _recipeSlot.transform.SetParent(_contentPanel.transform);
            _recipeSlot.GetComponent<RectTransform>().anchoredPosition = new Vector3(_xPos, _yPos, 0f);
            _recipeSlot.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            _xPos += 35;
            _i += 1;
            if(_i%3 == 0)
            {
                _xPos = -88;
                _yPos -= 40;
                _i = 0;
            }
        }
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

    public void AddRecipeToCraft(ElementScriptableObject element)
    {
        ElementScriptableObject[] elementForCraft = { element.ElementToCraft1, element.ElementToCraft2 };
            foreach(ElementScriptableObject craftElement in elementForCraft)
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
