using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    private TextMeshProUGUI headerField;
    private TextMeshProUGUI contentField;
    private LayoutElement layoutElemnt;

    [SerializeField]
    private int characterLimit;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
    public void SetText(string content, string header)
    {
        if(string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        if (string.IsNullOrEmpty(content))
        {
            contentField.gameObject.SetActive(false);
        }
        else
        {
            contentField.gameObject.SetActive(true);
            contentField.text = content;
        }

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElemnt.enabled = (headerLength > characterLimit || contentLength > characterLimit) ? true : false;
    }

    private void Update()
    {
        Vector2 position = Input.mousePosition;
        transform.position = position;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
    }

    private void Start()
    {
        headerField = gameObject.transform.Find("Header").GetComponent<TextMeshProUGUI>();
        contentField = gameObject.transform.Find("Content").GetComponent<TextMeshProUGUI>();
        layoutElemnt = GetComponent<LayoutElement>();
        characterLimit = 80;
    }

}
