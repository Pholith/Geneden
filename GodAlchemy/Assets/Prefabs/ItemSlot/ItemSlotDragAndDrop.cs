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

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerCurrentRaycast.gameObject;
        GameObject origin = eventData.pointerDrag;
        ItemSlot originSlot = origin.GetComponent<ItemSlot>();
        ItemSlot finalSlot = dropped.GetComponentInParent<ItemSlot>();
        GetComponent<ItemSlot>().SetParentSnap(transform);

        if (finalSlot.SlotType == Type.recipeSlot)
        {
            if (originSlot.SlotType == Type.ressourceSlot)
            {
                finalSlot.AddItem(originSlot.Element);
            }
            else if (originSlot.SlotType == Type.recipeSlot || originSlot.SlotType == Type.itemSlot || originSlot.SlotType == Type.craftSlot)
            {
                if (finalSlot.AddItem(originSlot.Element))
                {
                    originSlot.SuppItem();
                }
            }
        }
        if (finalSlot.SlotType == Type.itemSlot)
        {
            if (originSlot.SlotType == Type.itemSlot || originSlot.SlotType == Type.recipeSlot)
            {
                if (finalSlot.AddItem(originSlot.Element))
                {
                    originSlot.SuppItem();
                }
            }
            if (originSlot.SlotType == Type.craftSlot)
            {
                if (finalSlot.AddItem(originSlot.Element))
                {
                    craftSystem.ConsumeElement(5); // 5 juste pour le test
                }
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
