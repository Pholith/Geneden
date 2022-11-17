using Fusion;
using UnityEngine;

/// <summary>
/// Exemple de GameManager
/// </summary>
[ExecuteAlways]
public class GameManager : BaseGameManager
{
    private static GameManager instance;
    public static new GameManager Instance => instance;


    [SerializeField]
    private GridManager grid;
    public static GridManager GridManager => instance.grid;

    [SerializeField]
    private ElementsManager elementManager;
    public static ElementsManager ElementManager => instance.elementManager;

    [SerializeField]
    private ResourceManager resourceManager;
    public static ResourceManager ResourceManager => instance.resourceManager;

    [SerializeField]
    private NetworkManager networkManager;
    public static NetworkManager NetworkManager => instance.networkManager;

    [SerializeField]
    private BuildingManager buildingManager;
    public static BuildingManager BuildingManager => instance.buildingManager;

    protected override void InitManager()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("Multiple GameManager on this scene !! Destoying this one.");
            Destroy(gameObject);
            return;
        }
        grid?.Initialization();
        elementManager?.Initialization();
        resourceManager?.Initialization();
        networkManager?.Initialization();
        buildingManager?.Initialization();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (grid == null) grid = GetComponentInChildren<GridManager>();
        if (elementManager == null) elementManager = GetComponentInChildren<ElementsManager>();
        if (resourceManager == null) resourceManager = GetComponentInChildren<ResourceManager>();
        if (networkManager == null) networkManager = GetComponentInChildren<NetworkManager>();
        if (buildingManager == null) buildingManager = GetComponentInChildren<BuildingManager>();
    }
#endif
}
