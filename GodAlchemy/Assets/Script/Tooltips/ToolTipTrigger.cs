using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    private static LTDescr delay;
    private string headerText;
    private string contentText;

    private void Start()
    {
        headerText = null;
        contentText = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(1.0f, () =>
         {
             ToolTipsSystem.Show(contentText, headerText);
         });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        ToolTipsSystem.Hide();
    }

    public void SetHeader(string header)
    {
        headerText = header;
    }

    public void SetContent(string content)
    {
        contentText = content;
    }
}
