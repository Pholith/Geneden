using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Tooltip("Boolean value if mouse is pressed or not.")]
    [SerializeField] private bool isMousePressed;
    private TileEditor editorHandler;
    // Start is called before the first frame update
    void Start()
    {
        isMousePressed = false;
        editorHandler = GetComponent<TileEditor>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        RenderInputs();
        ResetValues();
    }

    private void GetInputs()
    {
        if(Input.GetMouseButton(0))
        {
            //Debug.Log("Mouse Hold");
            isMousePressed = true;
        }
    }

    private void RenderInputs()
    {
        if (isMousePressed == true)
        {
            editorHandler.ChangeTile();
        }
    }

    private void ResetValues()
    {
        isMousePressed = false;
    }
}
