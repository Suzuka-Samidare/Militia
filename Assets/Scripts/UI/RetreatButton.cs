using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatButton : BaseButton
{
    private MapManager _mapManager;
    private TileManager _tileManager;

    void Start()
    {
        _mapManager = MapManager.Instance;
        _tileManager = TileManager.Instance;
    }

    void Update()
    {
        CheckButtonInteractable();
    }
    
    void Onclick()
    {
        MapManager.Instance.UpdateSelectedTileOnUnitId(0);
        TileManager.Instance.ClearSelectedTileOnUnit();
    }

    private void CheckButtonInteractable()
    {
        if (_tileManager.selectedTile == null || _mapManager.GetSelectedTileId() == 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
