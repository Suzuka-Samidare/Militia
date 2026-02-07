using System;
using UnityEngine;
using MapId = MapManager.MapId;

[Serializable]
public struct UnitProfile
{
    [Tooltip("ID")] public MapId id;
    [Tooltip("ユニット名")] public string unitName;
    [Tooltip("最大耐久値")] public float maxHp;
}

// 右クリックメニューからアセットを作成するための属性
[CreateAssetMenu(fileName = "BaseUnitData", menuName = "ScriptableObjects/BaseUnitData")]
public class BaseUnitData : ScriptableObject
{
    [Header("本体ユニット関連")]
    [Tooltip("ステータス")] public UnitProfile profile;
    [Tooltip("3Dオブジェクト")] public GameObject prefab;

    [Header("呼び出しユニット関連")]
    [Tooltip("ステータス")]
    public UnitProfile callingProfile = new UnitProfile
    {
        id = MapId.Calling,
        unitName = "",
        maxHp = 10.0f,
    };
    [Tooltip("呼び出しコスト")] public int callCost;
    [Tooltip("呼び出し所要時間（秒）")] public float callTime;
    [Tooltip("3Dオブジェクト")] public GameObject callingPrefab;

    [Header("外見設定")]
    [Tooltip("下部の位置")] public Vector3 initPos;

    private void OnValidate()
    {
        callingProfile.unitName = profile.unitName;
    }
}
