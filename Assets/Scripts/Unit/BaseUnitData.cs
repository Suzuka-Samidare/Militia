using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapId = MapManager.MapId;

[Serializable]
public struct UnitProfile
{
    [Tooltip("ID")] public MapId id;
    [Tooltip("ユニット名")] public string unitName;
    [Tooltip("最大耐久値")] public int maxHp;
}

[Serializable]
public struct CallingProfile
{
    [Tooltip("ID")] public MapId id;
    [Tooltip("ユニット名")] public string unitName;
    [Tooltip("最大耐久値")] public int maxHp;
    [Tooltip("呼び出しコスト")] public int callCost;
    [Tooltip("呼び出し所要時間（秒）")] public float callTime;
}

// 右クリックメニューからアセットを作成するための属性
[CreateAssetMenu(fileName = "BaseUnitData", menuName = "ScriptableObjects/BaseUnitData")]
public class BaseUnitData : ScriptableObject
{
    [Header("共通ステータス")]
    public UnitProfile profile;

    [Header("呼び出し中のステータス")]
    public CallingProfile callingProfile;

    [Header("外見設定")]
    [Tooltip("ゲームオブジェクト")] public GameObject prefab;
    [Tooltip("下部の位置")] public Vector3 initPos;
}
