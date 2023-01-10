using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PendingSlot : MonoBehaviour,IPointerClickHandler
{

    // Start is called before the first frame update

    private ToolTipTrigger toolTipTrigger;
    private UpgradesScriptableObject Upgrade;
    private BuildingGeneric SelectedBuilding;

    void Start()
    {
        //Tooltip
        toolTipTrigger = gameObject.GetComponent<ToolTipTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateToolTip();
    }

    private void UpdateToolTip()
    {
        toolTipTrigger.SetHeader(Upgrade.name);
        toolTipTrigger.SetContent(Upgrade.UpgradeDescription);
    }

    public void SetUpgrade(UpgradesScriptableObject upgrade, BuildingGeneric building)
    {
        Upgrade = upgrade;
        SelectedBuilding = building;
        transform.Find("Case").transform.Find("ItemIcon").GetComponent<Image>().sprite = Upgrade.Sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        toolTipTrigger.StopDelay();
        PlayerManager.Instance.StopUpgrade(Upgrade);
        SelectedBuilding.RemoveUpgradeToPendingList(Upgrade);
        FindObjectOfType<BuildingInfosTable>(true).UpdateSearchingPendingList();
    }

}
