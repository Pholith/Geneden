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
        ShowBuildingUI(false);
        ShowElementUI(true);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void ShowBuildingUI(bool show)
    {
        InventoryTable.transform.Find("InfosTable").gameObject.SetActive(show);
        ShowAssignationUI(true);
    }

    public void ShowElementUI(bool show)
    {
        InventoryTable.transform.Find("RightTable").gameObject.SetActive(show);
        InventoryTable.transform.Find("MiddleTable").gameObject.SetActive(show);
        InventoryTable.transform.Find("CraftingTable").gameObject.SetActive(show);
        InventoryTable.transform.Find("RecipeTable").gameObject.SetActive(show);
        InventoryTable.transform.Find("BuildingTable").gameObject.SetActive(false);
        InventoryTable.transform.Find("SwitchButtons").gameObject.SetActive(show);
    }

    public void ShowAssignationUI(bool show)
    {
        InventoryTable.transform.Find("InfosTable").transform.Find("AsignationTable").gameObject.SetActive(show);
    }

    public void ShowGatheringUI(bool show)
    {
        InventoryTable.transform.Find("InfosTable").transform.Find("AsignationTable").transform.Find("GatheringPanel").gameObject.SetActive(show);
    }
}
