using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBoard : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        GameObject origin = eventData.pointerDrag;
        ItemSlot originSlot = origin.GetComponent<ItemSlot>();
        if (originSlot == null) return;

        if (originSlot.SlotType != ItemSlot.Type.ressourceSlot) originSlot.Empty();
    }
}
