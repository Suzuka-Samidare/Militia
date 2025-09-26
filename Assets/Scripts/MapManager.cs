using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public GameObject tilePrefab; // マスのPrefab
    public int mapWidth;     // マップの幅
    public int mapHeight;    // マップの高さ
    private int[,] mapData;
    // 0: 空地
    // 1: red
    // 2: orange
    // 3: yellow
    // 4: yellowgreen
    // 5: green

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GenerateMapData();
        UpdateMapText();
    }

    void Start()
    {
        GenerateMapTile();
    }

    void GenerateMapData()
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

    void GenerateMapTile()
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

    void UpdateMapText()
    {
        string resultText = "";
        for (int h = 0; h < mapHeight; h++)
        {
            string rowstr = "";
            for (int w = 0; w < mapWidth; w++)
            {
                rowstr = rowstr + mapData[w, h] + " ";
            }
            resultText = rowstr + "\n" + resultText;
        }
        // Debug.Log(resultText);
    }

    public void UpdateUnitOnMapData(int unitId)
    {
        GameObject selectedTile = TileManager.Instance.selectedTile;

        if (selectedTile != null)
        {
            Vector3 selectedTilePosition = selectedTile.transform.position;
            mapData[(int)selectedTilePosition.x, (int)selectedTilePosition.z] = unitId;
            UpdateMapText();
        }

        // if (selectedTilePosition != gameObject.transform.position)
        // {
        //     Vector3 selectedTilePosition = selectedTile.transform.position;
        //     Vector3 UnitPosition = new Vector3(selectedTilePosition.x, 0.75f, selectedTilePosition.z);
        //     mapData[(int)selectedTilePosition.x, (int)selectedTilePosition.z] = unitId;

        //     // TODO: TileManagerで処理したほうが良いかも
        //     GameObject tile = Instantiate(UnitPrefab, UnitPosition, Quaternion.identity);
        // }
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
