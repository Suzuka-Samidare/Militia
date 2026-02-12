using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public BaseUnitData unitData;

    private MapManager _mapManager;
    private TileManager _tileManager;

    void Start()
    {
        _mapManager = MapManager.Instance;
        _tileManager = TileManager.Instance;
    }

    public void Onclick()
    {
        int centerY = _tileManager.selectedTileController.gridPosY;
        int centerX = _tileManager.selectedTileController.gridPosX;

        TileRangeUtil.ForEachManhattanRange(centerY, centerX, 3, (y, x) => {
            if (y >= 0 && y < _mapManager.mapHeight && x >= 0 && x < _mapManager.mapWidth)
            {
                _mapManager.playerMapData[y, x].SpawnUnitDelayed(unitData);
            }
        });
    }
}
