using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public BaseUnitData unitData;

    private MapManager _mapManager;

    void Start()
    {
        _mapManager = MapManager.Instance;
    }

    public void Onclick()
    {
        _mapManager.RevealManhattanRange(4, 3, 3, (y, x) => {
            _mapManager.playerMapData[y, x].SpawnUnitDelayed(unitData);
        });
    }
}
