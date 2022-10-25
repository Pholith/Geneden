using UnityEngine.Tilemaps;

public class ElementsManager : BaseManager<ElementsManager>
{
    public void Fire()
    {
    }

    public TileBase DirtTile;
    public void Dirt()
    {
        GameManager.GridManager.SetTilesOnMouseInRange(DirtTile, 4);
    }

    protected override void InitManager()
    {
    }
}
