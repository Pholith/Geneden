
using Fusion;
using System;
using System.Collections.Generic;
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
        tiles = new List<TileBase>(Resources.FindObjectsOfTypeAll<TileBase>());
        tiles.Sort(new Comparison<TileBase>((target, target2) => target.name.CompareTo(target2.name)));
    }

    private enum GameTileMapRPC
    {
        MainGameGrid,
        DirtGameGrid
    }

    [SerializeField]
    private List<TileBase> tiles;
    private int GetTileType(TileBase tileBase)
    {
        if (tileBase == null) return -1;
        return tiles.IndexOf(tileBase);
    }
    private TileBase GetTileFromType(int tileType)
    {
        if (tileType == -1) return null;
        return tiles[tileType];
    }

    [Rpc]
    private void SetTileRPC(GameTileMapRPC tilemap, Vector3Int position, int tileType)
    {
        Tilemap tilemapToChange;
        switch (tilemap)
        {
            case GameTileMapRPC.MainGameGrid:
                tilemapToChange = MainGameGrid;
                break;
            case GameTileMapRPC.DirtGameGrid:
                tilemapToChange = DirtGameGrid;
                break;
            default:
                throw new ArgumentException();
        }
        tilemapToChange.SetTile(position, GetTileFromType(tileType));
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
                    SetTileRPC(GameTileMapRPC.DirtGameGrid, position + new Vector3Int(ix, iy), GetTileType(tile));
                }
                else
                {
                    SetTileRPC(GameTileMapRPC.MainGameGrid, position + new Vector3Int(ix, iy), GetTileType(tile));
                    if (tile == null) SetTileRPC(GameTileMapRPC.DirtGameGrid, position + new Vector3Int(ix, iy), GetTileType(null));
                }
            }
        }
    }

    public void SetTilesOnMouseInRange(TileBase tile, int radius)
    {
        SetTileInRange(tile, GetMouseGridPos().ToVector3Int(), radius);
    }

    public Vector3 GetMouseGridPos()
    {
        Vector3 _screenPos = Input.mousePosition;
        Vector3 _worldPos = mainCamera.ScreenToWorldPoint(_screenPos);
        _worldPos.z = 0.0f;
        return MainGameGrid.WorldToCell(_worldPos);
    }
}
