using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IDropHandler
{

    [SerializeField] public ElementScriptableObject element;
    [SerializeField] private int itemNumber;
    [SerializeField] public type slotType;
    private GameObject uiCase;

    private Image itemIcon;
    private TextMeshProUGUI amountText;

    //Crafting Slot Type
    private CraftingSystem craftManager;
    private InventorySystem playerInventory;





    // Start is called before the first frame update
    void Start()
    {
        uiCase = transform.Find("Case").transform.gameObject;
        itemIcon = uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        amountText = uiCase.transform.Find("ItemAmount").transform.gameObject.GetComponent<TextMeshProUGUI>();
        craftManager = FindObjectOfType<CraftingSystem>();
        playerInventory = FindObjectOfType<InventorySystem>();
        initSlot(1);
    }

    // Update is called once per frame
    void Update()
    {
        updateSlot();
    }

    private void initSlot(int amount)
    {
        if (element != null)
        {
            itemIcon.sprite = element.Sprite;
            itemNumber = amount;
            amountText.text = "x" + itemNumber;
        }
        else
        {
            itemIcon.sprite = null;
            itemNumber = 0;
            amountText.text = ""; 
        }
    }

    public void updateSlot()
    {
        if(itemNumber > 0)
        {
            itemIcon.sprite = element.Sprite;
            amountText.text = "x" + itemNumber;
        }
        else
        {
            element = null;
            itemNumber = 0;
            itemIcon.sprite = null;
            amountText.text = "";
        }
    }

    public bool addItem(ElementScriptableObject addedElement,int amount)
    {
        if(element != null)
        {
            if(slotType == type.itemSlot)
            {
                if (addedElement.name == element.name)
                {
                    itemNumber += amount;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(slotType == type.craftSlot)
            {
                if (addedElement.name == element.name)
                {
                    itemNumber = amount;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
        else
        {
            element = addedElement;
            itemNumber = amount;
            return true;
        }

        return false;
    }

    public void suppItem(int amount)
    {
        if(itemNumber > 0)
        {
            itemNumber -= amount;
            if(itemNumber < 0)
            {
                itemNumber = 0;
            }
        }
    }

    //Testing
    public void onClick()
    {
        if(slotType == type.craftSlot)
        {
            craftManager.craftDone();
        }
        else if(slotType == type.ressourceSlot)
        {
            craftManager.addRessource(element);
        }
        else if((slotType == type.itemSlot) || !isNotForbiddenElement())
        {
            suppItem(1);
        }
        else if((slotType == type.recipeSlot))
        {
            if (playerInventory.AddItem(element))
                suppItem(1);
        }
    }

    public Image getItemIcon()
    {
        return itemIcon;
    }
    public void OnDrop(PointerEventData eventData)
    {
        //GameObject
        GameObject dropped = eventData.pointerCurrentRaycast.gameObject;
        GameObject origin = eventData.pointerDrag;
        ItemSlot originSlot = origin.GetComponent<ItemSlot>();
        ItemSlot finalSlot = dropped.GetComponentInParent<ItemSlot>();
        GetComponent<DragAndDrop>().setParentSnap(transform);

        if (finalSlot.slotType == type.recipeSlot)
        {
            if (originSlot.slotType == type.ressourceSlot)
            {
                finalSlot.addItem(originSlot.element, 1);
            }
            else if ((originSlot.slotType == type.recipeSlot) || (originSlot.slotType == type.itemSlot) || (originSlot.slotType == type.craftSlot))
            {
                if (finalSlot.addItem(originSlot.element, 1))
                {
                    originSlot.suppItem(1);
                }
            }
        }
        if ((finalSlot.slotType == type.itemSlot) & (originSlot.isNotForbiddenElement()))
        {
            if ((originSlot.slotType == type.itemSlot) || (originSlot.slotType == type.recipeSlot))
            {
                if (finalSlot.addItem(originSlot.element, originSlot.itemNumber))
                {
                    originSlot.suppItem(itemNumber);
                }
            }
            if((originSlot.slotType == type.craftSlot))
            {
                if (finalSlot.addItem(originSlot.element, originSlot.itemNumber))
                {
                    craftManager.consumeElement();
                }
            }
        }
    }

    public bool isNotForbiddenElement()
    {
        string[] forbiddenList = { "Feu", "Air", "Eau", "Terre" };
        foreach(string str in forbiddenList)
        {
            if (element.name.Contains(str))
            {
                return false;
            }
        }
        return true;
    }

    public enum type
    {
        itemSlot,
        craftSlot,
        recipeSlot,
        ressourceSlot
    }
}
