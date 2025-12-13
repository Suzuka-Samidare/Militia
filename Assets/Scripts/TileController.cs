using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using MapId = MapManager.MapId;

public class TileController : MonoBehaviour
{
    [Header("Tile Status")]
    public Boolean isSelected;
    public int xPos;
    public int zPos;
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

    private IEnumerator callUnit(BaseUnitData newUnit)
    {
        yield return new WaitForSeconds(newUnit.callTime);

        if (currentUnit != null)
        {
            Destroy(currentUnit);
        }

        Vector3 tilePosition = gameObject.transform.position;
        Vector3 UnitPosition = new Vector3(tilePosition.x, newUnit.initPos.y, tilePosition.z);
        MapManager.Instance.UpdateSelectedTileOnUnitId(newUnit.id);
        currentUnit = Instantiate(newUnit.unitPrefab, UnitPosition, Quaternion.identity, gameObject.transform);
    }

    public void SetUnitObject(BaseUnitData newUnit)
    {
        CheckSelectedTile();

        // 呼び出し時間がある場合は仮オブジェクト配置
        if (newUnit.callTime > 0)
        {
            Vector3 tilePosition = gameObject.transform.position;
            Vector3 UnitPosition = new Vector3(tilePosition.x, 0.75f, tilePosition.z);
            MapManager.Instance.UpdateSelectedTileOnUnitId(MapId.Calling);
            currentUnit = Instantiate(tempUnit, UnitPosition, Quaternion.identity, gameObject.transform);
        }

        // 呼び出し開始
        StartCoroutine(callUnit(newUnit));
    }

    public void DestroyUnitObject()
    {
        CheckSelectedTile();
        Destroy(currentUnit);
    }

    private void UpdateFocusVisual()
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

    private void Blink()
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
