using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableObject : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer selectionSprite;
    private ObjectType type;
    

    public enum ObjectType
    {
        Building,
        Units,
        Ressource
    }

    private void Awake()
    {
        SelectionManager.Instance.SelectableObjects.Add(this);
        selectionSprite = transform.Find("SelectionIndicator").GetComponent<SpriteRenderer>();
        selectionSprite.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (SelectionManager.Instance.isSelected(this))
        {
            Debug.Log("Object Deselected");
            SelectionManager.Instance.DeselectObject(this);
        }
        else
        {
            Debug.Log("Object Selected");
            SelectionManager.Instance.DeselectAll();
            SelectionManager.Instance.SelectObject(this);  
        }
           
    }

    public void OnSelected()
    {
        FindObjectOfType<BuildingInfosTable>(true).ObjectSelected(this);
        selectionSprite.gameObject.SetActive(true);

    }

    public void OnDeselected()
    {
        FindObjectOfType<BuildingInfosTable>(true).ObjectUnSelected(this);
        selectionSprite.gameObject.SetActive(false);
    }

    public ObjectType GetObjectType()
    {
        return type;
    }
}
