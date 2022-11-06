using UnityEngine;
using UnityEngine.UI;

public class SwitchButtonBuilding : MonoBehaviour
{
    public GameObject buildingScrollList;
    public GameObject ressourcesScrollList;
    
    public void OnClickEvent() {
        if (buildingScrollList != null && ressourcesScrollList != null) {
            buildingScrollList.SetActive(true);
            ressourcesScrollList.SetActive(false);
        }
    }
}
