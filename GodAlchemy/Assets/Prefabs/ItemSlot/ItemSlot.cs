using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private GameObject uiCase;
    [SerializeField] public ElementScriptableObject element;
    [SerializeField] private int itemNumber;
    [SerializeField] public type slotType;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI amountText;

    //Crafting Slot Type
    [SerializeField] private CraftingSystem craftManager;



    // Start is called before the first frame update
    void Start()
    {
        uiCase = transform.Find("Case").transform.gameObject;
        itemIcon = uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        amountText = uiCase.transform.Find("ItemAmount").transform.gameObject.GetComponent<TextMeshProUGUI>();
        craftManager = FindObjectOfType<CraftingSystem>();
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
        if(itemNumber != 0)
        {
            itemIcon.sprite = element.Sprite;
            amountText.text = "x" + itemNumber;
        }
        else
        {
            element = null;
            itemIcon.sprite = null;
            amountText.text = "";
        }
        
    }

    public void addItem(ElementScriptableObject addedElement,int amount)
    {
        if(element != null)
        {
            if(slotType == type.itemSlot)
            {
                if (addedElement.name == element.name)
                {
                    itemNumber += amount;
                }
            }
            else if(slotType == type.craftSlot)
            {
                if (addedElement.name == element.name)
                {
                    itemNumber = amount;
                }
            }
            
        }
        else
        {
            element = addedElement;
            itemNumber = amount;
        }
    }

    public void suppItem(int amount)
    {
        if(itemNumber > 0)
        {
            itemNumber -= amount;
        }
    }
    //Testing
    public void clickCraftResult()
    {
        if(slotType == type.craftSlot)
        {
            craftManager.craftDone();
        }
        else if(slotType == type.ressourceSlot)
        {
            craftManager.addRessource(element);
        }
        else if(slotType == type.itemSlot)
        {
            suppItem(1);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCraftResult();
    }

    public enum type
    {
        itemSlot,
        craftSlot,
        ressourceSlot
    }
}
