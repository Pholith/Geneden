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
                     public bool elementIsPayed;
    [SerializeField] private int itemNumber;
    [SerializeField] public type slotType;
    private GameObject uiCase;

    private Image itemIcon;
    private TextMeshProUGUI costText;

    //Crafting Slot Type
    private CraftingSystem craftManager;
    private InventorySystem playerInventory;
    private RessourceManager ressourceManager;

    const int COST_FACTOR = 10;






    // Start is called before the first frame update
    void Start()
    {
        uiCase = transform.Find("Case").transform.gameObject;
        itemIcon = uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        costText = uiCase.transform.Find("ItemAmount").transform.gameObject.GetComponent<TextMeshProUGUI>();
        craftManager = FindObjectOfType<CraftingSystem>();
        playerInventory = FindObjectOfType<InventorySystem>();
        ressourceManager = FindObjectOfType<RessourceManager>();
        elementIsPayed = false;
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
            costText.text = (element.DifficultyToCraft * 10).ToString();
        }
        else
        {
            itemIcon.sprite = null;
            itemNumber = 0;
            costText.text = ""; 
        }
    }

    public void updateSlot()
    {
        if(itemNumber > 0)
        {
            itemIcon.sprite = element.Sprite;
            costText.text = (element.DifficultyToCraft * 10).ToString();
        }
        else
        {
            element = null;
            itemNumber = 0;
            itemIcon.sprite = null;
            elementIsPayed = false;
            costText.text = "";
        }
    }

    public bool addItem(ElementScriptableObject addedElement,int amount)
    {
        if(element != null)
        {
            if (slotType == type.craftSlot)
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
            itemNumber += amount;
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
        if (slotType == type.craftSlot)
        {
            craftManager.craftDoneClick();
        }
        else if(slotType == type.ressourceSlot)
        {
            craftManager.addRessource(element);
        }
        else if((slotType == type.itemSlot) || (!isNotForbiddenElement() && elementIsPayed == false))
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
        if(finalSlot == originSlot)
        {
            return;
        }

        if(finalSlot.element == null)
        {
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
                        finalSlot.elementIsPayed = true;
                        originSlot.suppItem(1);
                    }
                }
            }
            if (finalSlot.slotType == type.itemSlot)
            {
                if ((originSlot.slotType == type.itemSlot))
                {
                    if (finalSlot.addItem(originSlot.element, originSlot.itemNumber))
                    {
                        finalSlot.elementIsPayed = originSlot.elementIsPayed;
                        originSlot.suppItem(itemNumber);
                    }
                }
                if (originSlot.slotType == type.recipeSlot)
                {
                    int _amount = originSlot.element.DifficultyToCraft * COST_FACTOR;
                    if(originSlot.elementIsPayed)
                    {
                        if (finalSlot.addItem(originSlot.element, originSlot.itemNumber))
                        {
                            originSlot.suppItem(itemNumber);
                            finalSlot.elementIsPayed = true;
                        }
                    }
                    else
                    {
                        if (ressourceManager.HasEnoughPower(_amount))
                        {
                            if (finalSlot.addItem(originSlot.element, originSlot.itemNumber))
                            {
                                originSlot.suppItem(itemNumber);
                                ressourceManager.SubstractDivinePower(_amount);
                                finalSlot.elementIsPayed = true;
                            }
                        }
                    }
                }
                if ((originSlot.slotType == type.craftSlot))
                {
                    craftManager.craftDoneDrop(finalSlot);
                }
                if ((originSlot.slotType == type.ressourceSlot))
                {
                    int _amount = originSlot.element.DifficultyToCraft * COST_FACTOR;
                    if (ressourceManager.HasEnoughPower(_amount))
                    {
                        if (finalSlot.addItem(originSlot.element, originSlot.itemNumber))
                        {
                            finalSlot.elementIsPayed = true;
                            ressourceManager.SubstractDivinePower(_amount);
                        }
                    }
                        
                }
            }
        }
        
    }

    public int getSlotCost()
    {
        if (elementIsPayed)
            return 0;
        else
            return (element.DifficultyToCraft * 10);
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
