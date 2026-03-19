using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using PlayerActions = GameInputs.PlayerActions;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnSelect;     // 短押し：選択
    public static event Action<Vector2> OnMoveUpdate;   // ドラッグ：カメラ平行移動
    public static event Action<Vector2> OnAngleUpdate; // ドラッグ：カメラ回転
    public static event Action<float> OnZoomUpdate;

    [SerializeField] private float dragThreshold = 10f;
    [SerializeField] private Vector2 _startPos;
    [SerializeField] private bool _isDragging;
    [SerializeField] private bool _isOverUI;
    [SerializeField] private bool _isPrimaryPressing; // 左クリ or 1本指
    [SerializeField] private bool _isSecondaryPressing; // 右クリ or 2本指
    [SerializeField] private float _prevPinchDist;

    [SerializeField] private PlayerActions controls;

    void Start()
    {
        controls = InputProvider.Controls.Player;

        // メイン操作（左クリ / 1本指）
        controls.MainInteract.started += StartMainInteract;
        controls.MainInteract.canceled += EndMainInteract;

        // サブ操作（右クリ / 2本指）
        controls.SubInteract.started += StartSubInteract;
        controls.SubInteract.canceled += EndSubInteract;
    }

    private void StartMainInteract(InputAction.CallbackContext ctx)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            _isOverUI = true;
            return;
        }
        // _isOverUI = false;
        // _isDragging = false;
        _isPrimaryPressing = true;
        _startPos = controls.Point.ReadValue<Vector2>();
    }

    private void EndMainInteract(InputAction.CallbackContext _)
    {
        if (!_isOverUI && !_isDragging)
        {
            OnSelect?.Invoke(controls.Point.ReadValue<Vector2>());
        }
        _isOverUI = false;
        _isDragging = false;
        _isPrimaryPressing = false;
    }

    private void StartSubInteract(InputAction.CallbackContext _)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            _isOverUI = true;
            return;
        }
        _isSecondaryPressing = true;
    }

    private void EndSubInteract(InputAction.CallbackContext _)
    {
        _isOverUI = false;
        _isSecondaryPressing = false;
        _prevPinchDist = 0;
    }

    void Update()
    {
        // UI上で操作している場合は処理しない
        if (_isOverUI) return;

        // --- マウスホイール (PC用) ---
        float scroll = controls.Scroll.ReadValue<Vector2>().y;
        if (scroll != 0)
        {
            OnZoomUpdate?.Invoke(scroll * 0.1f);
        }

        // 2. 2本指操作 (モバイル用：回転 & ピンチズーム)
        if (_isSecondaryPressing) 
        {
            // 回転の通知
            Vector2 delta = controls.Delta.ReadValue<Vector2>();
            if (delta != Vector2.zero)
            {
                OnAngleUpdate?.Invoke(delta);
            }

            // ピンチズーム計算 (モバイルのTouch1がある時だけ実行)
            // マウスの右クリック時は Touch1Point がゼロになるので、ここでズームは走らないよ！
            Vector2 t1 = controls.Touch1Point.ReadValue<Vector2>();
            if (t1 != Vector2.zero) 
            {
                Vector2 t0 = controls.Point.ReadValue<Vector2>();
                float currentDist = Vector2.Distance(t0, t1);
                if (_prevPinchDist > 0) 
                {
                    OnZoomUpdate?.Invoke((currentDist - _prevPinchDist) * 0.05f);
                }
                _prevPinchDist = currentDist;
            }
            return; 
        }

        // --- 1本指/左クリ操作 (移動) ---
        if (_isPrimaryPressing) {
            Vector2 currentPos = controls.Point.ReadValue<Vector2>();
            if (!_isDragging && Vector2.Distance(_startPos, currentPos) > dragThreshold)
            {
                _isDragging = true;
            }
            if (_isDragging)
            {
                OnMoveUpdate?.Invoke(controls.Delta.ReadValue<Vector2>());
            }
        }
    }
}