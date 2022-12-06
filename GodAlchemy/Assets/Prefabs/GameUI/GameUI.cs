using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private BuildingManager buildingManager;

    //
    [SerializeField]
    private GameObject InventoryTable;
    // Start is called before the first frame update
    private void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
        InventoryTable = transform.Find("Canvas").transform.Find("InventoryBoard").gameObject;
        ManageUI();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void ManageUI()
    {
        if(buildingManager.selectedBuilding == null)
        {
            InventoryTable.transform.Find("RightTable").gameObject.SetActive(true);
            InventoryTable.transform.Find("MiddleTable").gameObject.SetActive(true);
            InventoryTable.transform.Find("CraftingTable").gameObject.SetActive(true);
            InventoryTable.transform.Find("RecipeTable").gameObject.SetActive(true);
            InventoryTable.transform.Find("SwitchButtons").gameObject.SetActive(true);
            InventoryTable.transform.Find("BuildingTable").gameObject.SetActive(false);

            InventoryTable.transform.Find("InfosTable").gameObject.SetActive(false);
        }
        else
        {
            InventoryTable.transform.Find("RightTable").gameObject.SetActive(false);
            InventoryTable.transform.Find("MiddleTable").gameObject.SetActive(false);
            InventoryTable.transform.Find("CraftingTable").gameObject.SetActive(false);
            InventoryTable.transform.Find("RecipeTable").gameObject.SetActive(false);
            InventoryTable.transform.Find("BuildingTable").gameObject.SetActive(false);
            InventoryTable.transform.Find("SwitchButtons").gameObject.SetActive(false);

            InventoryTable.transform.Find("InfosTable").gameObject.SetActive(true);
        }
    }

}
