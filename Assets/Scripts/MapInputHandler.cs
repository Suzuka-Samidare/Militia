using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapInputHandler : MonoBehaviour
{
    private TileManager _tileManager;

    private void Start()
    {
        _tileManager = TileManager.Instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !CameraMovement.Instance.isDragging)
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        // UI要素を選択またはフォーカスしている場合は処理を進行しない
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Raycastでゲームオブジェクト接触判定
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 接触したオブジェクトが無い場合、タイル選択状態を解除
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // 接触オブジェクトがタイルまたはユニットだった場合、選択状態に更新
            if (hitObject.CompareTag("Tile"))
            {
                // タイルを引数にして処理
                _tileManager.SetSelectedTile(hitObject);
            }
            if (hitObject.CompareTag("Unit"))
            {
                // ユニットの親要素であるタイルを引数にして処理
                _tileManager.SetSelectedTile(hitObject.transform.parent.gameObject);
            }
            if (hitObject.CompareTag("Tile") || hitObject.CompareTag("Unit"))
            {
                _tileManager.GetSelectedTileUnitDetail();
            }

            // Debug.Log("Ray判定あり & タイルではない");
        }
        else
        {
            // Debug.Log("Ray判定なし");
            _tileManager.ClearSelectedTile();
        }
    }
}
