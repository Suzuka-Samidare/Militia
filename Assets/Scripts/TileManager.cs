using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    public GameObject selectedTile = null;

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
        Debug.Log(EventSystem.current.IsPointerOverGameObject());

        // UI要素を選択またはフォーカスしている場合は処理を進行しない
        // if (EventSystem.current.currentSelectedGameObject) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Raycastでゲームオブジェクト接触判定
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 接触したオブジェクトが無い場合、タイル選択状態を解除
        if (Physics.Raycast(ray, out hit))
        {
            // 接触オブジェクトがタイルだった場合、選択状態に更新
            if (hit.collider.gameObject.CompareTag("Tile"))
            {
                // Debug.Log("Ray判定あり & タイルである " + hit.collider.gameObject.name);
                SetSelectedTile(hit.collider.gameObject);
            }
            else if (hit.collider.gameObject.CompareTag("Unit"))
            {
                // Debug.Log("Ray判定あり & ユニットである " + hit.collider.gameObject.name);
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

    // 選択中のマスを取得するメソッド
    public GameObject GetSelectedTile()
    {
        return selectedTile;
    }

    // 選択中のマスを設定するメソッド
    void SetSelectedTile(GameObject tile)
    {
        // 以前に選択されていたマスがあれば、ハイライトを解除するなどの処理
        if (selectedTile != null)
        {
            selectedTile.GetComponent<TileController>().isSelected = false;
        }

        selectedTile = tile;

        // 新しく選択されたマスをハイライトするなどの処理
        if (selectedTile != null)
        {
            selectedTile.GetComponent<TileController>().isSelected = true;
        }
    }

    // 選択中のマスを解除する
    void ClearSelectedTile()
    {
        // 以前に選択されていたマスがあれば、ハイライトを解除するなどの処理
        if (selectedTile != null)
        {
            selectedTile.GetComponent<TileController>().isSelected = false;
        }
        selectedTile = null;
    }

    public void SetSelectedTileOnUnit(BaseUnitData baseUnitData)
    {
        if (selectedTile == null) return;

        selectedTile.GetComponent<TileController>().DestroyUnitObject();
        selectedTile.GetComponent<TileController>().SetUnitObject(baseUnitData);
    }

    // ======================================================
    // public Boolean isSelected;
    // public int unitId = 0;

    // [Header("Focus Visual")]
    // private Renderer objectRenderer;
    // public Color defaultColor;
    // public Color focusColor;
    // public Color currentColor;
    // private float startTime;
    // private Boolean isFadingToReverse;

    // void Awake()
    // {
    //     objectRenderer = GetComponent<Renderer>();
    //     defaultColor = objectRenderer.material.color;
    //     startTime = Time.time;
    // }

    // void Update()
    // {
    //     if (isSelected)
    //     {
    //         Blink();
    //     }

    //     CheckTileStatus();
    // }

    // public void OnPointerClick(PointerEventData pointerEventData)
    // {
    //     Selection.activeGameObject = this.gameObject;
    //     isSelected = true;
    // }

    // void CheckTileStatus()
    // {
    //     if (Selection.activeGameObject != this.gameObject && isSelected)
    //     {
    //         isSelected = false;
    //         objectRenderer.material.color = defaultColor;
    //     }
    // }

    // public void UpdateUnitOnTile(GameObject unitPrefab)
    // {
    //     GameObject selectedTile = Selection.activeGameObject;

    //     if (selectedTile != null)
    //     {
    //         Debug.LogError("選択状態のタイルがありません");
    //     }

    //     Vector3 tilePosition = gameObject.transform.position;
    //     Vector3 selectedTilePosition = selectedTile.transform.position;

    //     if (selectedTilePosition != tilePosition)
    //     {
    //         Debug.LogError("選択状態のタイルと位置データが一致しません");
    //     }

    //     Vector3 UnitPosition = new Vector3(tilePosition.x, 0.75f, tilePosition.z);
    //     Instantiate(unitPrefab, UnitPosition, Quaternion.identity);
    // }

    // void Blink()
    // {
    //     float duration = 1.0f;
    //     float elapsedTime = (Time.time - startTime) / duration;

    //     // LERPを使ってカラーを補間
    //     if (!isFadingToReverse)
    //     {
    //         currentColor = Color.Lerp(defaultColor, focusColor, elapsedTime);
    //     }
    //     else
    //     {
    //         currentColor = Color.Lerp(focusColor, defaultColor, elapsedTime);
    //     }

    //     // マテリアルカラーを更新
    //     objectRenderer.material.color = currentColor;

    //     // フェードが完了したら、次のフェードの準備
    //     if (elapsedTime >= 1.0f)
    //     {
    //         // 次にフェードする方向を切り替える
    //         isFadingToReverse = !isFadingToReverse;
    //         // 新しいフェードの開始時間をリセット
    //         startTime = Time.time;
    //     }
    // }
}
