using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using MapId = MapManager.MapId;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    [Header("選択済みタイル関連")]
    [Tooltip("タイルオブジェクト")]
    public GameObject selectedTile = null;
    [Tooltip("タイルコントローラ"), SerializeField]
    public TileController selectedTileController => selectedTile ? selectedTile.GetComponent<TileController>() : null;
    [Tooltip("アクセス可否"), SerializeField]
    private bool _canAccessSelectedTileController => selectedTile != null && selectedTileController != null;

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
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !CameraMovement.Instance.isDragging)
        {
            CheckMouseDown();
        }
    }

    void CheckMouseDown()
    {
        // UI要素を選択またはフォーカスしている場合は処理を進行しない
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Raycastでゲームオブジェクト接触判定
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 接触したオブジェクトが無い場合、タイル選択状態を解除
        if (Physics.Raycast(ray, out hit))
        {
            // 接触オブジェクトがタイルまたはユニットだった場合、選択状態に更新
            if (hit.collider.gameObject.CompareTag("Tile"))
            {
                // タイルを引数にして処理
                SetSelectedTile(hit.collider.gameObject);
            }
            else if (hit.collider.gameObject.CompareTag("Unit"))
            {
                // ユニットの親要素であるタイルを引数にして処理
                SetSelectedTile(hit.collider.gameObject.transform.parent.gameObject);
            }
            else
            {
                // Debug.Log("Ray判定あり & タイルではない");
            }
        }
        else
        {
            // Debug.Log("Ray判定なし");
            ClearSelectedTile();
        }
    }

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
    private void SetSelectedTile(GameObject tile)
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

    public void SpawnUnitOnSelectedTile(BaseUnitData unitData)
    {
        if (unitData.callingProfile.callTime > 0)
        {
            Debug.Log("SpawnUnitOnSelectedUnit: 待ち時間ありのユニットです。");
            selectedTileController.SpawnUnitDelayed(unitData);
        }
        else
        {
            Debug.Log("SpawnUnitOnSelectedUnit: 待ち時間なしのユニットです。");
            selectedTileController.SpawnUnit(unitData);
        }

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

    // public void DestroyAllTileOnUnitObject()
    // {
        
    // }

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
}
