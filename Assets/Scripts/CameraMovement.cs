using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance { get; private set; }

    [SerializeField] private Camera cam;
    [SerializeField] private float dragSpeed = 0.01f; // ドラッグの感度

    // [Header("状態設定")]
    // private Vector3 lastMousePosition;
    // private Vector3 currentMousePosition;

    [Header("ターゲット")]
    private Transform focusPoint; // 回転の中心となるプレイヤーオブジェクト

    // [Header("共通設定")]
    // private bool isDragging = false;

    [Header("回転設定")]
    // public bool isClickingOnUI = false;
    // public bool isDragging = false;
    [SerializeField] private float rotationSpeed = 0.3f; // カメラの回転速度
    [SerializeField] private float _currentAngleX; // 現在の水平回転角度
    [SerializeField] private float _currentAngleY; // 現在の垂直回転角度
    [SerializeField] private float minVerticalAngle = 30f; // 垂直方向の最小回転角度
    [SerializeField] private float maxVerticalAngle = 80f; // 垂直方向の最大回転角度

    [Header("距離設定")]
    [SerializeField] private float distance = 5f; // カメラとターゲットの距離
    [SerializeField] private float distanceSmoothSpeed = 10f; // 距離の滑らかさ

    private void Awake()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.Log("カメラコンポーネントの取得失敗");
        }

        if (!cam.orthographic)
        {
            Debug.Log("カメラがOrthographicモードではありません。");
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        // ターゲットが設定されていない場合は、カメラの親オブジェクトをターゲットとする
        if (focusPoint == null) 
        {
            focusPoint = transform.parent;
        }

        HandleAngle(new Vector2(0, 0));
    }

    private void OnEnable() {
        InputHandler.OnMoveUpdate += HandleMove;
        InputHandler.OnAngleUpdate += HandleAngle;
    }

    private void OnDisable() {
        InputHandler.OnMoveUpdate -= HandleMove;
        InputHandler.OnAngleUpdate -= HandleAngle;
    }

    private void HandleMove(Vector2 delta)
    {
        Debug.Log("HandleMove");

        // カメラの移動量を計算 (Y軸方向はZ軸にマッピングすることが多い)
        // スクリーン座標のY軸方向のドラッグをワールド座標のZ軸方向の移動に、
        // スクリーン座標のX軸方向のドラッグをワールド座標のX軸方向の移動にマッピング
        Vector3 moveDirection = new Vector3(-delta.x, 0, -delta.y);
        // Vector3 moveDirection = new Vector3(-deltaMousePosition.x, 0, -deltaMousePosition.y);
        Quaternion rotation = Quaternion.Euler(0, _currentAngleX, 0);

        // カメラの位置を更新
        // Time.deltaTime は不要な場合が多い（マウスデルタ自体がフレーム間の移動量のため）
        focusPoint.Translate(rotation * moveDirection * dragSpeed, Space.World);

        // カメラの移動位置の制限値
        Vector3 focusPointPosition = focusPoint.transform.position;
        float clampedX = Mathf.Clamp(focusPointPosition.x, 0, MapManager.Instance.mapWidth - 1);
        float clampedZ = Mathf.Clamp(focusPointPosition.z, 0, MapManager.Instance.mapHeight - 1);
        
        // clampによる調整があった場合、カメラの位置を修正
        if (focusPointPosition.x != clampedX || focusPointPosition.z != clampedZ)
        {
            Vector3 clampPosition = new Vector3(clampedX, focusPointPosition.y, clampedZ);
            focusPoint.transform.position = clampPosition;
        }
    }

    private void HandleAngle(Vector2 delta)
    {
        Debug.Log("HandleAngle");

        // マウスの移動量を取得
        _currentAngleX += delta.x * rotationSpeed;
        _currentAngleY -= delta.y * rotationSpeed; // Y軸は反転させる

        // 垂直方向の回転角度を制限
        _currentAngleY = Mathf.Clamp(_currentAngleY, minVerticalAngle, maxVerticalAngle);

        // 回転角度からクォータニオンを計算
        Quaternion rotation = Quaternion.Euler(_currentAngleY, _currentAngleX, 0);

        // ターゲットからの相対位置を計算
        Vector3 targetPosition = focusPoint.position;
        Vector3 negDistance = new Vector3(0f, 0f, -distance);
        Vector3 position = rotation * negDistance + targetPosition;

        // カメラの位置と回転を設定
        transform.rotation = rotation;
        transform.position = position;
    }

    // void Awake()
    // {
    //     cam = GetComponent<Camera>();

    //     if (cam == null)
    //     {
    //         Debug.Log("カメラコンポーネントの取得失敗");
    //     }

    //     if (!cam.orthographic)
    //     {
    //         Debug.Log("カメラがOrthographicモードではありません。");
    //     }

    //     if (Instance == null)
    //     {
    //         Instance = this;
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
        
    //     // ターゲットが設定されていない場合は、カメラの親オブジェクトをターゲットとする
    //     if (focusPoint == null) 
    //     {
    //         focusPoint = transform.parent;
    //     }

    //     // 初期角度を設定
    //     UpdateAngle();
    // }

    // void Update()
    // {
    //     MoveCamera();
    //     RotateCamera();
    //     ZoomCamera();
    // }

    // void RotateCamera()
    // {
    //     if (EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1))
    //     {
    //         isClickingOnUI = true;
    //     }

    //     // 左クリックが押されている場合のみ処理を行う
    //     if (Input.GetMouseButton(1))
    //     {
    //         UpdateAngle();
    //     }

    //     // マウスの右ボタンが離された瞬間
    //     if (Input.GetMouseButtonUp(1))
    //     {
    //         isClickingOnUI = false;
    //     }
    //     // else
    //     // {
    //     //     // 右クリックが離されたら、マウスカーソルを表示し、ロックを解除する
    //     //     Cursor.visible = true;
    //     //     Cursor.lockState = CursorLockMode.None;
    //     // }
    // }

    // void UpdateAngle()
    // {
    //     // Debug.Log("UpdateAngle");

    //     // マウスの移動量を取得
    //     currentAngleX += Input.GetAxis("Mouse X") * rotationSpeed;
    //     currentAngleY -= Input.GetAxis("Mouse Y") * rotationSpeed; // Y軸は反転させる

    //     // 垂直方向の回転角度を制限
    //     currentAngleY = Mathf.Clamp(currentAngleY, minVerticalAngle, maxVerticalAngle);

    //     // 回転角度からクォータニオンを計算
    //     Quaternion rotation = Quaternion.Euler(currentAngleY, currentAngleX, 0);

    //     // ターゲットからの相対位置を計算
    //     Vector3 targetPosition = focusPoint.position;
    //     Vector3 negDistance = new Vector3(0f, 0f, -distance);
    //     Vector3 position = rotation * negDistance + targetPosition;

    //     // カメラの位置と回転を設定
    //     transform.rotation = rotation;
    //     transform.position = position;
    // }

    // void MoveCamera()
    // {
    //     // Debug.Log(EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1));

    //     if (EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
    //     {
    //         isClickingOnUI = true;
    //     }

    //     // マウスの右ボタンが押された瞬間
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         lastMousePosition = Input.mousePosition;
    //     }

    //     // マウスの右ボタンが押されている状態かつ、押された瞬間のマウス位置から移動している場合
    //     if (Input.GetMouseButton(0) && lastMousePosition != Input.mousePosition)
    //     {
    //         isDragging = true;
    //     }

    //     // マウスの右ボタンが離された瞬間
    //     if (Input.GetMouseButtonUp(0))
    //     {
    //         isDragging = false;
    //         isClickingOnUI = false;
    //     }

    //     // 現在のマウス位置の取得
    //     currentMousePosition = Input.mousePosition;

    //     // ドラッグ中
    //     if (isClickingOnUI == false && isDragging)
    //     {
    //         // マウスの移動量（デルタ）を計算
    //         Vector3 deltaMousePosition = currentMousePosition - lastMousePosition;

    //         // カメラの移動量を計算 (Y軸方向はZ軸にマッピングすることが多い)
    //         // スクリーン座標のY軸方向のドラッグをワールド座標のZ軸方向の移動に、
    //         // スクリーン座標のX軸方向のドラッグをワールド座標のX軸方向の移動にマッピング
    //         Vector3 moveDirection = new Vector3(-deltaMousePosition.x, 0, -deltaMousePosition.y);
    //         Quaternion rotation = Quaternion.Euler(0, currentAngleX, 0);

    //         // カメラの位置を更新
    //         // Time.deltaTime は不要な場合が多い（マウスデルタ自体がフレーム間の移動量のため）
    //         focusPoint.Translate(rotation * moveDirection * dragSpeed, Space.World);

    //         // カメラの移動位置の制限値
    //         Vector3 focusPointPosition = focusPoint.transform.position;
    //         float clampedX = Mathf.Clamp(focusPointPosition.x, 0, MapManager.Instance.mapWidth - 1);
    //         float clampedZ = Mathf.Clamp(focusPointPosition.z, 0, MapManager.Instance.mapHeight - 1);
            
    //         // clampによる調整があった場合、カメラの位置を修正
    //         if (focusPointPosition.x != clampedX || focusPointPosition.z != clampedZ)
    //         {
    //             Vector3 clampPosition = new Vector3(clampedX, focusPointPosition.y, clampedZ);
    //             focusPoint.transform.position = clampPosition;
    //         }

    //         // 前のマウス位置を更新
    //         lastMousePosition = currentMousePosition;
    //     }
    // }

    // void ZoomCamera()
    // {
    //     if (Input.mouseScrollDelta.y != 0)
    //     {
    //         float clampedSize = Mathf.Clamp(cam.orthographicSize + Input.mouseScrollDelta.y / 10, 2.0f, 3.0f);
    //         cam.orthographicSize = clampedSize;
    //     }
    // }
}