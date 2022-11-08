using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class ElementsManager : BaseManager<ElementsManager>
{
    protected override void InitManager()
    {
    }



    public void Fire()
    {

    }
    
    [SerializeField]
    private TileBase hillTile;
    public void Hill()
    {
        GameManager.GridManager.SetTilesOnMouseInRange(hillTile, 5);
    }

    [SerializeField]
    private TileBase dirtTile;
    public void Dirt()
    {
        GameManager.GridManager.SetTilesOnMouseInRange(dirtTile, 3);
    }


    [SerializeField]
    private GameObject waterPrefab;
    public void Water()
    {
        var water = Instantiate(waterPrefab);
        water.transform.position = GameManager.GridManager.GetMouseGridPos() + new Vector3Int(0, 5, 0);
        var mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(2, () => GameManager.GridManager.SetTileInRange(null, mousePos.ToVector3Int(), 4));
    }

    [SerializeField]
    private GameObject lightningPrefab;
    public void Lightning()
    {
        GameObject lightning = Instantiate(lightningPrefab);
        lightning.transform.position = GameManager.GridManager.GetMouseGridPos(); ;
        var animator = lightning.GetComponent<Animator>();
        // Mettre le feu et particules de feu
    }
}
