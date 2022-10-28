using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private ItemSlot firstElement;
    [SerializeField] private ItemSlot secondElement;
    [SerializeField] private ItemSlot resultSlot;
    [SerializeField] public ElementScriptableObject[] recipeItemBook;
    // Start is called before the first frame update
    void Start()
    {
        firstElement = transform.Find("ItemSlot1").transform.gameObject.GetComponent<ItemSlot>();
        secondElement = transform.Find("ItemSlot2").transform.gameObject.GetComponent<ItemSlot>();
        resultSlot = transform.Find("ResultSlot").transform.gameObject.GetComponent<ItemSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        checkRecipe(); 
    }

    public void checkRecipe()
    {
        foreach(ElementScriptableObject combinedElement in recipeItemBook)
        {
            if ((combinedElement.elementToCraft1.name == firstElement.element.name) & (combinedElement.elementToCraft2.name == secondElement.element.name))
            {
                resultSlot.element = combinedElement;
                return;
            }
            else if((combinedElement.elementToCraft2.name == firstElement.element.name) & (combinedElement.elementToCraft1.name == secondElement.element.name))
            {
                resultSlot.element = combinedElement;
                return;
            }
            else
            {
                resultSlot.element = null;
            }

        }
    }

    public void consumeElement()
    {
        firstElement.element = null;
        secondElement.element = null;
    }


}
