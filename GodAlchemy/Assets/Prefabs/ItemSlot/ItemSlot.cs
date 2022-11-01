using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class ItemSlot : MonoBehaviour
{
    public enum Type
    {
        itemSlot,
        craftSlot,
        recipeSlot,
        ressourceSlot
    }

    [SerializeField]
    public ElementScriptableObject Element;

    [SerializeField]
    public Type SlotType;
    private GameObject uiCase;

    private Image itemIcon;
    private TextMeshProUGUI costText;
    private bool elementIsPayed = false;

    //Crafting Slot Type
    private CraftingSystem craftSystem;
    private InventorySystem playerInventory;
    private readonly RessourceManager ressourceManager;

    private void Start()
    {
        uiCase = transform.Find("Case").transform.gameObject;
        itemIcon = uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        costText = uiCase.transform.Find("ItemAmount").transform.gameObject.GetComponent<TextMeshProUGUI>();
        craftSystem = FindObjectOfType<CraftingSystem>();
        playerInventory = FindObjectOfType<InventorySystem>();
        InitSlot();

        // Définitions nécessaires pour le DragAndDrop
        currentSlot = transform.GetComponent<ItemSlot>();
        canvas = FindObjectOfType<Canvas>();

    }


    private void InitSlot()
    {
        if (Element != null)
        {
            itemIcon.sprite = Element.Sprite;
            costText.text = (Element.DifficultyToCraft * 10).ToString();
        }
        else
        {
            itemIcon.sprite = null;
            costText.text = "";
        }
    }

    /// <summary>
    /// Test si le slot est vide.
    /// </summary>
    public bool IsEmpty()
    {
        return Element == null;
    }

    /// <summary>
    /// Vide le slot.
    /// </summary>
    public void Empty()
    {
        Element = null;
    }

    public bool AddItem(ElementScriptableObject addedElement)
    {
        if (!IsEmpty()) return false;

        elementIsPayed = true;
        Element = addedElement;
        return true;
    }

    public void SuppItem()
    {
        costText.text = "x";
        Element = null;
        itemIcon.sprite = null;
        elementIsPayed = false;
        costText.text = "";
    }
    /// <summary>
    /// Quand on clic sur un slot sans drag and drop (pour aller plus vite parfois).
    /// </summary>
    public void OnClick()
    {
        switch (SlotType)
        {
            case Type.itemSlot:
                break;
            case Type.craftSlot:
                craftSystem.CraftDoneClick();
                break;
            case Type.recipeSlot:
                if (playerInventory.AddItem(Element))
                    SuppItem();
                break;
            case Type.ressourceSlot:
                craftSystem.AddRessource(Element);
                break;
            default:
                break;
        }
    }

    public Image GetItemIcon()
    {
        return itemIcon;
    }

    public int GetSlotCost()
    {
        return Element.GetElementPrice();
    }
}
