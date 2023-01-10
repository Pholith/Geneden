using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[ExecuteAlways] // Permet d'appeler certaines fonction dans le mode d'�dition pour actualiser les slots
public partial class BatimentSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    public BuildingsScriptableObject selectedBuildingDescriptor;
    public GameObject buildingViewer;
    private TextMeshProUGUI title, description, health, timeRequired, level,foodNumber,woodNumber,stoneNumber,ironNumber,silverNumber,goldNumber;
    private Image batIcon;

    [SerializeField]
    private GameObject uiCase;

    private Image itemIcon;

    private bool building_clicked = false;
    private GameObject buildingSelected;
    private SpriteRenderer spriteRenderer;

    //PreviewTile
    private GridManager gridManager;
    private BuildingManager buildingManager;
    private GameObject previewTile;
    private GameObject previewArea;
    
    private void Start()
    {
        //BuildingViewer
        title = buildingViewer.transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>();
        description = buildingViewer.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();
        health = buildingViewer.transform.Find("Health").gameObject.GetComponent<TextMeshProUGUI>();
        timeRequired = buildingViewer.transform.Find("BuildingTime").gameObject.GetComponent<TextMeshProUGUI>();
        level = buildingViewer.transform.Find("Civilisation").gameObject.GetComponent<TextMeshProUGUI>();
        batIcon = buildingViewer.transform.Find("Icon").gameObject.GetComponent<Image>();
        foodNumber = buildingViewer.transform.Find("FoodStat").gameObject.GetComponent<TextMeshProUGUI>();
        woodNumber = buildingViewer.transform.Find("WoodStat").gameObject.GetComponent<TextMeshProUGUI>();
        stoneNumber = buildingViewer.transform.Find("StoneStat").gameObject.GetComponent<TextMeshProUGUI>();
        ironNumber = buildingViewer.transform.Find("IronStat").gameObject.GetComponent<TextMeshProUGUI>();
        silverNumber = buildingViewer.transform.Find("SilverStat").gameObject.GetComponent<TextMeshProUGUI>();
        goldNumber = buildingViewer.transform.Find("GoldStat").gameObject.GetComponent<TextMeshProUGUI>();

        //Slot
        uiCase = transform.Find("Case").transform.gameObject;
        itemIcon = uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        itemIcon.sprite = selectedBuildingDescriptor.Sprite;

        //Preview
        gridManager = FindObjectOfType<GridManager>();
        buildingManager = FindObjectOfType<BuildingManager>();
        building_clicked = false;
        buildingSelected = new GameObject();
        spriteRenderer = buildingSelected.AddComponent<SpriteRenderer>();

    }

    private void Update()
    {
        SetBuildableFeedBack();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        UpdateBatView();
    }

    private void UpdateBatView()
    {
        title.SetText(selectedBuildingDescriptor.name);
        description.SetText(selectedBuildingDescriptor.BuildingDescription);
        health.SetText("Santé : " + (selectedBuildingDescriptor.MaxHealth).ToString());
        timeRequired.SetText("Temps construction : " + (selectedBuildingDescriptor.BuildingTime).ToString());
        level.SetText("Niveau recquis : " + (selectedBuildingDescriptor.RequiredCivilisationLvl).ToString());
        foodNumber.SetText(selectedBuildingDescriptor.FoodCost.ToString());
        woodNumber.SetText(selectedBuildingDescriptor.WoodCost.ToString());
        stoneNumber.SetText(selectedBuildingDescriptor.RockCost.ToString());
        ironNumber.SetText(selectedBuildingDescriptor.IronCost.ToString());
        silverNumber.SetText(selectedBuildingDescriptor.SilverCost.ToString());
        goldNumber.SetText(selectedBuildingDescriptor.GoldCost.ToString());
        batIcon.sprite = itemIcon.sprite;
        buildingViewer.SetActive(true);
    }


    private bool IsBuildable()
    {
        Dictionary<ResourceManager.RessourceType, int> ressourcesForBuild = new Dictionary<ResourceManager.RessourceType, int>();
        ressourcesForBuild.Add(ResourceManager.RessourceType.Food, selectedBuildingDescriptor.FoodCost);
        ressourcesForBuild.Add(ResourceManager.RessourceType.Wood, selectedBuildingDescriptor.WoodCost);
        ressourcesForBuild.Add(ResourceManager.RessourceType.Stone, selectedBuildingDescriptor.RockCost);
        ressourcesForBuild.Add(ResourceManager.RessourceType.Iron, selectedBuildingDescriptor.IronCost);
        ressourcesForBuild.Add(ResourceManager.RessourceType.Silver, selectedBuildingDescriptor.SilverCost);
        ressourcesForBuild.Add(ResourceManager.RessourceType.Gold, selectedBuildingDescriptor.GoldCost);

        if(!ResourceManager.Instance.HasEnoughRessource(ResourceManager.RessourceType.CivLevel,selectedBuildingDescriptor.RequiredCivilisationLvl))
        {
            return false;
        }

        foreach(ResourceManager.RessourceType type in ressourcesForBuild.Keys)
        {
            if(!ResourceManager.Instance.HasEnoughRessource(type, ressourcesForBuild[type]))
            {
                return false;
            }
        }

        return true;
    }

    private void SetBuildableFeedBack()
    {
        if(IsBuildable())
            SetCaseColor(new Color32(255, 255, 255, 255));
        else
            SetCaseColor(new Color32(106, 106, 106, 194));
    }

    private void SetCaseColor(Color32 color)
    {
        transform.Find("Case").gameObject.GetComponent<Image>().color = color;
        transform.Find("Case").transform.Find("ItemIcon").GetComponent<Image>().color = color;
    }

    private bool isEconomicBuilding()
    {
        return selectedBuildingDescriptor.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Economic);
    }

    private void CreatePreview()
    {
        previewTile = Instantiate(gridManager.previewTilePrefab);
        previewTile.transform.localScale = new Vector3(gridManager.MainGameGrid.cellSize.x * 2, gridManager.MainGameGrid.cellSize.y * 2, 0);
        if (isEconomicBuilding())
        {
            previewArea = Instantiate(buildingManager.buildingRangePrefab);
            previewArea.transform.SetParent(previewTile.transform);
            previewArea.transform.localPosition += new Vector3(0.5f, 0.5f, 0);
            foreach (BuildingsScriptableObject building in buildingManager.buildingsScriptableObjects)
            {
                if (building.name == selectedBuildingDescriptor.name)
                {
                    GatheringBuildingScript _gatheringBuilding = (GatheringBuildingScript)building;
                    previewArea.transform.localScale = new Vector3((_gatheringBuilding.gatheringRange /2) * gridManager.MainGameGrid.cellSize.x, (_gatheringBuilding.gatheringRange/2) * gridManager.MainGameGrid.cellSize.y, 1);
                }
            }
        }
        
    }

    public void OnPointerExit(PointerEventData eventData) {
        buildingViewer.SetActive(false);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if(IsBuildable())
        {
            Debug.Log("Building selected : " + selectedBuildingDescriptor.name);
            building_clicked = true;
            CreatePreview();
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        //Debug.Log(building_clicked);
        if (building_clicked)
        {
            spriteRenderer.gameObject.SetActive(true);
            previewTile.SetActive(true);
            spriteRenderer.sprite = selectedBuildingDescriptor.Sprite;
            if (spriteRenderer.sprite == null) return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
            previewTile.transform.position = gridManager.GetMouseGridPos();
            buildingSelected.transform.position = gridManager.GetMouseGridPos();
            buildingSelected.SetActive(true);
            /*if (GridManager.Instance.IsThereResourceOnTileSquare(GridManager.Instance.GetIntMouseGridPos(), 1))
            {
                spriteRenderer.color = new Color32(255, 0, 0, 101); ;
                previewTile.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 101);
            }
            else
            {
                spriteRenderer.color = new Color32(255,255,255,255);
                previewTile.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 101);
            }*/
                
        }

        

        if (eventData.pointerCurrentRaycast.gameObject.name != "GameplayBoard")
        {
            spriteRenderer.gameObject.SetActive(false);
            previewTile.SetActive(false);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Destroy(previewTile);
        if (isEconomicBuilding())
            Destroy(previewArea);
        spriteRenderer.sprite = null;
        building_clicked = false;
        buildingSelected.SetActive(false);

        GameObject dropped = eventData.pointerCurrentRaycast.gameObject;
        if (dropped.name == "GameplayBoard")
            BuildingManager.Instance.BuildBuilding(selectedBuildingDescriptor);
    }
}
