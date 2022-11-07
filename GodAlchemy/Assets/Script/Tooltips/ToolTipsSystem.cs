using UnityEngine;

public class ToolTipsSystem : MonoBehaviour
{
    private static ToolTipsSystem current;
    [SerializeField]
    private Tooltip tooltip;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        tooltip = FindObjectOfType<Tooltip>();
        current.tooltip.gameObject.SetActive(false);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }

    public static void Show(string content = "", string header = "")
    {
        if ((content == null) && (header == null))
        {
            return;
        }
        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
    }
}
