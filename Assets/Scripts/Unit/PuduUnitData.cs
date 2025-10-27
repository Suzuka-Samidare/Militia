using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 右クリックメニューからアセットを作成するための属性
[CreateAssetMenu(fileName = "PuduUnitData", menuName = "ScriptableObjects/PuduUnitData")]
public class PuduUnitData : BaseUnitData
{
    [Header("固有ステータス")]
    [Tooltip("命中率上昇値")] public float hitRate;

    public float GetHitRate()
    {
        return hitRate;
    }
}
