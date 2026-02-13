using System;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private UnitStats _stats;

    private void Start()
    {
        _stats = GetComponent<UnitStats>();
    }

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
    private void Faint()
    {
        Debug.Log("死亡演出スタート！");
        // オブジェクトの破棄や非アクティブ化など
    }

    public void ApplyDamage(float damage) => UpdateHp(-damage);
    
    public void ApplyHeal(float healAmount) => UpdateHp(healAmount);

    public void ExecuteAttack(int centerY, int centerX)
    {
        UnitProfile profile = _stats.profile;

        if (!profile.canAttack) return; // 攻撃不可なら即リターン

        if (profile.isAreaAttack)
        {
            // 範囲攻撃：汎用メソッドに Action を渡して実行！
            TileRangeUtil.ForEachSquareRange(centerY, centerX, profile.areaAttackRange, (y, x) =>
            {
                ApplyDamage(centerY, centerX, profile.power);
            });
        }
        else
        {
            // 単体攻撃
            ApplyDamage(centerY, centerX, profile.power);
        }
    }

    private void ApplyDamage(int centerY, int centerX, float damage)
    {
        // ここでタイル位置にいる敵を検知してダメージを与える処理
        Debug.Log($"({centerX}, {centerY}) の敵に {damage} ダメージ！");
    }
}
