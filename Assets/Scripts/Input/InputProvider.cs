using System;
using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public static GameInputs Controls { get; private set; }

    private void Awake()
    {
        if (Controls == null)
        {
            Controls = new GameInputs();
            Controls.Enable();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // // マップを切り替えるメソッドを用意
    // public static void SetMode(string mapName)
    // {
    //     Controls.Disable(); // 一旦全部止める（混線を防ぐ）
        
    //     // 特定のマップだけ有効化
    //     if (mapName == "Player") Controls.Player.Enable();
    //     if (mapName == "UI") Controls.UI.Enable();
    // }
}

// using UnityEngine;
// using UnityEngine.InputSystem; // これ忘れないで！

// public class PlayerController : MonoBehaviour
// {
//     private void OnEnable()
//     {
//         // 1. InputManager経由でアクションにメソッドを紐付け（購読）
//         // 「performed」はボタンが押された瞬間のイベントだよ
//         InputManager.Controls.Player.Jump.performed += OnJump;
        
//         // 移動（Vector2）とかは「started」や「canceled」も使うとエモい
//         InputManager.Controls.Player.Move.performed += OnMove;
//         InputManager.Controls.Player.Move.canceled += OnMove;
//     }

//     private void OnDisable()
//     {
//         // 2. 使い終わったら必ず解除！これマジで鉄則！
//         InputManager.Controls.Player.Jump.performed -= OnJump;
//         InputManager.Controls.Player.Move.performed -= OnMove;
//         InputManager.Controls.Player.Move.canceled -= OnMove;
//     }

//     // 実際に動く処理
//     private void OnJump(InputAction.CallbackContext context)
//     {
//         Debug.Log("マジでジャンプした！アゲ！");
//         // ここにジャンプの物理処理とかを書く
//     }

//     private void OnMove(InputAction.CallbackContext context)
//     {
//         Vector2 moveInput = context.ReadValue<Vector2>();
//         // 移動値を反映させる処理
//     }
// }
