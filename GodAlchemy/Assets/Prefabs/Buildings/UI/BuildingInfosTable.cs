using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static BuildingsScriptableObject;

public class BuildingInfosTable : MonoBehaviour
{
    //Manager
    [SerializeField]
    private BuildingManager buildingManager;
    [SerializeField]
    public GameObject tagIconPrefab;
    [SerializeField]
    public GameObject workerIconPrefab;
    [SerializeField]
    public GameObject upgradeSlotPrefab;
    [SerializeField]
    public GameObject researchIconPrefab;
    [SerializeField]
    private BuildingGeneric selectedBuilding;


    //UI Elements Infos
    [SerializeField]
    private GameObject BuildingInfosPanel;
    [SerializeField]
    private Image BuildingIcon;
    [SerializeField]
    private TextMeshProUGUI CurrentHealth, MaxHealth;
    [SerializeField]
    private Image HealthFillBar;
    [SerializeField]
    private GameObject Tags;

    //UI Elements Upgrades
    [SerializeField]
    private GameObject BuildingUpgradesPanel;

    //UI Elements Gathering
    [SerializeField]
    private GameObject BuildingGatheringPanel;


    // Start is called before the first frame update
    private void Awake()
    {
        //Managers & UI panel
        buildingManager = FindObjectOfType<BuildingManager>(true);
        selectedBuilding = null;

        BuildingInfosPanel = transform.Find("BuildingInfo").transform.gameObject;
        BuildingUpgradesPanel = transform.Find("UpgradeTable").transform.gameObject;
        BuildingGatheringPanel = transform.Find("AsignationTable").transform.gameObject;

        //UI Elements
        BuildingIcon = BuildingInfosPanel.transform.Find("BuildingIcon").transform.Find("ItemIcon").gameObject.GetComponent<Image>();
        CurrentHealth = BuildingInfosPanel.transform.Find("Health").transform.Find("CurrentHealth").gameObject.GetComponent<TextMeshProUGUI>();
        MaxHealth = BuildingInfosPanel.transform.Find("Health").transform.Find("MaxHealth").gameObject.GetComponent<TextMeshProUGUI>();
        HealthFillBar = BuildingInfosPanel.transform.Find("HealthBar").transform.Find("FillBar").gameObject.GetComponent<Image>();
        Tags = BuildingInfosPanel.transform.Find("TagsInfo").gameObject;
    }

    private void Start()
    {


    }

    // Update is called once per frame
    private void Update()
    {
        if (selectedBuilding != null)
        {
            CheckHealth();
            CheckGathering();
            CheckSearching();
        }
            
    }

    public void ObjectSelected(SelectableObject selectedObject)
    {
        SelectableObject.ObjectType objectType = selectedObject.GetObjectType();
        switch (objectType)
        {
            case SelectableObject.ObjectType.Building:
                BuildingSelected(selectedObject.GetComponentInParent<BuildingGeneric>());
                break;
        }
    }

    public void ObjectUnSelected(SelectableObject selectedObject)
    {
        SelectableObject.ObjectType objectType = selectedObject.GetObjectType();
        switch (objectType)
        {
            case SelectableObject.ObjectType.Building:
                BuildingUnselected();
                break;
        }
    }

    public void BuildingSelected(BuildingGeneric building)
    {
        selectedBuilding = building;
        SetInfos();
        if(selectedBuilding.buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Gathering))
        {
            UpdateWorkerUI();
            UpdateSearchingIcon();
            FindObjectOfType<GameUI>().ShowGatheringUI(true);
            selectedBuilding.GetComponent<GatheringBuildings>().ShowRange(true);
        }
       
    }

    public void BuildingUnselected()
    {
        GameUI _gameUI = FindObjectOfType<GameUI>();
        _gameUI.ShowElementUI(true);
        _gameUI.ShowBuildingUI(false);
        if (selectedBuilding.buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Gathering))
        {
            FindObjectOfType<GameUI>().ShowGatheringUI(false);
            selectedBuilding.GetComponent<GatheringBuildings>().ShowRange(false);
        }
        selectedBuilding = null;
    }

    public void SetInfos()
    {
        BuildingIcon.sprite = selectedBuilding.buildingScriptObj.Sprite;
        GameUI _gameUI = FindObjectOfType<GameUI>();
        _gameUI.ShowElementUI(false);
        _gameUI.ShowBuildingUI(true);
        FindObjectOfType<GameUI>().ShowGatheringUI(false);
        UpdateUpgradesUI();
        

        CheckHealth();
        DestroyContentUI(BuildingInfosPanel.transform.Find("TagsInfo").transform.Find("TagList").gameObject);
        CreateTagUI();
    }

    private void CheckHealth()
    {
        if (selectedBuilding.GetHp() > 0)
        {
            SetHealthText();
            SetHealthBar();
        }
        else
            selectedBuilding = null;
    }

    private void SetHealthBar()
    {
        HealthFillBar.fillAmount = ((float)selectedBuilding.GetHp()) / ((float)selectedBuilding.buildingScriptObj.MaxHealth);
    }

    private void SetHealthText()
    {
        CurrentHealth.text = selectedBuilding.GetHp().ToString();
        MaxHealth.text = selectedBuilding.buildingScriptObj.MaxHealth.ToString();
    }

    public void AddWorker()
    {
        if(ResourceManager.Instance.HasEnoughPop(1))
        {
            selectedBuilding.GetComponent<GatheringBuildings>().AddWorker(1);
            UpdateWorkerUI();
        }  
    }

    public void RemoveWorker()
    {
        selectedBuilding.GetComponent<GatheringBuildings>().RemoveWorker(1);
        UpdateWorkerUI();
    }

    private void UpdateWorkerUI()
    {
        int workers = selectedBuilding.GetComponent<GatheringBuildings>().GetWorker();
        int maxWorkers = selectedBuilding.GetComponent<GatheringBuildings>().GetMaxWorker();
        Debug.Log(workers);
        Debug.Log(maxWorkers);
        int _xPos = -60;
        int _yPos = 0;

        GameObject _contentPanel = BuildingGatheringPanel.transform.Find("WorkerList").gameObject;
        DestroyContentUI(_contentPanel);
        for (int i = 1;i<6;i++)
        {
            GameObject _workerIcon = Instantiate(workerIconPrefab);
            _workerIcon.transform.SetParent(_contentPanel.transform);
            _workerIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(_xPos, _yPos, 0f);
            _workerIcon.GetComponent<RectTransform>().localScale = _workerIcon.GetComponent<RectTransform>().localScale * 2;
            if (i <= workers)
            {
                _workerIcon.GetComponent<Image>().color = new Color32(255,255,255,255);
                _workerIcon.transform.Find("Locked").gameObject.SetActive(false);
            }
            else if(i <= maxWorkers)
            {
                _workerIcon.GetComponent<Image>().color = new Color32(106, 106, 106, 194);
                _workerIcon.transform.Find("Locked").gameObject.SetActive(false);
            }
            else if(i > maxWorkers)
            {
                _workerIcon.GetComponent<Image>().color = new Color32(106, 106, 106, 194);
                _workerIcon.transform.Find("Locked").gameObject.SetActive(true);
            }
            _xPos += 30;
        }

    }

    public void UpdateUpgradesUI()
    {
        int _i = 0;
        int _xPos = -91;
        int _yPos = 355;
        

        GameObject _contentPanel = BuildingUpgradesPanel.transform.Find("ScrollArea").gameObject.transform.Find("Content").gameObject;
        DestroyContentUI(_contentPanel);
        foreach (UpgradesScriptableObject upgrade in selectedBuilding.buildingScriptObj.UpgradeList)
        {
            Debug.Log(upgrade.name);
            UpgradesScriptableObject upgradeToShow = PlayerManager.Instance.GetLastTierUpgrade(upgrade);
            if(upgradeToShow != null)
            {
                GameObject _upgradeSlot = Instantiate(upgradeSlotPrefab);
                _upgradeSlot.GetComponent<UpgradeSlot>().SetUpgrade(upgradeToShow, selectedBuilding);
                _upgradeSlot.transform.SetParent(_contentPanel.transform);
                _upgradeSlot.GetComponent<RectTransform>().anchoredPosition = new Vector3(_xPos, _yPos, 0f);
                _upgradeSlot.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            }
            _xPos += 35;
            _i += 1;
            if (_i % 3 == 0)
            {
                _xPos = -91;
                _yPos -= 40;
                _i = 0;
            }
        }
    }

    private void CheckGathering()
    {
        if (selectedBuilding.buildingScriptObj.BuildingTags.Contains(BuildingsScriptableObject.BuildingType.Gathering))
        {
            UpdateGatheringUI();
        }
    }

    private void CheckSearching()
    {
        UpdateSearchingUI();
    }

    private void UpdateGatheringUI()
    {
        Image _gatheringBar = BuildingGatheringPanel.transform.Find("GatheringBar").transform.Find("FillBar").gameObject.GetComponent<Image>();
        if (selectedBuilding.GetComponent<GatheringBuildings>().GetTimer().IsActive())
        {
           _gatheringBar.fillAmount = ((float)selectedBuilding.GetComponent<GatheringBuildings>().GetTimer().GetCurrentTime() / ((float)selectedBuilding.GetComponent<GatheringBuildings>().GetTimer().endTime));
        }
        else
        {
            _gatheringBar.fillAmount = 0.0f;
        }
        
        
    }

    private void UpdateSearchingUI()
    {
        GameObject ResearchPanel = BuildingGatheringPanel.transform.Find("ResearchPanel").gameObject;
        
        Image _searchingBar = ResearchPanel.transform.Find("ResearchBar").transform.Find("FillBar").gameObject.GetComponent<Image>();
        TextMeshProUGUI _searchingTimerText = ResearchPanel.transform.Find("ResearchBar").transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();
        if (selectedBuilding.GetComponent<BuildingGeneric>().IsSearching())
        {
            ResearchPanel.transform.gameObject.SetActive(true);
            _searchingBar.fillAmount = ((float)selectedBuilding.GetComponent<BuildingGeneric>().GetTimer().GetCurrentTime() / ((float)selectedBuilding.GetComponent<BuildingGeneric>().GetTimer().endTime));
            TimeSpan time = TimeSpan.FromSeconds((double)selectedBuilding.GetComponent<BuildingGeneric>().GetTimer().GetCurrentTime());
            _searchingTimerText.text = time.ToString(@"mm\:ss");
        }
        else
        {
            ResearchPanel.transform.gameObject.SetActive(false);
        }
    }

    public void UpdateSearchingIcon()
    {
        if(selectedBuilding.IsSearching())
        {
            GameObject ResearchPanel = BuildingGatheringPanel.transform.Find("ResearchPanel").gameObject;
            DestroyContentUI(ResearchPanel.transform.Find("IconPanel").gameObject);
            GameObject _researchIcon = Instantiate(researchIconPrefab);
            _researchIcon.GetComponent<ResearchIcon>().SetIcon(selectedBuilding);
            _researchIcon.transform.SetParent(ResearchPanel.transform.Find("IconPanel").transform);
            _researchIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-13.5f, 13.5f, 0f);
            _researchIcon.GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.75f, 1.0f);
        }  
    }

    private void DestroyContentUI(GameObject UIPanel)
    {
        foreach (Transform child in UIPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void CreateTagUI()
    {
        int _i = 0;
        int _xPos = -15;
        int _yPos = 0;

        GameObject _contentPanel = BuildingInfosPanel.transform.Find("TagsInfo").transform.Find("TagList").gameObject;
        foreach (BuildingsScriptableObject.BuildingType buildingType in selectedBuilding.buildingScriptObj.BuildingTags)
        {
            foreach(TagScriptableObject tag in buildingManager.tagsSpriteList)
            {
                if(tag.TagType == buildingType)
                {
                    GameObject _tagIcon = Instantiate(tagIconPrefab);
                    _tagIcon.GetComponent<Image>().sprite = tag.Sprite;
                    _tagIcon.transform.SetParent(_contentPanel.transform);
                    _tagIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(_xPos, _yPos, 0f);
                    _tagIcon.GetComponent<Tag>().SetName(buildingType.ToString());
                }
            }
            

            _xPos += 15;
            _i += 1;
            if (_i % 4 == 0)
                return;
        }
    }
}
