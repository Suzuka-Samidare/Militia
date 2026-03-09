using System;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [Header("ビジュアル関連")]
    [Tooltip("ベース色")]
    public Color baseColor;
    [Tooltip("明滅色（自マップ用）")]
    public Color blinkAllyColor;
    [Tooltip("明滅色（敵マップ用）")]
    public Color blinkEnemyColor;
    [Tooltip("不可視状態時の色")]
    public Color invisibleColor;

    [Header("状態管理")]
    [SerializeField, Tooltip("現在のベースカラー")]
    private Color currentBaseColor;
    [SerializeField, Tooltip("現在の上面カラー")]
    private Color currentTopColor;


    [Header("Refs")]
    private Renderer objectRenderer;
    private MaterialPropertyBlock propBlock;
    // シェーダーのプロパティ名（Shader GraphのReferenceで設定したもの）
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int TopColorId = Shader.PropertyToID("_TopColor");

    [Header("Dependencies")]
    private TileController _tileController;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();

        // 初期状態のセット
        currentBaseColor = baseColor;
        currentTopColor = baseColor;
    }

    private void Start()
    {
        _tileController = GetComponent<TileController>();
        if (_tileController == null) throw new Exception("TileControllerがありません。");

        RefreshVisual();
    }

    private void Update()
    {
        if (_tileController.isSelected)
        {
            Blink(blinkAllyColor);
            return;
        }

        if (_tileController.isTargeted)
        {
            Blink(blinkEnemyColor);
            return;
        }
    }

    public void RefreshVisual()
    {
        if (_tileController.isSelected) return;

        if (_tileController.owner == TileController.TileOwner.Enemy && !_tileController.isRevealed)
        {
            currentBaseColor = invisibleColor;
            currentTopColor = invisibleColor;
        }
        else
        {
            currentBaseColor = baseColor;
            currentTopColor = baseColor;
        }     
        ApplyColors();
    }

    private void Blink(Color blinkColor)
    {
        float time = Mathf.PingPong(Time.time, 1.0f);
        Color normalColor;
        if (_tileController.owner == TileController.TileOwner.Enemy && !_tileController.isRevealed)
        {
            normalColor = invisibleColor;
        }
        else
        {
            normalColor = baseColor;
        }  

        currentTopColor = Color.Lerp(normalColor, blinkColor, time);

        ApplyColors();
    }

    private void ApplyColors()
    {
        // objectRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor(BaseColorId, currentBaseColor);
        propBlock.SetColor(TopColorId, currentTopColor);
        objectRenderer.SetPropertyBlock(propBlock);
    }
}
