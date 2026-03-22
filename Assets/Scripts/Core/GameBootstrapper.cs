using UnityEngine;
using System.Collections.Generic;

public class GameBootstrapper : MonoBehaviour
{
    [Header("上から順番に初期化されます")]
    [SerializeField] private List<MonoBehaviour> managerObjects;

    private async void Start()
    {
        Debug.Log("🚀 シーケンス開始...");
        foreach (var obj in managerObjects)
        {
            if (obj is IInitializable manager)
            {
                // 終わるまでここで「待機」する！
                await manager.Initialize(); 
                Debug.Log($"{obj.name} 初期化完了");
            }
        }
        
        Debug.Log("🎉 全てのマネージャーの準備が整いました。メイン処理を開始します！");
    }
}