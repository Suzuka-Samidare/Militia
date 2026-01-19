using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallingUnitController : MonoBehaviour
{
    [Header("静的ステータス")]
    [Tooltip("基本プロパティ")] public CallingProfile profile;
    [Tooltip("UID"), SerializeField] private string _uuid;

    [Header("動的ステータス")]
    [Tooltip("耐久値")] public float hp;

    [Header("タイマー")]
    private UnitCallTimer _timer = new UnitCallTimer();

    void Update()
    {
        // タイマーを進める
        _timer.UpdateTick(Time.deltaTime);

        // UI表示などのために現在の時間をログに出す
        if (_timer.IsRunning)
        {
            Debug.Log($"残り時間: {_timer.CurrentTime}秒");
        }

        // スペースキーで一時停止/再開を切り替え
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            _timer.IsPaused = !_timer.IsPaused;
            Debug.Log(_timer.IsPaused ? "一時停止" : "再開");
        }
    }

    public void Initialize(CallingProfile profile, Action onCompleteCallback)
    {
        this.profile = profile;
        hp = profile.maxHp;

        _timer.OnTimerComplete += () =>
        {
            Debug.Log("アラーム");
            // isCalling = false;
            onCompleteCallback?.Invoke();
        };
        _timer.Start(profile.callTime);
    }
}

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
