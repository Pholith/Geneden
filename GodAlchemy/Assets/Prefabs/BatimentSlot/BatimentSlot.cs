using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[ExecuteAlways] // Permet d'appeler certaines fonction dans le mode d'�dition pour actualiser les slots
public partial class BatimentSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public HouseScriptableObject House;
    public TextMeshProUGUI title, description, health, timeRequired, level;

    [SerializeField]
    private GameObject uiCase;

    private Image itemIcon;
    private TextMeshProUGUI costText;

    public GameObject batView;

    private bool mouse_over = false;

    private void Start()
    {
        mouse_over = false;
        uiCase = transform.Find("Case").transform.gameObject;
        itemIcon = uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        costText = uiCase.transform.Find("ItemAmount").transform.gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        if (mouse_over) {
            title.SetText(House.name);
            description.SetText(House.BuildingDescription);
            health.SetText("Santé : "+(House.MaxHealth).ToString());
            timeRequired.SetText("Temps construction : "+(House.BuildingTime).ToString());
            level.SetText("Niveau recquis : "+(House.RequiredCivilisationLvl).ToString());
            batView.SetActive(true);
        } else {
            batView.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouse_over = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouse_over = false;
        Debug.Log("Mouse exit");
    }
}
