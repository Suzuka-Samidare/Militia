using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;

public class UnitCallTimer
{
    public float CurrentTime { get; private set; }
    public bool IsPaused { get; set; } // ここをtrueにすると止まる
    public bool IsRunning { get; private set; }

    // タイマーが終了した時のイベント（必要であれば）
    public event Action OnTimerComplete;

    private float _targetTime;
    private bool _isCountDown;

    // タイマー開始
    public void Start(float seconds, bool countDown = true)
    {
        // Debug.Log(seconds);
        _targetTime = seconds;
        _isCountDown = countDown;
        CurrentTime = countDown ? seconds : 0f;
        IsPaused = false;
        IsRunning = true;
    }

    // 毎フレーム更新 (MonoBehaviourのUpdateから呼ぶ)
    public void UpdateTick(float deltaTime)
    {
        if (!IsRunning || IsPaused) return;

        if (_isCountDown)
        {
            CurrentTime -= deltaTime;
            if (CurrentTime <= 0f)
            {
                CurrentTime = 0f;
                Complete();
            }
        }
        else
        {
            CurrentTime += deltaTime;
            if (CurrentTime >= _targetTime)
            {
                CurrentTime = _targetTime;
                Complete();
            }
        }
    }

    public void Stop() => IsRunning = false;

    private void Complete()
    {
        IsRunning = false;
        OnTimerComplete?.Invoke();
    }
}
