using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // マスのPrefab
    public int mapWidth;     // マップの幅
    public int mapHeight;    // マップの高さ
    private int[,] mapData;

    void Awake()
    {
        mapData = new int[mapWidth, mapHeight];

        for (int w = 0; w < mapWidth; w++)
        {
            for (int h = 0; h < mapHeight; h++)
            {
                mapData[w, h] = 0;
            }
        }
    }

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int w = 0; w < mapWidth; w++)
        {
            for (int h = 0; h < mapHeight; h++)
            {
                // マスの位置を計算
                Vector3 position = new Vector3(w, 0, h);

                // Prefabをインスタンス化
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);

                // 生成したタイルをMapGeneratorの子オブジェクトにする (任意、Hierarchyを整理するため)
                tile.transform.parent = this.transform;

                // タイルの名前を設定 (任意)
                tile.name = $"Tile_{w}_{h}";
            }
        }
    }
}

// public class MapGenerator : MonoBehaviour
// {
//     public GameObject tilePrefab; // マスのPrefab
//     public int mapWidth;     // マップの幅
//     public int mapHeight;    // マップの高さ
//     private float tileSize = 1.0f; // マスのサイズ（Prefabのスケールに合わせる）

//     void Start()
//     {
//         GenerateMap();
//     }

//     void GenerateMap()
//     {
//         for (int x = 0; x < mapWidth; x++)
//         {
//             for (int z = 0; z < mapHeight; z++)
//             {
//                 // マスの位置を計算
//                 Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);

//                 // Prefabをインスタンス化
//                 GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);

//                 // 生成したタイルをMapGeneratorの子オブジェクトにする (任意、Hierarchyを整理するため)
//                 tile.transform.parent = this.transform;

//                 // タイルの名前を設定 (任意)
//                 tile.name = $"Tile_{x}_{z}";
//             }
//         }
//     }
// }
