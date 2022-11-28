using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[ExecuteAlways] // Permet d'appeler certaines fonction dans le mode d'�dition pour actualiser les slots
public partial class BatimentSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    public BuildingsScriptableObject selectedBuildingDescriptor;
    public TextMeshProUGUI title, description, health, timeRequired, level;

    [SerializeField]
    private GameObject uiCase;

    private Image itemIcon;
    private TextMeshProUGUI costText;

    public GameObject batView;

    private bool mouse_over = false;

    private bool building_clicked = false;
    private GameObject buildingSelected;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        mouse_over = false;
        uiCase = transform.Find("Case").transform.gameObject;
        itemIcon = uiCase.transform.Find("ItemIcon").transform.gameObject.GetComponent<Image>();
        costText = uiCase.transform.Find("ItemAmount").transform.gameObject.GetComponent<TextMeshProUGUI>();

        building_clicked = false;
        buildingSelected = new GameObject();
        spriteRenderer = buildingSelected.AddComponent<SpriteRenderer>();

    }

    private void Update() {
        UpdateBatView();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouse_over = false;
    }

    public void UpdateBatView()
    {
        if (mouse_over)
        {
            title.SetText(selectedBuildingDescriptor.name);
            description.SetText(selectedBuildingDescriptor.BuildingDescription);
            health.SetText("Santé : " + (selectedBuildingDescriptor.MaxHealth).ToString());
            timeRequired.SetText("Temps construction : " + (selectedBuildingDescriptor.BuildingTime).ToString());
            level.SetText("Niveau recquis : " + (selectedBuildingDescriptor.RequiredCivilisationLvl).ToString());
            batView.SetActive(true);
        }
        else
        {
            batView.SetActive(false);
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Building selected : " + selectedBuildingDescriptor.name);
        building_clicked = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        //Debug.Log(building_clicked);
        if (building_clicked)
        {
            spriteRenderer.sprite = selectedBuildingDescriptor.Sprite;
            if (spriteRenderer.sprite == null) return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
            buildingSelected.transform.position = mousePosition;
            buildingSelected.SetActive(true);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        spriteRenderer.sprite = null;
        building_clicked = false;
        buildingSelected.SetActive(false);
        
        BuildingManager.Instance.BuildBuilding(selectedBuildingDescriptor);
    }
}
