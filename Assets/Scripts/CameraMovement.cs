using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float dragSpeed = 0.1f; // ドラッグの感度
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        // マウスの左ボタンが押された瞬間
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        // マウスの左ボタンが離された瞬間
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // ドラッグ中
        if (isDragging)
        {
            // 現在のマウス位置
            Vector3 currentMousePosition = Input.mousePosition;

            // マウスの移動量（デルタ）を計算
            Vector3 deltaMousePosition = currentMousePosition - lastMousePosition;

            // カメラの移動量を計算 (Y軸方向はZ軸にマッピングすることが多い)
            // スクリーン座標のY軸方向のドラッグをワールド座標のZ軸方向の移動に、
            // スクリーン座標のX軸方向のドラッグをワールド座標のX軸方向の移動にマッピング
            Vector3 moveDirection = new Vector3(-deltaMousePosition.x, 0, -deltaMousePosition.y);

            // カメラの位置を更新
            // Time.deltaTime は不要な場合が多い（マウスデルタ自体がフレーム間の移動量のため）
            transform.Translate(moveDirection * dragSpeed, Space.World);

            // 前のマウス位置を更新
            lastMousePosition = currentMousePosition;
        }
    }
}