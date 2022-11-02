using UnityEngine;
using UnityEngine.EventSystems;

public class GameplayBoard : MonoBehaviour, IDropHandler
{
    private CraftingSystem craftingSystem;

    private void Start()
    {
        craftingSystem = FindObjectOfType<CraftingSystem>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject origin = eventData.pointerDrag;
        ItemSlot originSlot = origin.GetComponent<ItemSlot>();
        if (originSlot == null) return;

        if (originSlot.SlotType != ItemSlot.Type.recipeSlot && GameManager.Instance.ResourceManager.HasEnoughPower(originSlot.GetSlotCost()))
        {
            GameManager.Instance.ResourceManager.ConsumePower(originSlot.GetSlotCost());
            originSlot.Element.EffectOnMap.Invoke();
        }

        if (originSlot.SlotType == ItemSlot.Type.recipeSlot) craftingSystem.ConsumePower();
        if (originSlot.SlotType != ItemSlot.Type.ressourceSlot) originSlot.Empty();

    }
}
