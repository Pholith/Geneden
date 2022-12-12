using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Partie de la classe qui gère le drag and drop des item slot
/// </summary>
public partial class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Vector2 initialRectTransform;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private GameObject previewTile;

    [SerializeField]
    private Transform snapParent;

    private Canvas canvas;
    private ItemSlot currentSlot;

    // Le Start est dans ItemSlot

    public void OnBeginDrag(PointerEventData eventData)
    {
        if ((itemIcon.sprite != null) && (SlotType != Type.recipeBookSlot))
        {
            previewTile = Instantiate(gridManager.previewTilePrefab);
            previewTile.transform.localScale = new Vector3(gridManager.MainGameGrid.cellSize.x, gridManager.MainGameGrid.cellSize.y, 0);
            rectTransform = itemIcon.GetComponent<RectTransform>();
            initialRectTransform = rectTransform.anchoredPosition;
            rectTransform.localScale = rectTransform.localScale / 2;
            snapParent = itemIcon.transform.parent;
            itemIcon.transform.SetParent(canvas.transform);
            itemIcon.transform.SetAsLastSibling();
            itemIcon.raycastTarget = false;
            GetComponent<ToolTipTrigger>().StopDelay();
        }
    }
    [SerializeField]
    private float dampingSpeed = .05f;
    private Vector3 velocity = Vector3.zero;

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;
        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform,eventData.position,eventData.pressEventCamera,out var mousePos))
        {
            rectTransform.position = Vector3.SmoothDamp(rectTransform.position, mousePos, ref velocity, dampingSpeed);
            previewTile.transform.position = gridManager.GetMouseGridPos();
        }
       

    }

    /// <summary>
    /// Quand on clic sur un slot sans drag and drop (pour aller plus vite que le drag and drop).
    /// </summary>
    public void OnClick()
    {
        switch (SlotType)
        {
            case Type.itemSlot:
                if (craftSystem.AddPayedRessource(Element))
                    this.Empty();
                break;
            case Type.craftSlot:
                if (this.elementIsPayed)
                    playerInventory.AddItem(Element, true);
                this.Empty();
                break;
            case Type.recipeSlot:
                if (!IsEmpty() && craftSystem.HasEnoughPower() && playerInventory.AddItem(Element, true)) craftSystem.ConsumePower();
                break;
            case Type.ressourceSlot:
                craftSystem.AddRessource(Element);
                break;
            case Type.recipeBookSlot:
                if (IsRecipeUnlock())
                    recipeSystem.ComputeRecipe(Element);
                    /*recipeSystem.AddRecipeToCraft(Element);*/
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
        itemIcon.transform.SetParent(snapParent);
        snapParent = null;
        Destroy(previewTile);
        rectTransform.anchoredPosition = initialRectTransform;
        rectTransform.localScale = rectTransform.localScale * 2;
        itemIcon.raycastTarget = true;
        rectTransform = null;
        
        initialRectTransform = new Vector2(0, 0);
        GameManager.ResourceManager.ToggleShowCost();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        currentSlot.OnClick();
    }

    public void SetParentSnap(Transform parentSnap)
    {
        snapParent = parentSnap;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.ResourceManager.ToggleShowCost(SlotType == Type.recipeSlot ? craftSystem.ComputeCraftCost() : GetSlotCost());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging) GameManager.ResourceManager.ToggleShowCost();
    }
}
