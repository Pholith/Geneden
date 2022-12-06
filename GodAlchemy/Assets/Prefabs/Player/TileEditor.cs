using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileEditor : MonoBehaviour
{

    private ResourceManager ressourceManager;
    private GameUI gameUI;
    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameManager.GridManager.MainGameGrid;
        gameUI = FindObjectOfType<GameUI>();
        ressourceManager = GetComponent<ResourceManager>();
    }

    private void Update()
    {
        
    }

}
