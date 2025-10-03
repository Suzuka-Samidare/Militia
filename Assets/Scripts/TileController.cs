using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour
{
    [Header("Tile Status")]
    public Boolean isSelected;
    public GameObject currentUnit;

    [Header("Unit")]
    public GameObject tempUnit;

    [Header("Focus Visual")]
    private Renderer objectRenderer;
    public Color defaultColor;
    public Color focusColor;
    public Color currentColor;
    private float startTime;
    private Boolean isFadingToReverse;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        defaultColor = objectRenderer.material.color;
        startTime = Time.time;
    }

    void Update()
    {
        UpdateFocusVisual();
    }

    private void CheckSelectedTile()
    {
        Vector3 tilePosition = gameObject.transform.position;
        Vector3 selectedTilePosition = TileManager.Instance.selectedTile.transform.position;

        if (selectedTilePosition != tilePosition)
        {
            Debug.LogError("選択状態のタイルと位置データが一致しません。");
            return;
        }
    }

    public void DestroyUnitObject()
    {
        CheckSelectedTile();
        // ユニットの破棄
        Destroy(currentUnit);
    }

    private IEnumerator callUnit(BaseUnitData newUnit)
    {
        Vector3 tilePosition = gameObject.transform.position;
        Vector3 UnitPosition = new Vector3(tilePosition.x, newUnit.initPos.y, tilePosition.z);

        yield return new WaitForSeconds(newUnit.callTime);

        // ユニットの破棄
        Destroy(currentUnit);

        currentUnit = Instantiate(newUnit.unitPrefab, UnitPosition, Quaternion.identity, gameObject.transform);
    }

    public void SetUnitObject(BaseUnitData newUnit)
    {
        CheckSelectedTile();

        Vector3 tilePosition = gameObject.transform.position;
        Vector3 UnitPosition = new Vector3(tilePosition.x, 0.75f, tilePosition.z);

        // 呼び出し中の仮オブジェクト配置
        currentUnit = Instantiate(tempUnit, UnitPosition, Quaternion.identity, gameObject.transform);

        StartCoroutine(callUnit(newUnit));
    }

    // public void UpdateUnitObject(GameObject newUnit)
    // {
    //     CheckSelectedTile();

    //     Vector3 tilePosition = gameObject.transform.position;
    //     Vector3 UnitPosition = new Vector3(tilePosition.x, 0.75f, tilePosition.z);

    //     // ユニットの破棄
    //     Destroy(currentUnit);
    //     // ユニットの配置
    //     currentUnit = Instantiate(newUnit, UnitPosition, Quaternion.identity, gameObject.transform);
    // }

    void UpdateFocusVisual()
    {
        if (isSelected)
        {
            Blink();
        }
        else
        {
            objectRenderer.material.color = defaultColor;
        }
    }

    public void Blink()
    {
        float duration = 1.0f;
        float elapsedTime = (Time.time - startTime) / duration;

        // LERPを使ってカラーを補間
        if (!isFadingToReverse)
        {
            currentColor = Color.Lerp(defaultColor, focusColor, elapsedTime);
        }
        else
        {
            currentColor = Color.Lerp(focusColor, defaultColor, elapsedTime);
        }

        // マテリアルカラーを更新
        objectRenderer.material.color = currentColor;

        // フェードが完了したら、次のフェードの準備
        if (elapsedTime >= 1.0f)
        {
            // 次にフェードする方向を切り替える
            isFadingToReverse = !isFadingToReverse;
            // 新しいフェードの開始時間をリセット
            startTime = Time.time;
        }
    }
}

// public class TileController : MonoBehaviour, IPointerClickHandler
// {
//     public Boolean isSelected;
//     public int unitId = 0;

//     [Header("Focus Visual")]
//     private Renderer objectRenderer;
//     public Color defaultColor;
//     public Color focusColor;
//     public Color currentColor;
//     private float startTime;
//     private Boolean isFadingToReverse;

//     void Awake()
//     {
//         objectRenderer = GetComponent<Renderer>();
//         defaultColor = objectRenderer.material.color;
//         startTime = Time.time;
//     }

//     void Update()
//     {
//         if (isSelected)
//         {
//             Blink();
//         }

//         CheckTileStatus();
//     }

//     public void OnPointerClick(PointerEventData pointerEventData)
//     {
//         Selection.activeGameObject = this.gameObject;
//         isSelected = true;
//     }

//     void CheckTileStatus()
//     {
//         if (Selection.activeGameObject != this.gameObject && isSelected)
//         {
//             isSelected = false;
//             objectRenderer.material.color = defaultColor;
//         }
//     }

//     public void UpdateUnitOnTile(GameObject unitPrefab)
//     {
//         GameObject selectedTile = Selection.activeGameObject;

//         if (selectedTile != null)
//         {
//             Debug.LogError("選択状態のタイルがありません");
//         }

//         Vector3 tilePosition = gameObject.transform.position;
//         Vector3 selectedTilePosition = selectedTile.transform.position;

//         if (selectedTilePosition != tilePosition)
//         {
//             Debug.LogError("選択状態のタイルと位置データが一致しません");
//         }

//         Vector3 UnitPosition = new Vector3(tilePosition.x, 0.75f, tilePosition.z);
//         Instantiate(unitPrefab, UnitPosition, Quaternion.identity);
//     }

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
// }
