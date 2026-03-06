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
    [SerializeField, Tooltip("明滅開始時間")]
    private float blinkStartTime;
    [SerializeField, Tooltip("明滅の反転")]
    private bool isFadingToReverse;

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
            Blink();
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

    private void Blink()
    {
        float duration = 1.0f;
        float elapsedTime = (Time.time - blinkStartTime) / duration;

        Color targetColor;
        // LERPを使ってカラーを補間
        if (!isFadingToReverse)
        {
            targetColor = Color.Lerp(baseColor, blinkAllyColor, elapsedTime);
        }
        else
        {
            targetColor = Color.Lerp(blinkAllyColor, baseColor, elapsedTime);
        }

        // マテリアルカラーを更新
        // currentBaseColor = baseColor; // ベースカラーも変える必要があればここを有効化する
        currentTopColor = targetColor;

        ApplyColors();

        // フェードが完了したら、次のフェードの準備
        if (elapsedTime >= 1.0f)
        {
            // 次にフェードする方向を切り替える
            isFadingToReverse = !isFadingToReverse;
            // 新しいフェードの開始時間をリセット
            blinkStartTime = Time.time;
        }
    }

    private void ApplyColors()
    {
        // objectRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor(BaseColorId, currentBaseColor);
        propBlock.SetColor(TopColorId, currentTopColor);
        objectRenderer.SetPropertyBlock(propBlock);
    }
}
