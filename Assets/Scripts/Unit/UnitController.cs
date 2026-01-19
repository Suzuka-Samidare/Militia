using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [Header("静的ステータス")]
    [Tooltip("基本プロパティ")] public UnitProfile profile;
    [Tooltip("UID"), SerializeField] private string _uuid;

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

    public void Initialize(UnitProfile profile)
    {
        this.profile = profile;
        hp = profile.maxHp;
    }

    private void CheckFaint()
    {
        isFaint = hp < 1;
    }

    // public void UpdateHp(float value)
    // {
    //     float calcResult = hp + value;
    //     hp = Mathf.Clamp(calcResult, minHp, maxHp);
    //     CheckFaint();
    // }
}
