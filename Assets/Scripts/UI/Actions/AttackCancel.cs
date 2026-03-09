using UnityEngine;

public class AttackCancel : MonoBehaviour, IButtonAction
{
    [Header("Refs")]
    private TileManager _tileManager;

    private void Start()
    {
        _tileManager = TileManager.Instance;
    }

    public void Execute()
    {
        _tileManager.ClearTargetTiles();
    }
}

