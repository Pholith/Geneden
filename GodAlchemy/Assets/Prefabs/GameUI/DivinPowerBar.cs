using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DivinPowerBar : MonoBehaviour
{
    [SerializeField]
    private Image costBar;
    [SerializeField]
    private Image fillBar2;

    [SerializeField]
    private TextMeshProUGUI topBarText;
    [SerializeField]
    private TextMeshProUGUI midBarText;

    private Color baseColor;

    [SerializeField]
    [ReadOnly]
    private int powerMax;
    public int PowerMax
    {
        get => powerMax;
        set
        {
            powerMax = value;
            UpdateUI();
        }
    }

    [SerializeField]
    private int currentPower;
    public int CurrentPower
    {
        get => currentPower;
        set
        {
            currentPower = Mathf.Min(value, powerMax);
            UpdateUI();
        }
    }

    private void Start()
    {
        baseColor = costBar.color;
    }

    private void Update()
    {
        UpdateUI();
    }
    private int showedCost = -1;

    public void ToggleShowCost(int cost = -1)
    {
        showedCost = cost;
    }
    private void UpdateUI()
    {
        if (showedCost != -1)
        {
            float percentOfFillWithoutCost = (currentPower - showedCost) / (float)powerMax;
            float percentOfCost = showedCost / (float)powerMax;
            costBar.color = Color.Lerp(costBar.color, Color.red, 0.1f);
            costBar.fillAmount = percentOfCost;
            fillBar2.fillAmount = percentOfFillWithoutCost;
            float heightFilled = percentOfFillWithoutCost * fillBar2.rectTransform.rect.height;
            float filledTop = heightFilled;
            costBar.rectTransform.anchoredPosition = fillBar2.rectTransform.anchoredPosition + new Vector2(0, filledTop);
        }
        else
        {
            float percentOfFill = currentPower / (float)powerMax;
            costBar.rectTransform.position = fillBar2.rectTransform.position;
            costBar.fillAmount = 0;
            costBar.color = fillBar2.color;

            if (percentOfFill > fillBar2.fillAmount) fillBar2.fillAmount = percentOfFill;
            else fillBar2.fillAmount = Mathf.Lerp(fillBar2.fillAmount, percentOfFill, 0.2f);
        }
        midBarText.text = (powerMax / 2).ToString();
        topBarText.text = powerMax.ToString();
    }
}
