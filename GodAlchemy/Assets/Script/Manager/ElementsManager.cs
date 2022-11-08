using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class ElementsManager : BaseManager<ElementsManager>
{
    protected override void InitManager()
    {
    }

    [SerializeField]
    private Vector3Int particleSystemOffsets = new(0, 5, 0);
    [SerializeField]
    private float timeInSecondAfterParticleStart = 2;


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
    private ParticleSystem airParticlePrefab;
    public void Air()
    {
        airParticlePrefab.gameObject.Instantiate(GameManager.GridManager.GetMouseGridPos());
    }

    [SerializeField]
    private TileBase dirtTile;
    [SerializeField]
    private ParticleSystem dirtParticlePrefab;
    public void Dirt()
    {
<<<<<<< Updated upstream
        GameManager.GridManager.SetTilesOnMouseInRange(dirtTile, 3);
=======
<<<<<<< Updated upstream
        GameManager.GridManager.SetTilesOnMouseInRange(dirtTile, 4);
=======
        dirtParticlePrefab.gameObject.Instantiate(GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        var mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => GameManager.GridManager.SetTileInRange(dirtTile, mousePos.ToVector3Int(), 4));
>>>>>>> Stashed changes
>>>>>>> Stashed changes
    }


    [SerializeField]
<<<<<<< Updated upstream
    private GameObject waterPrefab;
    public void Water()
    {
        var water = Instantiate(waterPrefab);
        water.transform.position = GameManager.GridManager.GetMouseGridPos() + new Vector3Int(0, 5, 0);
        var mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(2, () => GameManager.GridManager.SetTileInRange(null, mousePos.ToVector3Int(), 4));
    }

    [SerializeField]
=======
<<<<<<< Updated upstream
=======
    private ParticleSystem waterParticlePrefab;
    public void Water()
    {
        waterParticlePrefab.gameObject.Instantiate(GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        var mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => GameManager.GridManager.SetTileInRange(null, mousePos.ToVector3Int(), 4));
    }

    [SerializeField]
>>>>>>> Stashed changes
>>>>>>> Stashed changes
    private GameObject lightningPrefab;
    public void Lightning()
    {
        GameObject lightning = Instantiate(lightningPrefab);
<<<<<<< Updated upstream
        lightning.transform.position = GameManager.GridManager.GetMouseGridPos(); ;
=======
<<<<<<< Updated upstream
        lightning.transform.position = mousePos;
=======
        lightning.transform.position = GameManager.GridManager.GetMouseGridPos();
>>>>>>> Stashed changes
>>>>>>> Stashed changes
        var animator = lightning.GetComponent<Animator>();
        // Mettre le feu et particules de feu
    }
}
