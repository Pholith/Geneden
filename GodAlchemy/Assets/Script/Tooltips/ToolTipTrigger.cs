using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static GameTimer delay;
    private string headerText;
    private string contentText;

    [SerializeField]
    private float toolTipDelay = 0.5f;
    private void Start()
    {
        headerText = null;
        contentText = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = new GameTimer(toolTipDelay, () =>
         {
             ToolTipsSystem.Show(contentText, headerText);
         });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopDelay();
    }

    public void StopDelay()
    {
        delay.Stop();
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
