using UnityEngine;

public class UnitCaller : MonoBehaviour, IButtonAction {
    [SerializeField] private BaseUnitData unitData;

    public void Execute() {
        TileManager.Instance.SpawnUnitOnSelectedTile(unitData);
    }
}