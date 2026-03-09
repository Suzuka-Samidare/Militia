using System;
using System.Collections.Generic;
using UnityEngine;
using MapId = MapManager.MapId;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    [Header("味方マップ関連")]
    [SerializeField, Tooltip("セレクト中のタイル")]
    private GameObject _selectedTile;
    public GameObject selectedTile
    {
        get => _selectedTile;
        set
        {
            if (_selectedTile == value) return;
            _selectedTile = value;
            RefreshComponents();
        }
    }
    [field: SerializeField, Tooltip("セレクト中のタイルコントローラ")]
    public TileController selectedTileController { get; private set; }
    [SerializeField, Tooltip("アクセス可否")]
    private bool _canAccessSelectedTileController => selectedTile != null && selectedTileController != null;

    [Header("敵マップ関連")]
    [SerializeField, Tooltip("ターゲット指定中タイル")]
    private List<TileController> targetTiles = new List<TileController>();

    [Header("Refs")]
    private MapManager _mapManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
         _mapManager = MapManager.Instance;
    }

    private void RefreshComponents()
    {
        if (selectedTile != null)
        {
            selectedTileController = selectedTile.GetComponent<TileController>();
        }
        else
        {
            selectedTileController = null;
        }
    }

    public void SetTargetTiles(Vector2Int targetPos)
    {
        if (!_canAccessSelectedTileController) return;

        if (targetTiles.Count > 0)
        {
            ClearTargetTiles();
        }

        if (!selectedTileController.unitController)
        {
            throw new Exception("UnitControllerがありません");
        }

        List<Vector2Int> tilePositions = selectedTileController.unitController.GetTargetTilePositions(targetPos);

        foreach (Vector2Int pos in tilePositions)
        {
            TileController tileController = _mapManager.GetEnemyMapTile(pos);

            if (tileController != null)
            {
                // 新しく選択状態にする
                tileController.isTargeted = true;
                // 配列（リスト）に保存
                targetTiles.Add(tileController);
            }
        }
    }

    public void ClearTargetTiles()
    {
        foreach (TileController tileController in targetTiles)
        {
            if (tileController != null)
            {
                tileController.isTargeted = false;
            }
        }
        targetTiles.Clear();
    }

    // TODO: MapManagerにするか検討する。
    private TileController GetPlayerTile(Vector2Int pos)
    {
        return _mapManager.playerMapData[pos.x, pos.y];
    }



    // void Update()
    // {
    //     if (Input.GetMouseButtonUp(0) && !CameraMovement.Instance.isDragging)
    //     {
    //         CheckMouseDown();
    //     }
    // }

    // void CheckMouseDown()
    // {
    //     // UI要素を選択またはフォーカスしている場合は処理を進行しない
    //     if (EventSystem.current.IsPointerOverGameObject()) return;

    //     // Raycastでゲームオブジェクト接触判定
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     RaycastHit hit;

    //     // 接触したオブジェクトが無い場合、タイル選択状態を解除
    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         GameObject hitObject = hit.collider.gameObject;

    //         // 接触オブジェクトがタイルまたはユニットだった場合、選択状態に更新
    //         if (hitObject.CompareTag("Tile"))
    //         {
    //             // タイルを引数にして処理
    //             SetSelectedTile(hitObject);
    //         }
    //         else if (hitObject.CompareTag("Unit"))
    //         {
    //             // ユニットの親要素であるタイルを引数にして処理
    //             SetSelectedTile(hitObject.transform.parent.gameObject);
    //         }
    //         else
    //         {
    //             // Debug.Log("Ray判定あり & タイルではない");
    //         }
    //     }
    //     else
    //     {
    //         // Debug.Log("Ray判定なし");
    //         ClearSelectedTile();
    //     }
    // }

    // private void CheckSelectedTile()
    // {
    //     Vector3 tilePosition = gameObject.transform.position;
    //     Vector3 selectedTilePosition = TileManager.Instance.selectedTile.transform.position;

    //     if (selectedTilePosition != tilePosition)
    //     {
    //         Debug.LogError("選択状態のタイルと位置データが一致しません。");
    //         return;
    //     }
    // }

    // タイルを選択状態に設定する
    public void SetSelectedTile(GameObject tile)
    {
        // 以前に選択されていたマスがあれば、ハイライトを解除するなどの処理
        if (_canAccessSelectedTileController)
        {
            selectedTileController.isSelected = false;
        }

        selectedTile = tile;

        // 新しく選択されたマスをハイライトするなどの処理
        if (_canAccessSelectedTileController)
        {
            selectedTileController.isSelected = true;
        }
    }

    // 選択中のマスを解除する
    public void ClearSelectedTile()
    {
        // 以前に選択されていたマスがあれば、ハイライトを解除するなどの処理
        if (_canAccessSelectedTileController)
        {
            selectedTileController.isSelected = false;
        }

        selectedTile = null;
    }

    // 選択中のマス上にあるユニットを消す
    public void ClearSelectedTileOnUnit()
    {
        if (!selectedTileController.isExistUnit)
        {
            Debug.Log("削除するユニットが存在しません");
            return;
        }

        selectedTileController.DestroyUnit();
    }

    // 選択中のマス上にあるユニットのマップIDを取得
    public MapId GetSelectedTileMapId()
    {
        if (selectedTileController.unitObject != null)
        {
            return selectedTileController.unitMapId;
        }
        else
        {
            return MapId.Empty;
        }
    }

    public void GetSelectedTileUnitDetail()
    {
        if (selectedTileController.unitStats)
        {
            UnitDetailController.Instance.Open(
                selectedTileController.unitStats.profile.unitName,
                selectedTileController.unitStats.profile.maxHp,
                selectedTileController.unitStats.hp,
                selectedTileController.unitMapId == MapId.Calling
            );
        }
        else
        {
            UnitDetailController.Instance.Close();
        }
    }
}
