using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingButton : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public ButtonType type;
    public enum ButtonType
    {
        Add,
        Minus,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch(type)
        {
            case ButtonType.Add:
                AddButton();
                break;
            case ButtonType.Minus:
                MinusButton();
                break;

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        return;
    }

    private void AddButton()
    {
        FindObjectOfType<BuildingInfosTable>().AddWorker();
    }

    private void MinusButton()
    {
        FindObjectOfType<BuildingInfosTable>().RemoveWorker();
    }



}
