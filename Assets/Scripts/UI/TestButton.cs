using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using MapId = MapManager.MapId;

public class TestButton : BaseButton
{
    public BaseUnitData unitData;

    private MapManager _mapManager;
    private TileManager _tileManager;

    private void Start()
    {
        _mapManager = MapManager.Instance;
        _tileManager = TileManager.Instance;
    }

    public void Onclick()
    {
        if (_tileManager.selectedTile == null)
        {
            Debug.Log("タイルが選択されていません");
            return;
        }
        if (_tileManager.GetSelectedTileMapId() == MapId.Empty)
        {
            Debug.Log("ユニットがありません");
            return;
        }
        if (_tileManager.GetSelectedTileMapId() == MapId.Headquarter)
        {
            Debug.Log("本部ユニットは攻撃できません。");
            return;
        }
        if (_tileManager.GetSelectedTileMapId() == MapId.Calling)
        {
            Debug.Log("呼び出し中は攻撃できません。");
            return;
        }
        if (_tileManager.selectedTileController.unitController == null)
        {
            Debug.Log("ユニットコントローラーがありません");
            return;
        }

        Vector2Int target = new Vector2Int(3, 3);
        _tileManager.selectedTileController.unitController.ExecuteAttackRequest(target);
    }

    // private void CheckButtonInteractable()
    // {
    //     if (_tileManager.selectedTile == null
    //         || _tileManager.selectedTileController.unitController == null
    //         || _tileManager.GetSelectedTileMapId() == MapId.Empty
    //         || _tileManager.GetSelectedTileMapId() == MapId.Headquarter)
    //     {
    //         button.interactable = false;
    //     }
    //     else
    //     {
    //         button.interactable = true;
    //     }
    // }

    // public void Onclick()
    // {
    //     int centerY = _tileManager.selectedTileController.gridPosY;
    //     int centerX = _tileManager.selectedTileController.gridPosX;

    //     TileRangeUtil.ForEachSquareRange(centerY, centerX, 1, (y, x) => {
    //         if (y >= 0 && y < _mapManager.mapHeight && x >= 0 && x < _mapManager.mapWidth)
    //         {
    //             _mapManager.playerMapData[y, x].SpawnUnitDelayed(unitData);
    //         }
    //     });
    // }
}
