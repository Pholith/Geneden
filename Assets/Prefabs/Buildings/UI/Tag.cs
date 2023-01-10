using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag : MonoBehaviour
{
    private string tagName;
    //ToolTip
    private ToolTipTrigger toolTipTrigger;
    // Start is called before the first frame update
    void Start()
    {
        //Tooltip
        toolTipTrigger = gameObject.GetComponent<ToolTipTrigger>();
        UpdateToolTip();
    }

    public void SetName(string name)
    {
        this.tagName = name;
    }

    private void UpdateToolTip()
    {
        toolTipTrigger.SetHeader(tagName);
        toolTipTrigger.SetContent(null);
    }
}
