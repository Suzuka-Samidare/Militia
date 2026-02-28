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
