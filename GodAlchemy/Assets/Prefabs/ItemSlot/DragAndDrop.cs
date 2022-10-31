using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler, IPointerClickHandler
{

    [SerializeField] private ItemSlot currentSlot;
    [SerializeField] private Image itemImage;
    [SerializeField] private Vector2 initialRectTransform;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;

    [SerializeField] private Transform snapParent;


    // Start is called before the first frame update
    void Start()
    {
        currentSlot = transform.GetComponent<ItemSlot>();
        itemImage = transform.Find("Case").transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        canvas = FindObjectOfType<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemImage.sprite != null)
        {
            rectTransform = itemImage.GetComponent<RectTransform>();
            initialRectTransform = rectTransform.anchoredPosition;
            snapParent = transform.parent;
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            itemImage.raycastTarget = false;

        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = initialRectTransform;
        transform.SetParent(snapParent);
        itemImage.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        currentSlot.onClick();
    }

    public void setParentSnap(Transform parentSnap)
    {
        snapParent = parentSnap;
    }
    

}
