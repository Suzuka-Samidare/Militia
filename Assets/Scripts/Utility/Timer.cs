using UnityEngine;
using System; // Actionを使うために必要！

public class Timer
{
    public float ElapsedTime { get; private set; }
    public float LimitTime { get; set; }
    public bool IsRunning { get; private set; }

    // 残り時間や進捗（0〜1）も取れるようにしておくとUIで便利！
    public float RemainingTime => Mathf.Max(0, LimitTime - ElapsedTime);
    public float Progress => LimitTime > 0 ? Mathf.Clamp01(ElapsedTime / LimitTime) : 0;
    public string ElapsedTimeStr => FormatTime(ElapsedTime);
    public string RemainingTimeStr => FormatTime(RemainingTime);

    // ⏰ 時間が来た時に実行するイベント（コールバック）
    public event Action OnTimeUp;

    public void Start() => IsRunning = true;
    public void Start(float limitTime)
    {
        LimitTime = limitTime;
        IsRunning = true;
    }
    public void Pause() => IsRunning = false;
    public void Reset()
    {
        ElapsedTime = 0f;
        IsRunning = false;
    }

    public void UpdateTick(float deltaTime)
    {
        if (!IsRunning) return;

        ElapsedTime += deltaTime;

        // 目標時間に達したかチェック
        if (LimitTime > 0 && ElapsedTime >= LimitTime)
        {
            ElapsedTime = LimitTime; // 時間がオーバーしないように固定
            IsRunning = false;        // 一旦止める（ループさせたいならここを調整）
            
            // イベントを発火！(?をつけると、登録者がいない時のエラーを防げるよ)
            OnTimeUp?.Invoke();
        }
    }

    private string FormatTime(float time)
    {
        // 分と秒を計算（整数に切り捨て）
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        // 2桁固定（00:00形式）で返す
        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }
}