using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    // private UnitStats _stats;
    private UnitProfile _profile;

    private void Start()
    {
        // _stats = GetComponent<UnitStats>();
        _profile = GetComponent<UnitStats>().profile;
    }

    private List<Vector2Int> GetTargetTiles(Vector2Int targetPos)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();

        // 単体攻撃ならそのマスだけ
        if (_profile.atkType == AttackType.Single)
        {
            tiles.Add(targetPos);
            return tiles;
        }

        // 範囲攻撃ならTileRangeUtilを使ってリストを埋める
        switch (_profile.atkType)
        {
            case AttackType.Square:
                TileRangeUtil.ForEachSquareRange(targetPos, _profile.atkRange.max, 
                    (y, x) => tiles.Add(new Vector2Int(x, y)));
                break;
            case AttackType.Manhattan:
                TileRangeUtil.ForEachManhattanRange(targetPos, _profile.atkRange.max, 
                    (y, x) => tiles.Add(new Vector2Int(x, y)));
                break;
            case AttackType.Cross:
                // 十字範囲が必要ならここにUtilを追加して呼ぶ感じ！
                break;
        }

        return tiles;
    }

    // 攻撃を実行（予約）するメソッド
    public void ExecuteAttackRequest(Vector2Int targetPos)
    {
        if (!_profile.canAttack) {
            Debug.Log("攻撃可能ユニットではありません");
            return;
        }

        // 1. 形状と範囲に基づいて攻撃対象タイルをリストアップ
        List<Vector2Int> targetTiles = GetTargetTiles(targetPos);

        // 2. 攻撃キューに登録（前回作ったManagerへ）
        // delayは攻撃アニメーションの着弾時間とかを想定
        AttackManager.Instance.EnqueueAttack(targetTiles, _profile.power, 3.0f);

        Debug.Log($"{_profile.unitName} が {_profile.atkType} 範囲で攻撃予約！ ✨");
    }

    // private void UpdateHp(float amount)
    // {
    //     // 1. HPの増減計算
    //     float previousHp = _stats.hp;
    //     _stats.hp = Mathf.Clamp(_stats.hp + amount, 0, _stats.profile.maxHp);

    //     // 2. 変化がなかったら処理終了
    //     if (Mathf.Approximately(previousHp, _stats.hp)) return;

    //     Debug.Log($"{_stats.profile.unitName} のHPが {_stats.hp} になったよ！ (変化量: {amount})");

    //     // 3. 状態に応じた処理の分岐
    //     if (_stats.hp <= 0)
    //     {
    //         Faint();
    //     }
    //     else if (amount > 0)
    //     {
    //         OnHeal();
    //     }
    //     else
    //     {
    //         OnDamage();
    //     }
    // }

    // private void OnDamage() => Debug.Log("痛いっ！エフェクト出すよ！");
    // private void OnHeal() => Debug.Log("回復！キラキラさせるよ！");
    // private void Faint()
    // {
    //     Debug.Log("死亡演出スタート！");
    //     // オブジェクトの破棄や非アクティブ化など
    // }

    // public void ApplyDamage(float damage) => UpdateHp(-damage);
    
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
