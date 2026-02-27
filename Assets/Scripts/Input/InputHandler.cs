using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnSelect;     // 短押し：選択
    public static event Action<Vector2> OnMoveUpdate;   // ドラッグ：カメラ平行移動
    public static event Action<Vector2> OnAngleUpdate; // ドラッグ：カメラ回転

    [SerializeField] private float dragThreshold = 10f;
    [SerializeField] private Vector2 _startPos;
    [SerializeField] private bool _isDragging;
    [SerializeField] private bool _isOverUI;
    [SerializeField] private bool _isPrimaryPressing; // 左クリ or 1本指
    [SerializeField] private bool _isSecondaryPressing; // 右クリ or 2本指

    [SerializeField] private GameInputs.PlayerActions controls;

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

    void Update()
    {
        if (_isOverUI) return;

        Vector2 currentPos = InputProvider.Controls.Player.Point.ReadValue<Vector2>();
        Vector2 delta = InputProvider.Controls.Player.Delta.ReadValue<Vector2>();

        // 1. 回転処理（右クリ or 2本指）
        if (_isSecondaryPressing)
        {
            if (delta != Vector2.zero) OnAngleUpdate?.Invoke(delta);
            return; // 回転中は移動させない
        }

        // 2. 移動処理（左クリドラッグ or 1本指ドラッグ）
        if (_isPrimaryPressing)
        {
            if (!_isDragging && Vector2.Distance(_startPos, currentPos) > dragThreshold) _isDragging = true;
            if (_isDragging && delta != Vector2.zero) OnMoveUpdate?.Invoke(delta);
        }
    }
    
    private void StartMainInteract(InputAction.CallbackContext ctx)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            _isOverUI = true;
            return;
        }
        _isOverUI = false;
        _isPrimaryPressing = true;
        _isDragging = false;
        _startPos = controls.Point.ReadValue<Vector2>();
    }

    private void EndMainInteract(InputAction.CallbackContext _)
    {
        if (!_isOverUI && !_isDragging)
        {
            OnSelect?.Invoke(controls.Point.ReadValue<Vector2>());
        }
        _isPrimaryPressing = false;
    }

    private void StartSubInteract(InputAction.CallbackContext _)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            _isOverUI = true; return;
        }
        _isOverUI = false;
        _isSecondaryPressing = true;
    }

    private void EndSubInteract(InputAction.CallbackContext _)
    {
        _isSecondaryPressing = false;
    }
}

// public class InputHandler : MonoBehaviour
// {
//     public static event Action<Vector2> OnSelect;
//     public static event Action<Vector2> OnDragUpdate;
//     public static event Action<Vector2> OnRotateDragUpdate;

//     [SerializeField] private float dragThreshold = 3f;
//     [SerializeField] private Vector2 _startMousePos;
//     [SerializeField] private Vector2 _currentPos;
//     [SerializeField] private bool _isDragging;
//     [SerializeField] private bool _isClickingOnUI;
//     [SerializeField] private bool _isPrimaryPressing;
//     [SerializeField] private bool _isSecondaryPressing;

//     void Start()
//     {
//         // ProviderからActionを購読
//         InputProvider.Controls.Player.PrimaryInteract.started += StartInteract;
//         InputProvider.Controls.Player.PrimaryInteract.canceled += EndInteract;
//         InputProvider.Controls.Player.SecondaryInteract.started += StartViewAngle;
//         InputProvider.Controls.Player.SecondaryInteract.performed += UpdateViewAngle;
//         InputProvider.Controls.Player.SecondaryInteract.canceled += EndViewAngle;

//     }

//     private void Update()
//     {
//         // 押されていない、またはUI操作中なら何もしない
//         if (!_isPrimaryPressing || !_isSecondaryPressing || _isClickingOnUI) return;

//         _currentPos = Mouse.current.position.ReadValue();
        
//         // まだドラッグ判定になっていない場合、しきい値チェック
//         if (!_isDragging)
//         {
//             if (Vector2.Distance(_startMousePos, _currentPos) > dragThreshold)
//             {
//                 _isDragging = true;
//             }
//         }

//         // ドラッグ中なら、毎フレーム移動量（delta）を飛ばす
//         if (_isDragging)
//         {
//             Vector2 delta = Mouse.current.delta.ReadValue();
//             if (delta != Vector2.zero)
//             {
//                 OnDragUpdate?.Invoke(delta);
//             }
//         }
//     }

//     private void StartInteract(InputAction.CallbackContext context)
//     {
//         // Debug.Log("StartInteract");

//         // UIだった場合はフラグ更新して処理終了
//         if (EventSystem.current.IsPointerOverGameObject())
//         {
//             _isClickingOnUI = true;
//             return;
//         }
//         _isPressing = true;
//         _isClickingOnUI = false;
//         _isDragging = false;
//         _startMousePos = Mouse.current.position.ReadValue();
//     }

//     private void EndInteract(InputAction.CallbackContext context)
//     {
//         // Debug.Log("EndInteract");

//         if (!_isClickingOnUI && !_isDragging)
//         {
//             OnSelect?.Invoke(Mouse.current.position.ReadValue());
//         }
//         _isPressing = false;
//         _isClickingOnUI = false;
//         _isDragging = false;
//     }

//     private void StartViewAngle(InputAction.CallbackContext context)
//     {
//         Debug.Log("StartViewAngle");
//     }

//     private void UpdateViewAngle(InputAction.CallbackContext context)
//     {
//         Debug.Log("UpdateViewAngle");
//     }

//     private void EndViewAngle(InputAction.CallbackContext context)
//     {
//         Debug.Log("EndViewAngle");
//     }

//     void OnDestroy()
//     {
//         // 購読解除も忘れずに（メモリリーク防止！）
//         if (InputProvider.Controls != null)
//         {
//             InputProvider.Controls.Player.Interact.started -= StartInteract;
//             InputProvider.Controls.Player.Interact.canceled -= EndInteract;
//             InputProvider.Controls.Player.ViewAngle.started -= StartViewAngle;
//             InputProvider.Controls.Player.ViewAngle.performed -= UpdateViewAngle;
//             InputProvider.Controls.Player.ViewAngle.canceled -= EndViewAngle;
//         }
//     }

//     // private void UpdateInteract(InputAction.CallbackContext context)
//     // {
//     //     Debug.Log("UpdateInteract");

//     //     if (_isClickingOnUI) return;

//     //     _lastMousePos = Mouse.current.position.ReadValue();
//     //     if (Vector2.Distance(_startMousePos, _lastMousePos) > dragThreshold)
//     //     {
//     //         _isDragging = true;
//     //         OnDragUpdate?.Invoke(Mouse.current.delta.ReadValue());
//     //     }
        
//     //     // Vector2 currentPos = Mouse.current.position.ReadValue();
//     //     // if (Vector2.Distance(_startMousePos, currentPos) > dragThreshold)
//     //     // {
//     //     //     _isDragging = true;
//     //     //     OnDragUpdate?.Invoke(Mouse.current.delta.ReadValue());
//     //     // }
//     // }
// }