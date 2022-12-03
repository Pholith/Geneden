using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ElementsManager : BaseManager<ElementsManager>
{
    protected override void InitManager()
    {
        Resources.LoadAll("Elements");
        Elements = new List<ElementScriptableObject>(Resources.FindObjectsOfTypeAll<ElementScriptableObject>());
        Elements.Sort();
    }
    [SerializeField]
    public List<ElementScriptableObject> Elements { get; private set; }

    /// <summary>
    /// Permet de faire spawn un objet en r�seau. Utiliser Instance.SpawnObjectRPC !!!!
    /// </summary>
    /// <param name="position"></param>
    /// <param name="prefabRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void SpawnObjectRPC(NetworkPrefabRef prefabRef, Vector3 position)
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
    private NetworkPrefabRef airParticlePrefab;
    public void Air()
    {
        Instance.SpawnObjectRPC(airParticlePrefab, GameManager.GridManager.GetMouseGridPos());
    }

    [SerializeField]
    private TileBase dirtTile;
    [SerializeField]
    private NetworkPrefabRef dirtParticlePrefab;
    public void Dirt()
    {
        Instance.SpawnObjectRPC(dirtParticlePrefab, GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => GameManager.GridManager.SetTileInRange(dirtTile, mousePos.ToVector3Int(), 4));
    }

    [SerializeField]
    private NetworkPrefabRef rock;
    public void Rock()
    {
        Instance.SpawnObjectRPC(dirtParticlePrefab, GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        new GameTimer(timeInSecondAfterParticleStart, () => Instance.SpawnObjectRPC(rock, GameManager.GridManager.GetMouseGridPos()));
    }

    [SerializeField]
    private NetworkPrefabRef waterParticlePrefab;
    public void Water()
    {
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        Instance.SpawnObjectRPC(waterParticlePrefab, mousePos + particleSystemOffsets);
        new GameTimer(timeInSecondAfterParticleStart, () => GameManager.GridManager.SetTileInRange(null, mousePos.ToVector3Int(), 4));
    }

    [SerializeField]
    private NetworkPrefabRef lightningPrefab;
    public void Lightning()
    {
        Instance.SpawnObjectRPC(lightningPrefab, GameManager.GridManager.GetMouseGridPos());
        // inflige des dégats aux batiments
    }

    [SerializeField]
    private NetworkPrefabRef dustPrefab;
    [SerializeField]
    private int numberOfCloudToSpawn;
    public void Dust()
    {
        for (int i = 0; i < numberOfCloudToSpawn; i++)
        {
            Instance.SpawnObjectRPC(dustPrefab, GameManager.GridManager.GetMouseGridPos());
        }
    }

    [SerializeField]
    private NetworkPrefabRef meteorPrefab;
    public void Meteor()
    {
        Instance.SpawnObjectRPC(meteorPrefab, GameManager.GridManager.GetMouseGridPos());
    }


}
