using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    private Tile selectedTile;
    private TileEditor playerEditor;
    private Button desactivatedButton;
    // Start is called before the first frame update
    void Start()
    {
        selectedTile = null;
        playerEditor = FindObjectOfType<TileEditor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSelectTile(Tile tile)
    {
        if (desactivatedButton != null)
        {
            desactivatedButton.interactable = true;
        }
        string _buttonName = EventSystem.current.currentSelectedGameObject.name;
        Button _button = GameObject.Find(_buttonName).GetComponent<Button>();
        _button.interactable = false;
        desactivatedButton = _button;
        selectedTile = tile;
        playerEditor.SetSelectedTile(selectedTile);

    }

    public void OnSelectNone()
    {
        if (desactivatedButton != null)
        {
            desactivatedButton.interactable = true;
        }
        string _buttonName = EventSystem.current.currentSelectedGameObject.name;
        Button _button = GameObject.Find(_buttonName).GetComponent<Button>();
        _button.interactable = false;
        desactivatedButton = _button;
        selectedTile = null;
        playerEditor.SetSelectedTile(selectedTile);
    }

    public Tile GetSelectedTile()
    {
        
        return selectedTile;
    }
}
