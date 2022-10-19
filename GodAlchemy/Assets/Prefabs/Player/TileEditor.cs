using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileEditor : MonoBehaviour
{

    public Tilemap tilemap;
    [SerializeField] private Tile selectedTile;
    [SerializeField] private Vector3Int gridPos;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameObject previewSprite;
    [SerializeField] private SpriteRenderer previewSpriteRenderer;
    [SerializeField] private RessourceManager ressourceManager;
    [SerializeField] private int tileDivinePowerCost;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        gameUI = FindObjectOfType<GameUI>();
        ressourceManager = GetComponent<RessourceManager>();
        previewSprite = new GameObject();
        previewSprite.AddComponent<SpriteRenderer>();
        previewSpriteRenderer = previewSprite.GetComponent<SpriteRenderer>();
        previewSpriteRenderer.sortingOrder = 2;
        tileDivinePowerCost = 5;
    }

    private void Update()
    {
        GetMouseGridPos();
        PreviewTile();
    }
    private void GetMouseGridPos()
    {
        Vector3 _screenPos = Input.mousePosition;
        Vector3 _worldPos = mainCamera.ScreenToWorldPoint(_screenPos);
        _worldPos.z = 0.0f;
        gridPos = tilemap.WorldToCell(_worldPos);
    }
    public void ChangeTile()
    {
        if (selectedTile != null)
        {  
            if(ressourceManager.HasEnoughPower(tileDivinePowerCost))
            {
                if (!(tilemap.GetTile(gridPos) == selectedTile))
                {
                    ressourceManager.SubstractDivinePower(tileDivinePowerCost);
                    print("Tile Set!");
                    tilemap.SetTile(gridPos, selectedTile);
                }
            }
        }
        else
        {
            Debug.Log(gridPos);
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
            previewSprite.transform.position = gridPos;
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
