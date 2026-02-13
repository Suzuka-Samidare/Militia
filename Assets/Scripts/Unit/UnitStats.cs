using System;
using UnityEngine;
using Guid = System.Guid;

public class UnitStats : MonoBehaviour
{

    [Header("静的ステータス")]
    [Tooltip("UID"), SerializeField] private string _uuid;
    [Tooltip("基本データ")] public UnitProfile profile;

    [Header("動的ステータス")]
    [Tooltip("耐久値")] public float hp;  

    private void Awake()
    {
        _uuid = Guid.NewGuid().ToString();
    }

    public void Initialize(UnitProfile profile)
    {
        this.profile = profile;
        hp = profile.maxHp;
    }
}
