using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public GridPlacement gridPlacement;

    public GameObject singleRoad;
    public GameObject streetLightDouble;
    public GameObject streetLightSingle;
    public GameObject treeModel;
    public GameObject bushModel;
    public GameObject optionObject;
    public void SelectSingleRoad()
    {
        gridPlacement.DisableDeleteMode();
        gridPlacement.StartBuilding(singleRoad, new Vector3(-90, 0, 0));
    }

    public void SelectStreetLightDouble()
    {
        gridPlacement.DisableDeleteMode();
        gridPlacement.StartBuilding(streetLightDouble, new Vector3(-90, 180, 0));
    }

    public void SelectStreetLightTriple()
    {
        gridPlacement.DisableDeleteMode();
        gridPlacement.StartBuilding(streetLightSingle, new Vector3(-90, 90, 0));
    }
    
    public void SelectTreeModel()
    {
        gridPlacement.DisableDeleteMode();
        gridPlacement.StartBuilding(treeModel, new Vector3(0, 0, 0));
    }
    
    public void SelectBushModel()
    {
        gridPlacement.DisableDeleteMode();
        gridPlacement.StartBuilding(bushModel, new Vector3(0, 0, 0));
    }
    public void TogglePanel()
    {
        optionObject.SetActive(!optionObject.activeSelf);
    }
    public void ClosePanel()
    {
        if (optionObject != null)
        {
            optionObject.SetActive(!optionObject.activeSelf);
        }
    }
}