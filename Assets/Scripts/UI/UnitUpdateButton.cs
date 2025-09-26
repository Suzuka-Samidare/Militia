using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpdateButton : BaseButton
{
    // public int unitId;
    public BaseUnitData baseUnitData;

    void Update()
    {
        CheckButtonInteractable();
    }

    public void Onclick()
    {
        MapManager.Instance.UpdateUnitOnMapData(baseUnitData.id);
        TileManager.Instance.SetSelectedTileOnUnit(baseUnitData);
    }

    private void CheckButtonInteractable()
    {
        if (TileManager.Instance.selectedTile != null)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
