using System;
using UnityEngine;

public class CallingUnitController : MonoBehaviour
{
    [Header("タイマー")]
    private UnitCallTimer _timer = new UnitCallTimer();

    void Update()
    {
        // タイマーを進める
        _timer.UpdateTick(Time.deltaTime);

        // UI表示などのために現在の時間をログに出す
        // if (_timer.IsRunning)
        // {
        //     Debug.Log($"残り時間: {_timer.CurrentTime}秒");
        // }

        // // スペースキーで一時停止/再開を切り替え
        // if (Input.GetKeyDown(KeyCode.PageUp))
        // {
        //     _timer.IsPaused = !_timer.IsPaused;
        //     Debug.Log(_timer.IsPaused ? "一時停止" : "再開");
        // }
    }

    public void StartTimer(float callTime, Action onCompleteCallback)
    {
        _timer.OnTimerComplete += () =>
        {
            // Debug.Log("アラーム");
            onCompleteCallback?.Invoke();
        };
        _timer.Start(callTime);
    }
}
