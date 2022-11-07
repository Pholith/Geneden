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

    public void Water()
    {
        GameManager.GridManager.SetTilesOnMouseInRange(null, 4);
    }

    [SerializeField]
    private GameObject lightningPrefab;
    public void Lightning()
    {
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        GameObject lightning = Instantiate(lightningPrefab);
        lightning.transform.position = mousePos;
        var animator = lightning.GetComponent<Animator>();
        // Mettre le feu et particules de feu
    }
}
