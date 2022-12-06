using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfosTable : MonoBehaviour
{
    //Manager
    [SerializeField]
    private BuildingManager buildingManager;
    public GameObject tagIconPrefab;
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


    // Start is called before the first frame update
    private void Awake()
    {
        //Managers & UI panel
        buildingManager = FindObjectOfType<BuildingManager>(true);
        selectedBuilding = null;

        BuildingInfosPanel = transform.Find("BuildingInfo").transform.gameObject;
        BuildingUpgradesPanel = transform.Find("UpgradeTable").transform.gameObject;

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
            CheckHealth();
    }

    public void BuildingClicked()
    {
        if (buildingManager.selectedBuilding == null)
            BuildingUnselected();
        else
            SetInfos();
    }

    public void BuildingUnselected()
    {
        selectedBuilding = null;
        FindObjectOfType<GameUI>().ManageUI();
    }

    public void SetInfos()
    {
        selectedBuilding = buildingManager.selectedBuilding.GetComponent<BuildingGeneric>();
        BuildingIcon.sprite = selectedBuilding.buildingScriptObj.Sprite;
        FindObjectOfType<GameUI>().ManageUI();
        CheckHealth();
        DestroyTagUI();
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
        {
            selectedBuilding = null;
            buildingManager.selectedBuilding = null;
        }

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

    private void DestroyTagUI()
    {
        GameObject _contentPanel = BuildingInfosPanel.transform.Find("TagsInfo").transform.Find("TagList").gameObject;
        foreach (Transform child in _contentPanel.transform)
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
