using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // マスのPrefab
    public int mapWidth = 10;     // マップの幅
    public int mapHeight = 10;    // マップの高さ
    public float tileSize = 1.0f; // マスのサイズ（Prefabのスケールに合わせる）

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                // マスの位置を計算
                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);

                // Prefabをインスタンス化
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);

                // 生成したタイルをMapGeneratorの子オブジェクトにする (任意、Hierarchyを整理するため)
                tile.transform.parent = this.transform;

                // タイルの名前を設定 (任意)
                tile.name = $"Tile_{x}_{z}";
            }
        }
    }
}
