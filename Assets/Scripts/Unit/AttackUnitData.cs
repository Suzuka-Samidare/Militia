using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 右クリックメニューからアセットを作成するための属性
[CreateAssetMenu(fileName = "AttackUnitData", menuName = "ScriptableObjects/Attack Unit Data")]
public class AttackUnitData : BaseUnitData
{
    [Header("攻撃系ステータス")]
    [Tooltip("攻撃力")] public int attack;
    [Tooltip("消費エネルギー")] public int attackEnergy;
    [Tooltip("敵陣地攻撃の可否")] public bool canAttackEnemyBase;
}
