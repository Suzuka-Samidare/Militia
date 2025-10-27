using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 右クリックメニューからアセットを作成するための属性
[CreateAssetMenu(fileName = "GeckoUnitData", menuName = "ScriptableObjects/GeckoUnitData")]
public class GeckoUnitData : AttackUnitData
{
    [Tooltip("命中率上昇値")] public float hitRate;

    // public float GetHitRate()
    // {
    //     return hitRate;
    // }
}
