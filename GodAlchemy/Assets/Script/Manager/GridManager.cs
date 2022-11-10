
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

    public void SetTileInRange(TileBase tile, Vector3Int position, int radius)
    {
        for (int iy = -radius; iy <= radius; iy++)
        {
            int dx = (int)Mathf.Sqrt(radius * radius - iy * iy);
            for (int ix = -dx; ix <= dx; ix++)
            {
                if (tile == dirt)
                {
                    DirtGameGrid.SetTile(position + new Vector3Int(ix, iy), tile);
                }
                else
                {
                    MainGameGrid.SetTile(position + new Vector3Int(ix, iy), tile);
                    if (tile == null) DirtGameGrid.SetTile(position + new Vector3Int(ix, iy), null);
                }
            }
        }
    }

    public void SetTilesOnMouseInRange(TileBase tile, int radius)
    {
        SetTileInRange(tile, GetMouseGridPos().ToVector3Int(), radius);
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
