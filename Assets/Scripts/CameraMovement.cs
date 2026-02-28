using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance { get; private set; }

    [Header("Ref")]
    [SerializeField] private Transform focusPoint; // 回転の中心となるプレイヤーオブジェクト
    [SerializeField] private Camera cam;

    [Header("共通設定")]
    [SerializeField] private float dragSpeed = 0.01f; // ドラッグの感度

    [Header("移動設定")]

    [Header("ズーム設定（Projection Sizeの変更）")]
    [SerializeField] private float zoomSpeed = 0.02f;
    [SerializeField] private float minZoom = 2.0f;
    [SerializeField] private float maxZoom = 3.26f;

    [Header("回転設定")]
    [SerializeField] private float rotationSpeed = 0.3f; // カメラの回転速度
    [SerializeField] private float _currentAngleX = 0f; // 現在の水平回転角度
    [SerializeField] private float _currentAngleY = 0f; // 現在の垂直回転角度
    [SerializeField] private float minVerticalAngle = 30f; // 垂直方向の最小回転角度
    [SerializeField] private float maxVerticalAngle = 80f; // 垂直方向の最大回転角度

    [Header("距離設定")]
    [SerializeField] private float distance = 5f; // カメラとターゲットの距離（固定）

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
        InputHandler.OnZoomUpdate += HandleZoom;
    }

    private void OnDisable() {
        InputHandler.OnMoveUpdate -= HandleMove;
        InputHandler.OnAngleUpdate -= HandleAngle;
        InputHandler.OnZoomUpdate -= HandleZoom;
    }

    private void HandleMove(Vector2 delta)
    {
        // カメラの移動量を計算 (Y軸方向はZ軸にマッピングすることが多い)
        // スクリーン座標のY軸方向のドラッグをワールド座標のZ軸方向の移動に、
        // スクリーン座標のX軸方向のドラッグをワールド座標のX軸方向の移動にマッピング
        Vector3 direction = new Vector3(-delta.x, 0, -delta.y);
        Quaternion quaternion = Quaternion.Euler(0, _currentAngleX, 0);
        Vector3 translation = quaternion * direction;
        // カメラの位置を更新
        focusPoint.Translate(translation * dragSpeed, Space.World);

        // カメラの移動位置の制限値をクランプしつつ、位置を更新
        if (!MapManager.Instance) {
            throw new Exception("MapManagerのインスタンスがありません。");
        }
        focusPoint.position = new Vector3(
            Mathf.Clamp(focusPoint.position.x, 0, MapManager.Instance.mapWidth - 1),
            focusPoint.position.y,
            Mathf.Clamp(focusPoint.position.z, 0, MapManager.Instance.mapHeight - 1)
        );

        UpdateCam();
    }

    private void HandleAngle(Vector2 delta)
    {
        // マウスの移動量を取得
        _currentAngleX += delta.x * rotationSpeed;
        _currentAngleY -= delta.y * rotationSpeed; // Y軸は反転させる
        // 垂直方向のみ回転角度を制限
        _currentAngleY = Mathf.Clamp(_currentAngleY, minVerticalAngle, maxVerticalAngle);

        UpdateCam();
    }

    private void HandleZoom(float delta)
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + delta * zoomSpeed, minZoom, maxZoom);
        UpdateCam();
    }

    private void UpdateCam()
    {
        // 回転角度からクォータニオンを計算
        Quaternion rotation = Quaternion.Euler(_currentAngleY, _currentAngleX, 0);

        // ターゲットからの相対位置を計算
        Vector3 negDistance = new Vector3(0f, 0f, -distance);
        Vector3 position = rotation * negDistance + focusPoint.position;

        // カメラの位置と回転を設定
        transform.rotation = rotation;
        transform.position = position;
    }
}