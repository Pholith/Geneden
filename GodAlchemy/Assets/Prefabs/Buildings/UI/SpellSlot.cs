using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    private ToolTipTrigger toolTipTrigger;
    private SpellScriptableObject Spell;
    private BuildingGeneric SelectedBuilding;

    private GameObject SpellView;

    private void Start()
    {
        //Tooltip
        toolTipTrigger = gameObject.GetComponent<ToolTipTrigger>();
        SpellView = FindObjectOfType<BuildingInfosTable>(true).transform.Find("UpgradeTable").transform.Find("UpgradeView").gameObject;
    }

    private void Update()
    {
        UpdateToolTip();
        CheckUsable();
    }

    private void CheckUsable()
    {
        if (PlayerManager.Instance.IsSpellUsable(Spell))
            SetCaseColor(new Color32(255, 255, 255, 255));
        else
            SetCaseColor(new Color32(106, 106, 106, 194));
    }

    private void SetCaseColor(Color32 color)
    {
        transform.Find("Case").gameObject.GetComponent<Image>().color = color;
        transform.Find("Case").transform.Find("ItemIcon").GetComponent<Image>().color = color;
    }

    private void UpdateToolTip()
    {
        toolTipTrigger.SetHeader(Spell.name);
        toolTipTrigger.SetContent(Spell.SpellDescription);
    }

    private void ShowSpellView()
    {
        SpellView.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = Spell.name;
        SpellView.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = Spell.SpellDescription;
        SpellView.transform.Find("SearchingTime").GetComponent<TextMeshProUGUI>().text = "";
        SpellView.transform.Find("Civilisation").GetComponent<TextMeshProUGUI>().text = "Niveau recquis : " + Spell.RequiredCivilisationLvl.ToString();
        SpellView.transform.Find("Icon").GetComponent<Image>().sprite = Spell.Sprite;

        SpellView.transform.Find("FoodStat").GetComponent<TextMeshProUGUI>().text = Spell.FoodCost.ToString();
        SpellView.transform.Find("WoodStat").GetComponent<TextMeshProUGUI>().text = Spell.WoodCost.ToString();
        SpellView.transform.Find("StoneStat").GetComponent<TextMeshProUGUI>().text = Spell.RockCost.ToString();
        SpellView.transform.Find("IronStat").GetComponent<TextMeshProUGUI>().text = Spell.IronCost.ToString();
        SpellView.transform.Find("SilverStat").GetComponent<TextMeshProUGUI>().text = Spell.SilverCost.ToString();
        SpellView.transform.Find("GoldStat").GetComponent<TextMeshProUGUI>().text = Spell.GoldCost.ToString();
        SpellView.SetActive(true);
    }

    private void HideSpellView()
    {
        SpellView.SetActive(false);
    }

    public void SetSpell(SpellScriptableObject spell, BuildingGeneric building)
    {
        Spell = spell;
        SelectedBuilding = building;
        transform.Find("Case").transform.Find("ItemIcon").GetComponent<Image>().sprite = spell.Sprite;
    }

    public SpellScriptableObject GetSpell()
    {
        return Spell;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerManager.Instance.IsSpellUsable(Spell))
            PlayerManager.Instance.UseSpell(Spell);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowSpellView();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideSpellView();
    }




}
