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
        if (originSlot.IsEmpty()) return;

        if (originSlot.SlotType != ItemSlot.Type.recipeSlot && originSlot.SlotType != ItemSlot.Type.recipeBookSlot && GameManager.ResourceManager.HasEnoughPower(originSlot.GetSlotCost()))
        {
            GameManager.ResourceManager.ConsumePower(originSlot.GetSlotCost());
            originSlot.Element.EffectOnMap.Invoke();
        }

        if (originSlot.SlotType == ItemSlot.Type.recipeSlot && GameManager.ResourceManager.HasEnoughPower(originSlot.GetSlotCost()))
        {
            ElementScriptableObject _tempElement = originSlot.Element;
            craftingSystem.ConsumePower();
            _tempElement.EffectOnMap.Invoke();
        }

        if (originSlot.SlotType != ItemSlot.Type.ressourceSlot && originSlot.SlotType != ItemSlot.Type.recipeBookSlot) originSlot.Empty();

    }
}
