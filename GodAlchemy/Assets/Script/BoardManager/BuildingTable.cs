using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTable : MonoBehaviour
{
    [SerializeField]
    private GameObject buildingSlotPrefab;
    [SerializeField]
    private GameObject batView;
    // Start is called before the first frame update
    void Start()
    {
        batView = transform.Find("BatView").gameObject;
        CreateBuildingList();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBuildingList()
    {
        int _i = 0;
        int _xPos = -91;
        int _yPos = 355;

        BatimentSlot _buildingSlotScript = buildingSlotPrefab.GetComponent<BatimentSlot>();
        GameObject _contentPanel = transform.Find("ScrollArea").gameObject.transform.Find("Content").gameObject;
        foreach (BuildingsScriptableObject building in GameManager.BuildingManager.buildingsScriptableObjects)
        {
            if(!building.isBuildable)
            {
                continue;
            }
            _buildingSlotScript.buildingViewer = batView;
            _buildingSlotScript.selectedBuildingDescriptor = building;
            GameObject _buildingSlot = Instantiate(buildingSlotPrefab);
            _buildingSlot.transform.SetParent(_contentPanel.transform);
            _buildingSlot.GetComponent<RectTransform>().anchoredPosition = new Vector3(_xPos, _yPos, 0f);
            _buildingSlot.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            _xPos += 35;
            _i += 1;
            if (_i % 3 == 0)
            {
                _xPos = -91;
                _yPos -= 40;
                _i = 0;
            }
        }
        //batView.SetActive(false);
    }
}
