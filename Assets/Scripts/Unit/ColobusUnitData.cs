using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 右クリックメニューからアセットを作成するための属性
[CreateAssetMenu(fileName = "ColobusUnitData", menuName = "ScriptableObjects/ColobusUnitData")]
public class ColobusUnitData : AttackUnitData
{
    [Header("固有ステータス")]
    [Tooltip("")] public float aaaa;

    // public float GetHitRate()
    // {
    //     return hitRate;
    // }
}
