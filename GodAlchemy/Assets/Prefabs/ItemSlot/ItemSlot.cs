using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    [SerializeField] private GameObject uiCase;
    [SerializeField] public ElementScriptableObject element;
    [SerializeField] private int itemNumber;

    // Start is called before the first frame update
    void Start()
    {
        uiCase = transform.Find("Case").transform.gameObject;
        initSlot(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initSlot(int amount)
    {
        if (element != null)
        {
            uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>().sprite = element.Sprite;
            itemNumber = amount;
        }
        else
        {
            uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>().sprite = null;
            itemNumber = 0;
        }
    }
}
