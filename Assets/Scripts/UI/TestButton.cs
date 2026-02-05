using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public void Onclick()
    {
        // TestFunc();
    }

    private void TestFunc()
    {
        TileController tileController = TileManager.Instance.selectedTileController;

        if (tileController.unitController)
        {
            UnitDetailController.Instance.Open(
                tileController.unitController.profile.unitName,
                tileController.unitController.profile.maxHp,
                tileController.unitController.hp,
                false
            );
        }
        if (tileController.calllingUnitController)
        {
            UnitDetailController.Instance.Open(
                tileController.calllingUnitController.profile.unitName,
                tileController.calllingUnitController.profile.maxHp,
                tileController.calllingUnitController.hp,
                true
            );
        }
    }
}
