using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Partie de la classe qui gère le drag and drop des item slot
/// </summary>
public partial class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
{

    [SerializeField]
    private Vector2 initialRectTransform;
    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private Transform snapParent;

    private Canvas canvas;
    private ItemSlot currentSlot;

    // Le Start est dans ItemSlot

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemIcon.sprite != null)
        {
            rectTransform = itemIcon.GetComponent<RectTransform>();
            initialRectTransform = rectTransform.anchoredPosition;
            snapParent = transform.parent;
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            itemIcon.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    /// <summary>
    /// Quand on clic sur un slot sans drag and drop (pour aller plus vite que le drag and drop).
    /// </summary>
    public void OnClick()
    {
        switch (SlotType)
        {
            case Type.itemSlot:
                break;
            case Type.craftSlot:
                break;
            case Type.recipeSlot:
                if (!IsEmpty() && craftSystem.HasEnoughPower() && playerInventory.AddItem(Element, true)) craftSystem.ConsumePower();
                break;
            case Type.ressourceSlot:
                craftSystem.AddRessource(Element);
                break;
            case Type.recipeBookSlot:
                if (IsRecipeUnlock())
                recipeSystem.AddRecipeToCraft(Element);
                break;
            default:
                break;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerCurrentRaycast.gameObject;
        GameObject origin = eventData.pointerDrag;
        ItemSlot originSlot = origin.GetComponent<ItemSlot>();
        ItemSlot finalSlot = dropped.GetComponentInParent<ItemSlot>();
        GetComponent<ItemSlot>().SetParentSnap(transform);

        if (finalSlot.SlotType == Type.craftSlot)
        {
            switch (originSlot.SlotType)
            {
                case Type.itemSlot:
                    if (finalSlot.AddItem(originSlot.Element, true)) originSlot.Empty();
                    break;
                case Type.recipeSlot:
                    if (craftSystem.HasEnoughPower() && finalSlot.AddItem(originSlot.Element, true))
                    {
                        craftSystem.ConsumePower();
                        originSlot.Empty();
                    }
                    break;
                case Type.ressourceSlot:
                    finalSlot.AddItem(originSlot.Element);
                    break;
                default:
                    break;
            }
        }
        if (finalSlot.SlotType == Type.itemSlot)
        {
            switch (originSlot.SlotType)
            {
                case Type.itemSlot:
                    if (finalSlot.AddItem(originSlot.Element, true)) originSlot.Empty();
                    break;
                case Type.craftSlot:
                    if (GameManager.ResourceManager.HasEnoughPower(originSlot.GetSlotCost()) && finalSlot.AddItem(originSlot.Element, true))
                    {
                        GameManager.ResourceManager.ConsumePower(originSlot.GetSlotCost());
                        originSlot.Empty();
                    }
                    break;
                case Type.recipeSlot:
                    if (craftSystem.HasEnoughPower() && finalSlot.AddItem(originSlot.Element, true)) craftSystem.ConsumePower();
                    break;
                case Type.ressourceSlot:
                    if (GameManager.ResourceManager.HasEnoughPower(originSlot.Element.GetCost()) && finalSlot.AddItem(originSlot.Element, true)) GameManager.ResourceManager.ConsumePower(originSlot.Element.GetCost());
                    break;
                default:
                    break;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;
        rectTransform.anchoredPosition = initialRectTransform;
        transform.SetParent(snapParent);
        itemIcon.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        currentSlot.OnClick();
    }

    public void SetParentSnap(Transform parentSnap)
    {
        snapParent = parentSnap;
    }


}
