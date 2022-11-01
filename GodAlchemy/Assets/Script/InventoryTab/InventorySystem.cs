using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] 
    private ItemSlot[] itemSlotList;

    private void Start()
    {
        itemSlotList = transform.GetComponentsInChildren<ItemSlot>();
    }

    public bool AddItem(ElementScriptableObject addedItem)
    {
        foreach(ItemSlot slot in itemSlotList)
        {
            if (slot.IsEmpty()) { 
                slot.AddItem(addedItem);
                return true;
            }
        }
        return false;
    }
}
