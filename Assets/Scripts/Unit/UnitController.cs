using System;
using System.Collections.Generic;
using UnityEngine;
using AttackCommand = AttackManager.AttackCommand;

public class UnitController : MonoBehaviour
{
    [Header("Refs")]
    private UnitStats _stats;
    private UnitProfile _profile;
    private AttackManager _attackManager;
    private MapManager _mapManager;


    private void Start()
    {
        _stats = GetComponent<UnitStats>();
        _profile = GetComponent<UnitStats>().profile;
        _attackManager = AttackManager.Instance;
        _mapManager = MapManager.Instance;
    }

    public List<Vector2Int> GetTargetTilePositions(Vector2Int targetPos)
    {
        List<Vector2Int> tilePositions = new List<Vector2Int>();

        // 単体攻撃ならそのマスだけ
        if (_profile.atkType == AttackType.Single)
        {
            tilePositions.Add(targetPos);
            return tilePositions;
        }

        // 範囲攻撃ならTileRangeUtilを使ってリストを埋める
        switch (_profile.atkType)
        {
            case AttackType.Square:
                TileRangeUtil.ForEachSquareRange(targetPos, _profile.atkRange.max, 
                    (pos) => tilePositions.Add(pos));
                break;
            case AttackType.Manhattan:
                TileRangeUtil.ForEachManhattanRange(targetPos, _profile.atkRange.max, 
                    (pos) => tilePositions.Add(pos));
                break;
            case AttackType.Cross:
                // 十字範囲が必要ならここにUtilを追加して呼ぶ感じ！
                break;
        }

        return tilePositions;
    }

    // 攻撃を実行（予約）するメソッド
    // public void EnqueueAttackRequest(Vector2Int targetPos)
    // {
    //     if (!_profile.canAttack) {
    //         Debug.Log("攻撃可能ユニットではありません");
    //         return;
    //     }

    //     // 1. 形状と範囲に基づいて攻撃対象タイルをリストアップ
    //     List<Vector2Int> tilePositions = GetTargetTilePositions(targetPos);
    //     List<TileController> tileControllers = new List<TileController>();

    //     foreach (Vector2Int pos in tilePositions)
    //     {
    //         TileController tileController = _mapManager.GetEnemyMapTile(pos);
    //         if (tileController != null)
    //         {
    //             tileControllers.Add(tileController);
    //         }
    //     }

    //     // 2. 攻撃キューに登録（前回作ったManagerへ）
    //     // delayは攻撃アニメーションの着弾時間とかを想定
    //     AttackCommand newAttack = new AttackCommand(tileControllers, _profile.power, 3.0f);
    //     _attackManager.attackQueue.Enqueue(newAttack);

    //     Debug.Log($"{_profile.unitName} が {_profile.atkType} 範囲で攻撃予約！ ✨");
    // }

    private void UpdateHp(float amount)
    {
        // 1. HPの増減計算
        float previousHp = _stats.hp;
        _stats.hp = Mathf.Clamp(_stats.hp + amount, 0, _stats.profile.maxHp);

        // 2. 変化がなかったら処理終了
        if (Mathf.Approximately(previousHp, _stats.hp)) return;

        Debug.Log($"{_stats.profile.unitName} のHPが {_stats.hp} になったよ！ (変化量: {amount})");

        // 3. 状態に応じた処理の分岐
        if (_stats.hp <= 0)
        {
            Faint();
        }
        else if (amount > 0)
        {
            OnHeal();
        }
        else
        {
            OnDamage();
        }
    }

    private void OnDamage() => Debug.Log("痛いっ！エフェクト出すよ！");
    private void OnHeal() => Debug.Log("回復！キラキラさせるよ！");
    private void Faint() => Debug.Log("死亡演出スタート！");

    public void ApplyDamage(float damage) => UpdateHp(-damage);
    
    // public void ApplyHeal(float healAmount) => UpdateHp(healAmount);

    // public void ExecuteAttack(int centerY, int centerX)
    // {
    //     UnitProfile profile = _stats.profile;

    //     if (!profile.canAttack) return; // 攻撃不可なら即リターン

    //     if (profile.isAreaAttack)
    //     {
    //         // 範囲攻撃：汎用メソッドに Action を渡して実行！
    //         TileRangeUtil.ForEachSquareRange(centerY, centerX, profile.areaAttackRange, (y, x) =>
    //         {
    //             ApplyDamage(centerY, centerX, profile.power);
    //         });
    //     }
    //     else
    //     {
    //         // 単体攻撃
    //         ApplyDamage(centerY, centerX, profile.power);
    //     }
    // }

    // private void ApplyDamage(int centerY, int centerX, float damage)
    // {
    //     // ここでタイル位置にいる敵を検知してダメージを与える処理
    //     Debug.Log($"({centerX}, {centerY}) の敵に {damage} ダメージ！");
    // }
}
