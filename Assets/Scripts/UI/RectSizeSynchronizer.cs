using System;
using UnityEngine;
using UnityEngine.EventSystems;

// 1. サイズ変更を通知するための軽量なスクリプト
public class RectSizeObserver : UIBehaviour
{
    public Action OnSizeChanged;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        OnSizeChanged?.Invoke();
    }
}

public class RectSizeSynchronizer : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private RectTransform _sourceRect; // オブジェクトB

    [Header("Sync Options")]
    [SerializeField] private bool _syncWidth = false;
    [SerializeField] private bool _syncHeight = false;
    [SerializeField] private float _minWidth = 0f;
    [SerializeField] private float _minHeight = 0f;
    [SerializeField] private float _maxWidth = 1000f;
    [SerializeField] private float _maxHeight = 1000f;

    [Header("Debug")]
    [SerializeField] private RectTransform _myRect; // オブジェクトA（自分）
    [SerializeField] private RectSizeObserver _observer;

    void Awake()
    {
        _myRect = GetComponent<RectTransform>();

        if (_sourceRect != null)
        {
            // _側に検知用コンポーネントを動的に追加
            _observer = _sourceRect.gameObject.AddComponent<RectSizeObserver>();
            // サイズが変わった時に実行する関数を登録
            _observer.OnSizeChanged = SyncSize;
            // 初期状態を合わせておく
            SyncSize();
        }
    }

    private void SyncSize()
    {
        if (_sourceRect == null || _myRect == null) return;

        // 幅を同期する場合
        if (_syncWidth)
        {
            float targetWidth = Mathf.Clamp(_sourceRect.rect.width, _minWidth, _maxWidth);
            _myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        }

        // 高さを同期する場合
        if (_syncHeight)
        {
            float targetHeight = Mathf.Clamp(_sourceRect.rect.height, _minHeight, _maxHeight);
            _myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
        }
    }

    void OnDestroy()
    {
        // メモリリーク防止のため、破棄時にイベントを解除
        if (_observer != null) _observer.OnSizeChanged = null;
    }
}