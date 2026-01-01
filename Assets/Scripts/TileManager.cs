using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using MapId = MapManager.MapId;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    public GameObject selectedTile = null;
    private TileController _selectedTileController = null;

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

    // 選択中のマスを設定するメソッド
    void SetSelectedTile(GameObject tile)
    {
        // 以前に選択されていたマスがあれば、ハイライトを解除するなどの処理
        if (selectedTile != null && _selectedTileController != null)
        {
            _selectedTileController.isSelected = false;
        }

        selectedTile = tile;
        _selectedTileController = selectedTile.GetComponent<TileController>();

        // 新しく選択されたマスをハイライトするなどの処理
        if (selectedTile != null && _selectedTileController != null)
        {
            _selectedTileController.isSelected = true;
        }
    }

    // 選択中のマスを解除する
    void ClearSelectedTile()
    {
        // 以前に選択されていたマスがあれば、ハイライトを解除するなどの処理
        if (selectedTile != null && _selectedTileController != null)
        {
            _selectedTileController.isSelected = false;
        }

        selectedTile = null;
        _selectedTileController = null;
    }

    public async Task SetSelectedTileOnUnit(BaseUnitData newUnit)
    {
        if (selectedTile == null) return;

        _selectedTileController.DestroyUnitObject();
        // _selectedTileController.SetUnitObject(newUnit);

        // 呼び出し時間がある場合は仮オブジェクト配置
        if (newUnit.callTime > 0)
        {
            await MapManager.Instance.UpdateTileAsync(MapId.Calling);
            _selectedTileController.SetTempUnitObject();
        }

        // 呼び出し開始
        await Task.Delay((int)newUnit.callTime * 1000);

        await MapManager.Instance.UpdateTileAsync(newUnit.id);
        _selectedTileController.SetUnitObject(newUnit);
    }
    
    public void ClearSelectedTileOnUnit()
    {
        if (selectedTile == null || _selectedTileController == null) return;

        _selectedTileController.DestroyUnitObject();
    }
}
