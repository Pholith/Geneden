
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : BaseManager<GridManager>
{

    public Tilemap MainGameGrid;
    public Tilemap DirtGameGrid;
    
    [SerializeField]
    private TileBase dirt; 

    private Camera mainCamera;

    protected override void InitManager()
    {
        mainCamera = FindObjectOfType<Camera>();

    }

    public void SetTileOnMouse(TileBase tile)
    {
        MainGameGrid.SetTile(GetMouseGridPos().ToVector3Int(), tile);
    }

    public void SetTilesOnMouseInRange(TileBase tile, int range)
    {
        Vector3Int tilePos = GetMouseGridPos().ToVector3Int();
        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                if (tile == dirt)
                {
                    DirtGameGrid.SetTile(tilePos + new Vector3Int(i, j), tile);
                } else
                {
                    MainGameGrid.SetTile(tilePos + new Vector3Int(i, j), tile);
                    if (tile == null) DirtGameGrid.SetTile(tilePos + new Vector3Int(i, j), null);
                }
            }
        }
    }

    public void RemoveObjectsOnMouse()
    {
        //TODO
    }

    public Vector3 GetMouseGridPos()
    {
        Vector3 _screenPos = Input.mousePosition;
        Vector3 _worldPos = mainCamera.ScreenToWorldPoint(_screenPos);
        _worldPos.z = 0.0f;
        return MainGameGrid.WorldToCell(_worldPos);
    }
}
