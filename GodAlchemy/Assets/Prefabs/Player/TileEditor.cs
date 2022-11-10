using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileEditor : MonoBehaviour
{

    [SerializeField] private Tile selectedTile;
    private int tileDivinePowerCost;
    private GameObject previewSprite;
    private SpriteRenderer previewSpriteRenderer;
    private ResourceManager ressourceManager;
    private GameUI gameUI;
    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameManager.GridManager.MainGameGrid;
        gameUI = FindObjectOfType<GameUI>();
        ressourceManager = GetComponent<ResourceManager>();
        previewSprite = new GameObject();
        previewSprite.AddComponent<SpriteRenderer>();
        previewSpriteRenderer = previewSprite.GetComponent<SpriteRenderer>();
        previewSpriteRenderer.sortingOrder = 2;
        tileDivinePowerCost = 5;
    }

    private void Update()
    {
        
        PreviewTile();
    }
    public void ChangeTile()
    {
        if (selectedTile != null)
        {  
            Vector3Int tilePositionUnderMouse = GameManager.GridManager.GetMouseGridPos().ToVector3Int();
            if (ressourceManager.HasEnoughPower(tileDivinePowerCost))
            {
                if (!(tilemap.GetTile(tilePositionUnderMouse) == selectedTile))
                {
                    ressourceManager.ConsumePower(tileDivinePowerCost);
                    tilemap.SetTile(tilePositionUnderMouse, selectedTile);
                }
            }
        }
        
    }

    public void SetSelectedTile(Tile selected_tile)
    {
        selectedTile = selected_tile;
    }

    public void PreviewTile()
    {
        if (selectedTile != null)
        {
            previewSpriteRenderer.sprite = selectedTile.sprite;
            previewSprite.transform.position = GameManager.GridManager.GetMouseGridPos();
            previewSprite.transform.position += new Vector3(0.5f, 0.5f, 0);
            if (ressourceManager.HasEnoughPower(tileDivinePowerCost))
            {
                previewSpriteRenderer.color = new Color(255f, 255f, 255f,255f);
            }
            else
            {
                previewSpriteRenderer.color = new Color(0, 0, 0, 255f);
            }
        }
        else
        {
            previewSprite.SetActive(false);
        }
    }

    public void OnOverButton()
    {
        selectedTile = null;
        previewSprite.SetActive(false);
    }

    public void OnExitButton()
    {
        selectedTile = gameUI.GetSelectedTile();
        previewSprite.SetActive(true);
    }
}
