using UnityEngine;
using MapId = MapManager.MapId;

public class UnitRemoveCondition : MonoBehaviour, IButtonCondition
{
    private DialogController _dialogController;
    private TileManager _tileManager;

    private void Start()
    {
        _tileManager = TileManager.Instance;
    }

    public bool CanInteract()
    {
        if (_tileManager.selectedTile == null) return false;

        if (_tileManager.GetSelectedTileMapId() == MapId.Empty) return false;

        if (_tileManager.GetSelectedTileMapId() == MapId.Headquarter) return false;

        return true;
    }
}
