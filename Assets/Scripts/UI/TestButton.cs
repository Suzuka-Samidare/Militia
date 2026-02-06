using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public void Onclick()
    {
        TestFunc();
    }

    private void TestFunc()
    {
        // TileController tileController = TileManager.Instance.selectedTileController;
        TileController tileController = MapManager.Instance.enemyMapData[0,0];

        if (tileController)
        {
            tileController.Reveal();
        }
        else
        {
            Debug.Log("タイルが選択されていません");
        }
    }
}
