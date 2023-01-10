using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] 
    private ItemSlot[] itemSlotList;

    private void Start()
    {
        itemSlotList = transform.GetComponentsInChildren<ItemSlot>();
    }

    public bool AddItem(ElementScriptableObject addedItem, bool payItem = false)
    {
        foreach(ItemSlot slot in itemSlotList)
        {
            if (slot.IsEmpty()) { 
                slot.AddItem(addedItem, payItem);
                return true;
            }
        }
        return false;
    }

    public ItemSlot SearchElement(ElementScriptableObject searchedElement)
    {
        foreach(ItemSlot slot in itemSlotList)
        {
            if(!slot.IsEmpty())
            {
                if(searchedElement == slot.Element)
                {
                    return slot;
                }
            }
        }
        return null;
    }
}