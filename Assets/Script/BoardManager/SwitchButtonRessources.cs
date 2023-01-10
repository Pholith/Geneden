using UnityEngine;
using UnityEngine.UI;

public class SwitchButtonRessources : MonoBehaviour
{
    public GameObject buildingScrollList;
    public GameObject ressourcesScrollList;
    
    public void OnClickEvent() {
        if (buildingScrollList != null && ressourcesScrollList != null) {
            buildingScrollList.SetActive(false);
            ressourcesScrollList.SetActive(true);
        }
    }
}
