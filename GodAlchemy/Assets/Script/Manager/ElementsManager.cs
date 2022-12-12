using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ElementsManager : BaseManager<ElementsManager>
{
    public List<ElementScriptableObject> Elements { get; private set; }

    private ScreenShake cameraShaker;

    public const int DAMAGE_LOW = 2000;
    public const int DAMAGE_MEDIUM = 3000;
    public const int DAMAGE_HIGH = 10000;

    [SerializeField]
    private QuestSystem questSystem;

    protected override void InitManager()
    {
        Resources.LoadAll("Elements");
        Elements = new List<ElementScriptableObject>(Resources.FindObjectsOfTypeAll<ElementScriptableObject>());
        Elements.Sort();
    }

    private void Start()
    {
        cameraShaker = FindObjectOfType<ScreenShake>();
    }
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

    [Rpc]
    public void ShakeScreenRPC(float duration, float intensity)
    {
        cameraShaker.Shake(duration, intensity);
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

    [SerializeField]
    private NetworkPrefabRef foodMarmitePrefab;
    public void Food()
    {
        questSystem.ElementInvoked("Nourriture");
        Instance.SpawnObjectRPC(foodMarmitePrefab, GameManager.GridManager.GetMouseGridPos());
    }
    public void Fire()
    {
        questSystem.ElementInvoked("Feu");
    }

    public void CampFire()
    {
        questSystem.ElementInvoked("Foyer");
    }
    [SerializeField]
    private TileBase hillTile;
    public void Hill()
    {
        questSystem.ElementInvoked("Colline");
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        Instance.SpawnObjectRPC(dirtParticlePrefab, GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        new GameTimer(timeInSecondAfterParticleStart, () => GameManager.GridManager.SetTileInRange(hillTile, mousePos.ToVector3Int(), 5, true));
    }

    [SerializeField]
    private NetworkPrefabRef airParticlePrefab;
    public void Air()
    {
        questSystem.ElementInvoked("Air");
        Instance.SpawnObjectRPC(airParticlePrefab, GameManager.GridManager.GetMouseGridPos());
    }

    [SerializeField]
    private NetworkPrefabRef gustParticlePrefab;
    public void Gust()
    {
        questSystem.ElementInvoked("Bourrasque");
        Instance.SpawnObjectRPC(airParticlePrefab, GameManager.GridManager.GetMouseGridPos());
    }

    [SerializeField]
    private TileBase dirtTile;
    [SerializeField]
    private NetworkPrefabRef dirtParticlePrefab;
    public void Dirt()
    {
        questSystem.ElementInvoked("Terre");
        Instance.SpawnObjectRPC(dirtParticlePrefab, GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => GameManager.GridManager.SetTileInRange(dirtTile, mousePos.ToVector3Int(), 4, true));
    }

    [SerializeField]
    private NetworkPrefabRef waterParticlePrefab;
    public void Water()
    {
        questSystem.ElementInvoked("Eau");
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        Instance.SpawnObjectRPC(waterParticlePrefab, mousePos + particleSystemOffsets);
        new GameTimer(timeInSecondAfterParticleStart, () => GameManager.GridManager.SetTileInRange(null, mousePos.ToVector3Int(), 3, true));
    }

    [SerializeField]
    private NetworkPrefabRef rainPrefab;
    public void Rain()
    {
        questSystem.ElementInvoked("Pluie");
    }

    [SerializeField]
    private NetworkPrefabRef lightningPrefab;
    public void Lightning()
    {
        questSystem.ElementInvoked("Foudre");
        Instance.SpawnObjectRPC(lightningPrefab, GameManager.GridManager.GetMouseGridPos());
        Instance.ShakeScreenRPC(0.3f, 0.1f);
        List<Collider2D> resources = GameManager.GridManager.GetResourcesInRange(GameManager.GridManager.GetMouseGridPos().ToVector3Int(), 3);
        foreach (Collider2D resource in resources)
        {
            BuildingGeneric buildingComponent = resource.GetComponent<BuildingGeneric>();
            if (buildingComponent != null) buildingComponent.Damage(DAMAGE_MEDIUM);
            else Destroy(resource.gameObject);
        }
    }

    [SerializeField]
    private NetworkPrefabRef dustPrefab;
    [SerializeField]
    private int numberOfCloudToSpawn;
    public void Dust()
    {
        questSystem.ElementInvoked("Poussière");
        for (int i = 0; i < numberOfCloudToSpawn; i++)
        {
            Instance.SpawnObjectRPC(dustPrefab, GameManager.GridManager.GetMouseGridPos());
        }
    }

    [SerializeField]
    private NetworkPrefabRef meteorPrefab;
    public void Meteor()
    {
        questSystem.ElementInvoked("Météorite");
        Instance.SpawnObjectRPC(meteorPrefab, GameManager.GridManager.GetMouseGridPos());
    }


    [SerializeField]
    private NetworkPrefabRef treePrefab;
    [SerializeField]
    private NetworkPrefabRef bushPrefab;
    public void Plant()
    {
        questSystem.ElementInvoked("Végétation");
        IEnumerable<Vector3Int> cellPositions = GridManager.Instance.GetCellsPositionsOfRange(GameManager.GridManager.GetMouseGridPos().ToVector3Int(), 3).PickRandom(4);
        foreach (Vector3Int cell in cellPositions)
        {
            if (GameManager.GridManager.IsThereResourceOnTile(cell)) continue;
            switch (Random.Range(0, 2))
            {
                case 0:
                    Instance.SpawnObjectRPC(treePrefab, cell);
                    break;
                case 1:
                    Instance.SpawnObjectRPC(bushPrefab, cell);
                    break;
            }
        }
    }

    public void Adn()
    {
        questSystem.ElementInvoked("ADN");
        BuildingsScriptableObject buildingToSpawn = BuildingManager.Instance.GetBuildingByName("Maison Commune");
        BuildingManager.Instance.BuildBuilding(buildingToSpawn);
        /*switch (Random.Range(0, 1))
        {
            case 0:
                Plant();
                break;
            case 1:
                Animals();
                break;
        }*/
    }

    private void Animals()
    {

    }

    [SerializeField]
    private NetworkPrefabRef rockPrefab;
    public void Rock()
    {
        questSystem.ElementInvoked("Roche");
        Instance.SpawnObjectRPC(dirtParticlePrefab, GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => Instance.SpawnObjectRPC(rockPrefab, mousePos));
    }
    [SerializeField]
    private NetworkPrefabRef ironPrefab;
    public void Iron()
    {
        questSystem.ElementInvoked("Fer");
        Instance.SpawnObjectRPC(dirtParticlePrefab, GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => Instance.SpawnObjectRPC(ironPrefab, mousePos));
    }

    [SerializeField]
    private NetworkPrefabRef silverPrefab;
    public void Silver()
    {
        questSystem.ElementInvoked("Argent");
        Instance.SpawnObjectRPC(dirtParticlePrefab, GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => Instance.SpawnObjectRPC(silverPrefab, mousePos));
    }
    [SerializeField]
    private NetworkPrefabRef goldPrefab;
    public void Gold()
    {
        questSystem.ElementInvoked("Or");
        Instance.SpawnObjectRPC(dirtParticlePrefab, GameManager.GridManager.GetMouseGridPos() + particleSystemOffsets);
        Vector3 mousePos = GameManager.GridManager.GetMouseGridPos();
        new GameTimer(timeInSecondAfterParticleStart, () => Instance.SpawnObjectRPC(goldPrefab, mousePos));
    }

}
