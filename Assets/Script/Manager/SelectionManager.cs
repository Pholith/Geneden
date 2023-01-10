using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : BaseManager<SelectionManager>
{


    public HashSet<SelectableObject> SelectedObjects = new HashSet<SelectableObject>();
    public List<SelectableObject> SelectableObjects = new List<SelectableObject>();
    protected override void InitManager()
    {
    }

    public void SelectObject(SelectableObject SelectedObject)
    {
        SelectedObjects.Add(SelectedObject);
        SelectedObject.OnSelected();
    }

    public void DeselectObject(SelectableObject SelectedObject)
    {
        SelectedObject.OnDeselected();
        SelectedObjects.Remove(SelectedObject);
    }

    public void DeselectAll()
    {
        foreach(SelectableObject selectedObjects in SelectedObjects)
        {
            selectedObjects.OnDeselected();
        }
        SelectedObjects.Clear();
    }

    public bool isSelected(SelectableObject selectedObject)
    {
        return SelectedObjects.Contains(selectedObject);
    }
}
