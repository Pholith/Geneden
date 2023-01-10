﻿using UnityEngine;

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

    public GameObject victoryScreen;
    public GameObject defeatScreen;

    [SerializeField]
    private BuildingManager buildingManager;

    public static BuildingManager BuildingManager => instance.buildingManager;
    [SerializeField]
    private SelectionManager selectionManager;

    public static SelectionManager SelectionManager => instance.selectionManager;
    [SerializeField]
    private PlayerManager playerManager;

    public static PlayerManager PlayerManager => instance.playerManager;
    

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
        selectionManager?.Initialization();
        playerManager?.Initialization();
        
    }


    private bool isGameWon() {
        return networkManager.GetWinner();
    }

    public void EndGame() {
        // TODO: force the end for both sides
        Debug.Log("Game is ended");
        if (isGameWon()) {
            Debug.Log(victoryScreen.activeSelf);
            victoryScreen.SetActive(true);
            Debug.Log(victoryScreen.activeSelf);
        }
        else {
            //defeatScreen.SetActive(true);
        }
        
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