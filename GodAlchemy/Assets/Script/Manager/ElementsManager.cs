using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class ElementsManager : BaseManager<ElementsManager>
{
    protected override void InitManager()
    {
    }

    [SerializeField]
    private TileBase treeTile;
    // Creates some Trees / bushes near the cursor
    public void Vegetation()
    {
        GameManager.GridManager.SetTilesOnMouseInRange(treeTile, 2);
    }

<<<<<<< Updated upstream
=======

    [SerializeField]
    private GameObject rockPrefab;
    public void Rock()
    {
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        Debug.Log(rockPrefab);
        GameObject rock = Instantiate(rockPrefab);
        rock.transform.position = mousePos;
    }
>>>>>>> Stashed changes

    public void Fire()
    {
    }

    [SerializeField]
    private TileBase dirtTile;
    public void Dirt()
    {
        GameManager.GridManager.SetTilesOnMouseInRange(dirtTile, 4);
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
