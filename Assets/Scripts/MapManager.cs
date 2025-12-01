using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public GameObject playerMap;
    public GameObject enemyMap;
    public GameObject tilePrefab; // マスのPrefab
    public int mapWidth;     // マップの幅
    public int mapHeight;    // マップの高さ
    private int mapDistance = 5;
    private MapId[,] playerMapData;
    private MapId[,] enemyMapData;

    public enum MapId
    {
        Empty = 0,
        Headquarter = 1,
        Colobus = 2,
        Gecko = 3,
        Herring = 4,
        Muskrat = 5,
        Pudu = 6,
        Sparrow = 7,
        Squid = 8,
        Taipan = 9,
        Calling = 99,
        Error = -1
    }

    private TileManager _tileManager;

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
        GenerateAllyMapData();
        GenerateEnemyMapData();
        UpdateMapText();
    }

    void Start()
    {
        _tileManager = TileManager.Instance;
    }

    private void GenerateAllyMapData()
    {
        playerMapData = new MapId[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                // IDを空地に設定
                playerMapData[x, z] = 0;
                // マスの位置を計算
                Vector3 position = new Vector3(x, 0, z);
                // Prefabをインスタンス化
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                TileController tileController = tile.GetComponent<TileController>();
                // 生成したタイルをMapGeneratorの子オブジェクトにする (任意、Hierarchyを整理するため)
                tile.transform.SetParent(playerMap.transform);
                tileController.xPos = x;
                tileController.zPos = z;
                // タイルの名前を設定 (任意)
                tile.name = $"AllyTile_{x}_{z}";
            }
        }
    }

    // private void GenerateAllyMapData()
    // {
    //     playerMapData = new MapId[mapWidth, mapHeight];

    //     for (int x = 0; x < mapWidth; x++)
    //     {
    //         for (int z = 0; z < mapHeight; z++)
    //         {
    //             // IDを空地に設定
    //             playerMapData[x, z] = MapId.Empty;
    //             // マスの位置を計算
    //             Vector3 position = new Vector3(x, 0, z);
    //             // Prefabをインスタンス化
    //             GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
    //             TileController tileController = tile.GetComponent<TileController>();
    //             // 生成したタイルをMapGeneratorの子オブジェクトにする (任意、Hierarchyを整理するため)
    //             tile.transform.SetParent(playerMap.transform);
    //             tileController.xPos = x;
    //             tileController.zPos = z;
    //             // タイルの名前を設定 (任意)
    //             tile.name = $"AllyTile_{x}_{z}";
    //         }
    //     }
    // }

    private void GenerateEnemyMapData()
    {
        enemyMapData = new MapId[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                // IDを空地に設定
                enemyMapData[x, z] = 0;
                // マスの位置を計算
                Vector3 position = new Vector3(x, 0, z + mapHeight + mapDistance);
                // Prefabをインスタンス化
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                TileController tileController = tile.GetComponent<TileController>();
                // 生成したタイルをMapGeneratorの子オブジェクトにする (任意、Hierarchyを整理するため)
                tile.transform.SetParent(enemyMap.transform);
                tileController.xPos = x;
                tileController.zPos = z;
                // タイルの名前を設定 (任意)
                tile.name = $"EnemyTile_{x}_{z}";
            }
        }
    }

    private void UpdateMapText()
    {
        string resultText = "";
        for (int h = 0; h < mapHeight; h++)
        {
            string rowstr = "";
            for (int w = 0; w < mapWidth; w++)
            {
                rowstr = rowstr + playerMapData[w, h] + " ";
            }
            resultText = rowstr + "\n" + resultText;
        }
        Debug.Log(resultText);
    }

    public MapId GetSelectedTileId()
    {
        GameObject selectedTile = _tileManager.selectedTile;

        if (selectedTile == null) return MapId.Error;

        Vector3 selectedTilePosition = selectedTile.transform.position;
        return playerMapData[(int)selectedTilePosition.x, (int)selectedTilePosition.z];
    }

    public void UpdateSelectedTileOnUnitId(MapId unitId)
    {
        GameObject selectedTile = _tileManager.selectedTile;

        if (selectedTile == null) throw new NullReferenceException("selectedTile is NULL");

        Vector3 selectedTilePosition = selectedTile.transform.position;
        playerMapData[(int)selectedTilePosition.x, (int)selectedTilePosition.z] = unitId;

        UpdateMapText();
    }

    // public void GetPlayerHeadquartersCount()
    // {
    //     for (int h = 0; h < mapHeight; h++)
    //     {
    //         string rowstr = "";
    //         for (int w = 0; w < mapWidth; w++)
    //         {
    //             if (playerMapData[w, h])
    //             rowstr = rowstr + playerMapData[w, h] + " ";
    //         }
    //     }
    // }
}
