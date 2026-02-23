using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance { get; private set; }
    private MapManager _mapManager;

    // 攻撃の情報をまとめたクラス
    private class AttackCommand
    {
        public List<Vector2Int> Target;    // 攻撃対象の中心タイル
        public float Damage;        // ダメージ量
        public float RemainingTime; // 実行までの残り時間

        public AttackCommand(List<Vector2Int> tiles, float damage, float delay)
        {
            Target = tiles;
            Damage = damage;
            RemainingTime = delay;
        }
    }

    // 攻撃待ちのキュー（FIFO）
    private Queue<AttackCommand> attackQueue = new Queue<AttackCommand>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _mapManager = MapManager.Instance;
    }

    private void Update()
    {
        // キュー内の全攻撃のタイマーを更新（並行してカウントダウン）
        foreach (var attack in attackQueue)
        {
            attack.RemainingTime -= Time.deltaTime;
        }

        // キューが空じゃなくて、先頭の攻撃のタイマーが終了しているかチェック
        while (attackQueue.Count > 0 && attackQueue.Peek().RemainingTime <= 0)
        {
            // 先頭を取り出して攻撃実行！
            AttackCommand readyAttack = attackQueue.Dequeue();
            ExecuteAttack(readyAttack);
        }
    }

    private void ExecuteAttack(AttackCommand command)
    {
        Debug.Log("TODO: 攻撃実行");
        // // マップ参照
        // TileController targetTileController = _mapManager.enemyMapData[command.Target.y, command.Target.x];

        // if (targetTileController.isExistUnit)
        // {
        //     Debug.Log($"{command.Target} に {command.Damage} ダメージ！ ぶちかましたよ！✨");
        //     // ここに実際のダメージ処理を書く（HPを減らすとか）
        // }
        // else
        // {
        //     Debug.Log("ターゲットがもういないみたい。攻撃スカった！");
        // }
    }

    /// <summary>
    /// 攻撃を予約する（外部から呼ぶ）
    /// </summary>
    public void EnqueueAttack(List<Vector2Int> target, float damage, float delay)
    {
        // 攻撃内容を作成してキューに追加
        AttackCommand newAttack = new AttackCommand(target, damage, delay);
        attackQueue.Enqueue(newAttack);
        
        Debug.Log($"攻撃予約完了！実行まであと {delay}秒...");
    }
}
