using Fusion;
using UnityEngine;
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
        dirtParticlePrefab.gameObject.Instantiate(GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => GameManager.GridManager.SetTileInRange(dirtTile, mousePos.ToVector3Int(), 4));
    }


    [SerializeField]
    private ParticleSystem waterParticlePrefab;
    public void Water()
    {
        Runner.Spawn(waterParticlePrefab.gameObject, GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        //waterParticlePrefab.gameObject.Instantiate(GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => GameManager.GridManager.SetTileInRange(null, mousePos.ToVector3Int(), 4));
    }

    [SerializeField]
    private GameObject lightningPrefab;
    public void Lightning()
    {
        GameObject lightning = Instantiate(lightningPrefab);
        lightning.transform.position = GameManager.GridManager.GetMouseGridPos();
        Animator animator = lightning.GetComponent<Animator>();
        // Mettre le feu et particules de feu
    }
}
