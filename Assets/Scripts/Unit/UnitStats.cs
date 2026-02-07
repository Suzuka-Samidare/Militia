using System;
using UnityEngine;
using Guid = System.Guid;

public class UnitStats : MonoBehaviour
{
    [Header("静的ステータス")]
    [Tooltip("UID"), SerializeField] private string _uuid;
    [Tooltip("基本プロパティ")] public UnitProfile profile;

    [Header("動的ステータス")]
    [Tooltip("耐久値")] public float hp;  
    [Tooltip("気絶済み")] public bool isFaint;

    void Awake()
    {
        _uuid = Guid.NewGuid().ToString();
    }

    void Update()
    {
        CheckFaint();
    }

    private void CheckFaint()
    {
        isFaint = hp < 1;
    }

    public void Initialize(UnitProfile profile)
    {
        this.profile = profile;
        hp = profile.maxHp;
    }
}
