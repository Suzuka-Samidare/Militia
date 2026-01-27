using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityController : MonoBehaviour
{
private CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Hide();
    }

    public void SetVisible(bool isVisible)
    {
        // 透明度を切り替え
        _canvasGroup.alpha = isVisible ? 1f : 0f;
        // マウス反応を切り替え
        _canvasGroup.interactable = isVisible;
        // レイキャスト（当たり判定）を切り替え
        _canvasGroup.blocksRaycasts = isVisible;
    }

    public void Show() => SetVisible(true);

    public void Hide() => SetVisible(false);
}
