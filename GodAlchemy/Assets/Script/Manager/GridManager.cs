
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : BaseManager<GridManager>
{

    public Tilemap GameGrid;
    
    private Camera mainCamera;

    protected override void InitManager()
    {
        mainCamera = FindObjectOfType<Camera>();

    }

    public void SetTileOnMouse(TileBase tile)
    {
        GameGrid.SetTile(GetMouseGridPos().ToVector3Int(), tile);
    }

    public void SetTilesOnMouseInRange(TileBase tile, int range)
    {
        Vector3Int tilePos = GetMouseGridPos().ToVector3Int();
        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                GameGrid.SetTile(tilePos + new Vector3Int(i, j), tile);
            }
        }
    }

    public Vector3 GetMouseGridPos()
    {
        Vector3 _screenPos = Input.mousePosition;
        Vector3 _worldPos = mainCamera.ScreenToWorldPoint(_screenPos);
        _worldPos.z = 0.0f;
        return GameGrid.WorldToCell(_worldPos);
    }
}