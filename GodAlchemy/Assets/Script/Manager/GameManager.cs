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
    public ElementsManager ElementManager => elementManager;
    
    [SerializeField] 
    private ResourceManager resourceManager;
    public ResourceManager ResourceManager => resourceManager;

    protected override void InitManager()
    {
        if(instance == null)
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
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (grid == null) grid = GetComponentInChildren<GridManager>();
        if (elementManager == null) elementManager = GetComponentInChildren<ElementsManager>();
        if (resourceManager == null) resourceManager = GetComponentInChildren<ResourceManager>();
    }
#endif
}
