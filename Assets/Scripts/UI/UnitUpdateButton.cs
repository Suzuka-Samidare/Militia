using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpdateButton : BaseButton
{
    public BaseUnitData unitData;

    void Update()
    {
        CheckButtonInteractable();
    }

    public void Onclick()
    {
        TileManager.Instance.SetSelectedTileOnUnit(unitData);
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
