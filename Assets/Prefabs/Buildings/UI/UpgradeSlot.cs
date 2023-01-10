using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    private ToolTipTrigger toolTipTrigger;
    private UpgradesScriptableObject Upgrade;
    private BuildingGeneric SelectedBuilding;

    private GameObject UpgradeView;

    private void Start()
    {
        //Tooltip
        toolTipTrigger = gameObject.GetComponent<ToolTipTrigger>();
        UpgradeView = FindObjectOfType<BuildingInfosTable>(true).transform.Find("UpgradeTable").transform.Find("UpgradeView").gameObject;
    }

    private void Update()
    {
        UpdateToolTip();
        CheckSearchability();
    }

    private void CheckSearchability()
    {
        if (Upgrade.IsBuildingLevelUpUpgrade())
        {
            if(PlayerManager.Instance.IsIndividualUpgradeSearchable(Upgrade) && !SelectedBuilding.IsSearchingUpgrade(Upgrade))
            {
                SetCaseColor(new Color32(255, 255, 255, 255));
                return;
            }
            else
            {
                SetCaseColor(new Color32(106, 106, 106, 194));
                return;
            }
                
        }
        if (PlayerManager.Instance.IsUpgradeUnlocked(Upgrade))
            FindObjectOfType<BuildingInfosTable>(true).UpdateUpgradesUI();
        if (PlayerManager.Instance.IsUpgradeAlreadySearched(Upgrade))
            SetCaseColor(new Color32(106, 106, 106, 194));
        else
        {
            if (PlayerManager.Instance.IsUpgradeSearchable(Upgrade))
                SetCaseColor(new Color32(255, 255, 255, 255));
            else
                SetCaseColor(new Color32(106, 106, 106, 194));
        }
    }

    private void SetCaseColor(Color32 color)
    {
        transform.Find("Case").gameObject.GetComponent<Image>().color = color;
        transform.Find("Case").transform.Find("ItemIcon").GetComponent<Image>().color = color;
    }

    private void UpdateToolTip()
    {
        toolTipTrigger.SetHeader(Upgrade.name);
        toolTipTrigger.SetContent(Upgrade.UpgradeDescription);
    }

    private void ShowUpgradeView()
    {
        UpgradeView.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = Upgrade.name;
        UpgradeView.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = Upgrade.UpgradeDescription;
        UpgradeView.transform.Find("SearchingTime").GetComponent<TextMeshProUGUI>().text = "Temps de Recherche : " + TimeSpan.FromSeconds(Upgrade.ResearchTime).ToString(@"mm\:ss") + "s";
        UpgradeView.transform.Find("Civilisation").GetComponent<TextMeshProUGUI>().text = "Niveau recquis : " + Upgrade.RequiredCivilisationLvl.ToString();
        UpgradeView.transform.Find("Icon").GetComponent<Image>().sprite = Upgrade.Sprite;

        UpgradeView.transform.Find("FoodStat").GetComponent<TextMeshProUGUI>().text = Upgrade.FoodCost.ToString();
        UpgradeView.transform.Find("WoodStat").GetComponent<TextMeshProUGUI>().text = Upgrade.WoodCost.ToString();
        UpgradeView.transform.Find("StoneStat").GetComponent<TextMeshProUGUI>().text = Upgrade.RockCost.ToString();
        UpgradeView.transform.Find("IronStat").GetComponent<TextMeshProUGUI>().text = Upgrade.IronCost.ToString();
        UpgradeView.transform.Find("SilverStat").GetComponent<TextMeshProUGUI>().text = Upgrade.SilverCost.ToString();
        UpgradeView.transform.Find("GoldStat").GetComponent<TextMeshProUGUI>().text = Upgrade.GoldCost.ToString();
        UpgradeView.SetActive(true);
    }

    private void HideUpgradeView()
    {
        UpgradeView.SetActive(false);
    }

    public void SetUpgrade(UpgradesScriptableObject upgrade, BuildingGeneric building)
    {
        Upgrade = upgrade;
        SelectedBuilding = building;
        transform.Find("Case").transform.Find("ItemIcon").GetComponent<Image>().sprite = Upgrade.Sprite;
    }

    public UpgradesScriptableObject GetUpgrade()
    {
        return Upgrade;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CheckLevelingUpUpgrade())
            return;
        if (!SelectedBuilding.IsSearching())
        {
            if (PlayerManager.Instance.IsUpgradeSearchable(Upgrade))
            {
                SelectedBuilding.SearchUpgrade(Upgrade);
                FindObjectOfType<BuildingInfosTable>(true).UpdateSearchingIcon();
            }
        }
        else if ((SelectedBuilding.CanAddUpgradeToPendingList()) && !PlayerManager.Instance.IsUpgradeAlreadySearched(Upgrade))
        {
            if (PlayerManager.Instance.IsUpgradeSearchable(Upgrade))
            {
                PlayerManager.Instance.StartUpgrade(Upgrade);
                SelectedBuilding.AddUpgradeToPendingList(Upgrade);
                FindObjectOfType<BuildingInfosTable>(true).UpdateSearchingPendingList();
            }
        }
    }

    private bool CheckLevelingUpUpgrade()
    {
        if (Upgrade.type.Contains(UpgradesScriptableObject.UpgradeType.LevelUpBuilding))
        {
            if (!SelectedBuilding.IsSearching())
            {
                if (PlayerManager.Instance.IsIndividualUpgradeSearchable(Upgrade))
                {
                    SelectedBuilding.SearchUpgrade(Upgrade);
                    FindObjectOfType<BuildingInfosTable>(true).UpdateSearchingIcon();
                    return true;
                }
            }
            else if ((SelectedBuilding.CanAddUpgradeToPendingList()) && !SelectedBuilding.IsSearchingUpgrade(Upgrade))
            {
                if (PlayerManager.Instance.IsIndividualUpgradeSearchable(Upgrade))
                {
                    PlayerManager.Instance.StartUpgrade(Upgrade);
                    SelectedBuilding.AddUpgradeToPendingList(Upgrade);
                    FindObjectOfType<BuildingInfosTable>(true).UpdateSearchingPendingList();
                    return true;
                }
            }

        }
        return false;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowUpgradeView();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideUpgradeView();
    }




}