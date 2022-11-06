using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // Permet d'appeler certaines fonction dans le mode d'�dition pour actualiser les slots
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

    private void Start()
    {
        uiCase = transform.Find("Case").transform.gameObject;
        itemIcon = uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        costText = uiCase.transform.Find("ItemAmount").transform.gameObject.GetComponent<TextMeshProUGUI>();
        craftSystem = FindObjectOfType<CraftingSystem>();
        playerInventory = FindObjectOfType<InventorySystem>();

        // Adapte le isPayed selon l'�l�ment plac� dans l'inspecteur
        elementIsPayed = Element != null;
        // D�finitions n�cessaires pour le DragAndDrop
        currentSlot = transform.GetComponent<ItemSlot>();
        canvas = FindObjectOfType<Canvas>();

        UpdateUi();
    }

    /// <summary>
    /// Fonction qui permet de mettre � jour le slot avec tout le visuel. Cette fonction peut �tre utilis� lors de changements (supp, add...)
    /// Ne pas utiliser la fonction dans un update de unity !!
    /// </summary>
    private void UpdateUi()
    {
        if (Element != null)
        {
            itemIcon.sprite = Element.Sprite;
            costText.text = Element.GetCost().ToString();
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
    /// Ajoute un item dans la case.
    /// </summary>
    /// <param name="addedElement"> Element � ajouter. </param>
    /// <param name="payItem"> Si l'�l�ment ajout� vient d'�tre pay� ou l'est d�j�. </param>
    /// <returns> True si l'item a �t� ajout�. </returns>
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

    /// <summary>
    /// Retourne le co�t de l'�l�ment s'il n'a pas encore �t� pay�. Ou 0 s'il a d�j� �t� pay�.
    /// </summary>
    public int GetSlotCost()
    {
        return elementIsPayed ? 0 :  Element.GetCost();
    }
}
