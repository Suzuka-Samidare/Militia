using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    private DialogController _dialogController;
    private MapManager _mapManager;
    
    void Start()
    {
        // _dialogController = DialogController.Instance;
        _mapManager = MapManager.Instance;
    }

    public void Onclick()
    {
        Debug.Log("TestButton");
        TileManager.Instance.ClearSelectedTileOnUnit();
    }
}
