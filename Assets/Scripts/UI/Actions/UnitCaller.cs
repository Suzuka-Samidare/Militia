using UnityEngine;

public class UnitCaller : MonoBehaviour, IButtonAction {
    [SerializeField, Tooltip("ユニットデータ")]
    private BaseUnitData unitData;

    [Header("Refs")]
    private TileManager _tileManager;

    private void Start()
    {
        _tileManager = TileManager.Instance;
    }

    public void Execute() {
        if (unitData.callTime > 0)
        {
            Debug.Log("SpawnUnitOnSelectedUnit: 待ち時間ありのユニットです。");
            _tileManager.selectedTileController.SpawnUnitDelayed(unitData);
        }
        else
        {
            Debug.Log("SpawnUnitOnSelectedUnit: 待ち時間なしのユニットです。");
            _tileManager.selectedTileController.SpawnUnit(unitData);
        }
    }
}