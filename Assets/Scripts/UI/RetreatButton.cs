using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapId = MapManager.MapId;

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
    
    public void Onclick()
    {
        MapManager.Instance.UpdateSelectedTileOnUnitId(MapId.Empty);
        TileManager.Instance.ClearSelectedTileOnUnit();
    }

    private void CheckButtonInteractable()
    {
        if (GameManager.Instance.isMainViewDisabled
            || _tileManager.selectedTile == null
            || _mapManager.GetSelectedTileId() == MapId.Empty
            || _mapManager.GetSelectedTileId() == MapId.Headquarter)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
