
using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [Obsolete]
    public void SetTileOnMouse(TileBase tile)
    {
        MainGameGrid.SetTile(GetMouseGridPos().ToVector3Int(), tile);
    }

    private List<Vector3Int> GetCellsPositionsOfRange(Vector3Int position, int radius)
    {
        List<Vector3Int> positionList = new();
        for (int iy = -radius; iy <= radius; iy++)
        {
            int dx = (int)Mathf.Sqrt(radius * radius - iy * iy);
            for (int ix = -dx; ix <= dx; ix++)
            {
                positionList.Add(position + new Vector3Int(ix, iy));
            }
        }
        return positionList;
    }

    public void SetTileInRange(TileBase tile, Vector3Int position, int radius)
    {
        foreach (Vector3Int cellPosition in GetCellsPositionsOfRange(position, radius))
        {
            if (IsThereResourceOnTile(cellPosition)) continue;

            if (tile == dirt)
            {
                SetTileRPC(GameTileMapRPC.DirtGameGrid, cellPosition, GetTileType(tile));
            }
            else
            {
                SetTileRPC(GameTileMapRPC.MainGameGrid, cellPosition, GetTileType(tile));
                if (tile == null) SetTileRPC(GameTileMapRPC.DirtGameGrid, cellPosition, GetTileType(null));
            }
        }
    }

    public List<Collider2D> GetResourcesInRange(Vector3Int position, int radius)
    {
        return GetResourcesOfGridCells(GetCellsPositionsOfRange(position, radius).ToArray());
    }

    private List<Collider2D> GetResourcesOfGridCells(params Vector3Int[] targetPosition)
    {
        List<Collider2D> colliders = new();
        foreach (Vector3Int cellPosition in targetPosition)
        {
            Rect rect = new Rect((Vector2Int)cellPosition, MainGameGrid.cellSize); // Permet d'afficher du debug
            rect.DrawRect();
            colliders.AddRange(Physics2D.OverlapBoxAll((Vector2Int)cellPosition, MainGameGrid.cellSize, 0f, LayerMask.GetMask("Resources")));
        }

        return colliders.Distinct().ToList();
    }
    private bool IsThereResourceOnTile(Vector3Int targetPosition)
    {
        return GetResourcesOfGridCells(targetPosition).Count > 0;
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

    public Vector3 GetGridPos(Vector3 pos)
    {
        Vector3 _worldPos = mainCamera.ScreenToWorldPoint(pos);
        _worldPos.z = 0.0f;
        return MainGameGrid.WorldToCell(_worldPos);
    }
}
