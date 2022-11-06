using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // Permet d'appeler certaines fonction dans le mode d'édition pour actualiser les slots
public partial class ItemSlot : MonoBehaviour
{
    public enum Type
    {
        itemSlot,
        craftSlot,
        recipeSlot,
        ressourceSlot,
        recipeBookSlot,
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

    //RecipeSystem
    private RecipeSystem recipeSystem;
    private bool isRecipeUnlock;
    [SerializeField] 
    private Sprite lockedSprite;

    //ToolTip
    private ToolTipTrigger toolTipTrigger;

    private void Start()
    {
        uiCase = transform.Find("Case").transform.gameObject;
        itemIcon = uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        costText = uiCase.transform.Find("ItemAmount").transform.gameObject.GetComponent<TextMeshProUGUI>();
        craftSystem = FindObjectOfType<CraftingSystem>();
        playerInventory = FindObjectOfType<InventorySystem>();
        recipeSystem = FindObjectOfType<RecipeSystem>();

        // Adapte le isPayed selon l'élément placé dans l'inspecteur
        elementIsPayed = Element != null;
        // Définitions nécessaires pour le DragAndDrop
        currentSlot = transform.GetComponent<ItemSlot>();
        canvas = FindObjectOfType<Canvas>();

        //Variable pour recipeBookSlot
        isRecipeUnlock = false;

        //Tooltip
        toolTipTrigger = gameObject.GetComponent<ToolTipTrigger>();
        UpdateUi();
    }

    /// <summary>
    /// Fonction qui permet de mettre à jour le slot avec tout le visuel. Cette fonction peut être utilisé lors de changements (supp, add...)
    /// Ne pas utiliser la fonction dans un update de unity !!
    /// </summary>
    private void UpdateUi()
    {
        if (Element != null)
        {
            if((SlotType == Type.recipeBookSlot) && isRecipeUnlock == false)
            {
                itemIcon.sprite = lockedSprite;
                costText.text = "";
                toolTipTrigger.SetHeader(null);
                toolTipTrigger.SetContent(null);
            }
            else
            {
                toolTipTrigger.SetHeader(Element.name);
                toolTipTrigger.SetContent(Element.ElementDescription);
                itemIcon.sprite = Element.Sprite;
                costText.text = Element.GetCost().ToString();
            }
            
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

    public void UnlockRecipe()
    {
        isRecipeUnlock = true;
        UpdateUi();
    }

    public bool IsRecipeUnlock()
    {
        if(SlotType == Type.recipeBookSlot)
        {
            return isRecipeUnlock;
        }
        return true;
    }

    /// <summary>
    /// Ajoute un item dans la case.
    /// </summary>
    /// <param name="addedElement"> Element à ajouter. </param>
    /// <param name="payItem"> Si l'élément ajouté vient d'être payé ou l'est déjà. </param>
    /// <returns> True si l'item a été ajouté. </returns>
    public bool AddItem(ElementScriptableObject addedElement, bool payItem = false)
    {
        if (!IsEmpty()) return false;

        elementIsPayed |= payItem;
        Element = addedElement;
        UpdateUi();
        return true;
    }

    /// <summary>
    /// Vide le slot.
    /// </summary>
    public void Empty()
    {
        Element = null;
        elementIsPayed = false;
        UpdateUi();
    }

    public bool IsElementPayed()
    {
        return elementIsPayed;
    }

    /// <summary>
    /// Retourne le coût de l'élément s'il n'a pas encore été payé. Ou 0 s'il a déjà été payé.
    /// </summary>
    public int GetSlotCost()
    {
        return elementIsPayed ? 0 :  Element.GetCost();
    }
}
