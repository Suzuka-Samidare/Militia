using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance { get; private set; }

    // 攻撃の情報をまとめたクラス
    public class AttackCommand
    {
        public List<TileController> Targets;    // 攻撃対象の中心タイル
        public float Damage;        // ダメージ量
        public float RemainingTime; // 実行までの残り時間

        public AttackCommand(List<TileController> tiles, float damage, float delay)
        {
            Targets = tiles;
            Damage = damage;
            RemainingTime = delay;
        }
    }

    [SerializeField, Tooltip("攻撃待ちのキュー（FIFO）")]
    public Queue<AttackCommand> attackQueue { get; private set; } = new Queue<AttackCommand>();

    [Header("Refs")]
    private TileManager _tileManager;

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
        _tileManager = TileManager.Instance;
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

    /// <summary>
    /// 攻撃の実行
    /// </summary>
    private void ExecuteAttack(AttackCommand command)
    {
        Debug.Log("TODO: 攻撃実行");

        for (int i = 0; i < command.Targets.Count; i++)
        {
            TileController target = command.Targets[i];
            if (target.isExistUnit)
            {
                Debug.Log($"{target} に {command.Damage} ダメージ！ ぶちかましたよ！✨");
                target.unitController.ApplyDamage(command.Damage);
            }
            else
            {
                Debug.Log("ターゲットがもういないみたい。攻撃スカった！");
            }
        }
        // // マップ参照
        // TileController targetTileController = _mapManager.enemyMapData[command.Target.y, command.Target.x];
    }

    /// <summary>
    /// 攻撃を予約する（外部から呼ぶ）
    /// </summary>
    public void EnqueueAttack()
    {
        UnitProfile profile = _tileManager.selectedTileController.unitStats.profile;
        // 攻撃内容を作成してキューに追加
        AttackCommand newAttack = new AttackCommand(
            _tileManager.targetTiles,
            profile.power,
            profile.atkDelay
        );
        attackQueue.Enqueue(newAttack);
        
        Debug.Log($"攻撃予約完了！実行まであと {profile.atkDelay}秒...");
    }
}
