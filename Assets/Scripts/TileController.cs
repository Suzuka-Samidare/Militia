using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerClickHandler
{
    public Boolean isSelected;
    private Renderer objectRenderer;

    [Header("Focus Visual")]
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
        if (isSelected)
        {
            Blink();
        }

        CheckTileStatus();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Selection.activeGameObject = this.gameObject;
        isSelected = true;
    }

    void CheckTileStatus()
    {
        if (Selection.activeGameObject != this.gameObject && isSelected)
        {
            isSelected = false;
            objectRenderer.material.color = defaultColor;
        }
    }

    void Blink()
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
