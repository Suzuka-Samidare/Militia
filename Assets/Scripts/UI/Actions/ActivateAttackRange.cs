using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAttackRange : MonoBehaviour, IButtonAction
{
    [SerializeField, Tooltip("最後に攻撃元になっているタイル")]
    private TileController lastSelectedTile;

    [Header("Refs")]
    private TileManager _tileManager;
    private TileController _lastSelectedUnit;

    private void Start()
    {
        _tileManager = TileManager.Instance;
    }

    public void Execute()
    {
        if (_tileManager.targetTile && _tileManager.selectedTileController != lastSelectedTile)
        {
            _tileManager.RegisterTargetTiles(_tileManager.targetTile.gridPos);
            lastSelectedTile = _tileManager.selectedTileController;
        }
        if (_tileManager.targetTile)
        {
            _tileManager.ActivateTargetFlags();
        }
    }
}