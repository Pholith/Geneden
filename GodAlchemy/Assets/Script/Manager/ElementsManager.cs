using Fusion;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ElementsManager : BaseManager<ElementsManager>
{
    protected override void InitManager()
    {
    }

    /// <summary>
    /// Permet de faire spawn un objet en réseau. Utiliser Instance.SpawnObjectRPC !!!!
    /// </summary>
    /// <param name="position"></param>
    /// <param name="prefabRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void SpawnObjectRPC(Vector3 position, NetworkPrefabRef prefabRef)
    {
        GameManager.Instance.Runner.Spawn(prefabRef, position);
    }

    /// !!!!!!!!!!!!!
    /// Fonctions UnityEvent pour les éléments. 
    /// Attention, il ne faut pas utiliser les champs de ElementsManager autres que ceux que vous mettez en dessous.
    /// De même, ne pas utiliser les méthodes privés de cette classe sans utiliser Instance devant.
    /// C'est un peu bizarre mais c'est dû à la manière dont unity gère les unityEvent.
    /// Pour s'assurer du multijoueur, utiliser SpawnObjectRPC pour faire spawn un préfab.
    /// !!!!!!!!!!!!!

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
    private NetworkPrefabRef waterParticlePrefab;
    public void Water()
    {
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        Instance.SpawnObjectRPC(mousePos + particleSystemOffsets, waterParticlePrefab);
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
