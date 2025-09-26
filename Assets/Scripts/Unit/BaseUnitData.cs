using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnitData : ScriptableObject
{
    [Header("共通ステータス")]
    [Tooltip("ゲームオブジェクト")] public int id;
    [Tooltip("ゲームオブジェクト")] public GameObject unitPrefab;
    [Tooltip("耐久値")] public float hp;
    [Tooltip("最大耐久値")] public float maxHp;
    [Tooltip("最小耐久値")] private float minHp = 0;
    [Tooltip("呼び出し中フラグ")] public bool isCalling;
    [Tooltip("呼び出しコスト")] public int callCost;
    [Tooltip("呼び出し所要時間")] public float callTime;
    [Tooltip("気絶済み")] public bool isFaint;

    // void Update()
    // {
    //     CheckFaint();
    // }

    public void UpdateHp(float value)
    {
        float calcResult = hp + value;
        hp = Mathf.Clamp(calcResult, minHp, maxHp);
        CheckFaint();
    }

    void CheckFaint()
    {
        if (hp == 0)
        {
            isFaint = true;
        }
    }
}
