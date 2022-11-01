using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    [SerializeField] private ItemSlot[] ItemSlotList;



    // Start is called before the first frame update
    void Start()
    {
        ItemSlotList = transform.GetComponentsInChildren<ItemSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddItem(ElementScriptableObject addedItem)
    {
        //if(IsItemInList(addedItem))
        //{
        //    foreach(ItemSlot slot in ItemSlotList)
        //    {
        //        if(slot.element != null && slot.element.name == addedItem.name)
        //        {
        //            slot.addItem(addedItem,1);
        //            return true;
        //        }
        //    }
        //}
        //else
        //{
            foreach(ItemSlot slot in ItemSlotList)
            {
                if(slot.element == null)
                {
                    slot.addItem(addedItem,1);
                    slot.elementIsPayed = true;
                    return true;
                }
            }
        //}
        return false;
    }

    public bool IsItemInList(ElementScriptableObject checkedItem)
    {
        foreach(ItemSlot slot in ItemSlotList)
        {
            if(slot.element != null)
            {
                if (slot.element.name == checkedItem.name)
                {
                    return true;
                }
            }    
        }
        return false;
    }

}
