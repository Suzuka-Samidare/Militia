using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using System.Threading.Tasks;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("オブジェクト関連")]
    public GameObject playerMap;
    public GameObject enemyMap;
    public GameObject tilePrefab; // マスのPrefab

    [Header("マップ関連")]
    public int mapWidth;     // マップの幅
    public int mapHeight;    // マップの高さ
    private int mapDistance = 5;
    private TileController[,] playerMapData;
    private TileController[,] enemyMapData;
    public bool isDirty;
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

    [Header("本部関連")]
    [Tooltip("本部最大設置数")] public int maxHqCount = 2;
    [Tooltip("本部残数"), SerializeField] public int AllyHqCount;

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
    }

    void Start()
    {
        _tileManager = TileManager.Instance;
    }

    void Update()
    {
        if (isDirty)
        {
            UpdateData();
        }
    }

    private void GenerateAllyMapData()
    {
        playerMapData = new TileController[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                // マスの位置を計算
                Vector3 position = new Vector3(x, 0, z);
                // Prefabをインスタンス化
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                // 生成したタイルをMapGeneratorの子オブジェクトにする (任意、Hierarchyを整理するため)
                tile.transform.SetParent(playerMap.transform);
                tile.name = $"AllyTile_{x}_{z}";
                // 各フィールド値の更新
                TileController tileController = tile.GetComponent<TileController>();
                tileController.globalPos = position;
                tileController.gridPosX = x;
                tileController.gridPosZ = z;
                // クラスをマップデータとして格納
                playerMapData[x, z] = tileController;
            }
        }
    }

    private void GenerateEnemyMapData()
    {
        enemyMapData = new TileController[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                // マスの位置を計算
                Vector3 position = new Vector3(x, 0, z + mapHeight + mapDistance);
                // Prefabをインスタンス化
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                // 生成したタイルをMapGeneratorの子オブジェクトにする (任意、Hierarchyを整理するため)
                tile.transform.SetParent(enemyMap.transform);
                tile.name = $"EnemyTile_{x}_{z}";
                // 各フィールド値の更新
                TileController tileController = tile.GetComponent<TileController>();
                tileController.globalPos = position;
                tileController.gridPosX = x;
                tileController.gridPosZ = z;
                 // クラスをマップデータとして格納
                enemyMapData[x, z] = tileController;
            }
        }
    }

    private void UpdateData()
    {
        GetPlayerHeadquartersCount();
        isDirty = false;
    }

    // private void UpdateMapText()
    // {
    //     string resultText = "";
    //     for (int z = 0; z < mapHeight; z++)
    //     {
    //         string rowstr = "";
    //         for (int x = 0; x < mapWidth; x++)
    //         {
    //             MapId id;
    //             GameObject currentUnit =  playerMapData[x, z].currentUnit;
    //             if (currentUnit != null)
    //             {
    //                 id = currentUnit.GetComponent<BaseUnitData>().id;
    //             }
    //             else
    //             {
    //                 id = MapId.Empty;
    //             }
    //             rowstr = rowstr + (int)id + " ";
    //         }
    //         resultText = rowstr + "\n" + resultText;
    //     }
    //     Debug.Log(resultText);
    // }

    public void GetPlayerHeadquartersCount()
    {
        int count = 0;
        for (int z = 0; z < mapHeight; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                UnitController unitController = playerMapData[x, z].unitController;
                if (unitController)
                {
                    if (unitController.profile.id == MapId.Headquarter) count++;
                }
            }
        }

        if (count > maxHqCount) throw new Exception("Headquarters unit limit exceeded.");
        
        AllyHqCount = count;
    }

    // public void UpdateSelectedTileOnUnitId(MapId unitId)
    // {
    //     GameObject selectedTile = _tileManager.selectedTile;

    //     if (selectedTile == null) throw new NullReferenceException("selectedTile is NULL");

    //     Vector3 selectedTilePosition = selectedTile.transform.position;
    //     playerMapData[(int)selectedTilePosition.x, (int)selectedTilePosition.z] = unitId;

    //     UpdateMapText();
    // }

    // TODO: 上記の UpdateSelectedTileOnUnitId の役割になるようにしたい
    // public async Task UpdateTileAsync(MapId unitId)
    // {
    //     // --- API通信ありの場合のイメージ ---
    //     // var response = await MyApiService.PostTileUpdate(x, y, id);
    //     // if (!response.IsSuccess) throw new Exception("API Error");
    //     Debug.Log("UpdateTileAsync Delay開始");
    //     await Task.Delay(500); // 現時点では擬似的な通信待機（1秒）
    //     Debug.Log("UpdateTileAsync Delay完了");

    //     GameObject selectedTile = _tileManager.selectedTile;

    //     if (selectedTile == null) throw new NullReferenceException("selectedTile is NULL");
        
    //     Vector3 selectedTilePosition = selectedTile.transform.position;
    //     playerMapData[(int)selectedTilePosition.x, (int)selectedTilePosition.z] = unitId;

    //     // サーバー側でのID検証などをシミュレート
    //     // if (Enum.IsDefined(typeof(MapId), unitId)) throw new ArgumentException("無効なタイルIDです");
    // }

}
