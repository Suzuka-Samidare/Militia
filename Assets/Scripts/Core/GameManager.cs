using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour, IInitializable
{
    public static GameManager Instance { get; private set; }

    [Header("進行管理")]
    public Phase currentPhase = Phase.INIT;
    public enum Phase
    {
        INIT,
        PREPARATION,
        COMMAND,
        ACTION,
        GAMEOVER,
    };
    public int Turn = 0;
    [Tooltip("経過時間タイマー")]
    public Timer _elapsedTimer = new Timer();

    [Header("操作管理")]
    [Tooltip("操作可否")]
    public bool IsInputLocked;
    [Tooltip("ローディング状態")]
    public bool IsLoading;
    
    // [Tooltip("メインビュー操作可否")]
    // public bool isMainViewEnabled = true;

    // 準備フェーズ共通メッセージ
    private string initMessage => "Please place the remaining " + (_mapManager.maxHqCount - _mapManager.PlayerHqCount) + " headquarters units.";

    [Header("Refs")]
    private MapManager _mapManager;
    private AttackManager _attackManager; // AttackManager -> ActionManagerに名前を変えたい
    private UIManager _uiManager;
    private DialogController _dialogController;
    private InfomationController _infomationController;
    private PlayerManager _playerManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && currentPhase == Phase.PREPARATION)
        {
            EnterActionPhase();
        }
        
        if (_elapsedTimer.IsRunning)
        {
            _elapsedTimer.UpdateTick(Time.deltaTime);
            _uiManager.UpdateElapsedTime(_elapsedTimer.RemainingTimeStr);
        }
    }

    private void ResolveDependencies()
    {
        _mapManager = MapManager.Instance;
        _attackManager = AttackManager.Instance;
        _uiManager = UIManager.Instance;
        _dialogController = DialogController.Instance;
        _infomationController = InfomationController.Instance;
        _playerManager = PlayerManager.Instance;
    }
    
    public Task Initialize()
    {
        // 依存関係処理
        ResolveDependencies();
        // INITフェーズ時に条件満たした際に実行する処理の登録
        _mapManager.OnHqCountChanged += ValidateAndShowDialog;
        // インフォメーションの表示
        _infomationController.Open(initMessage);
        // メニューの初期化
        _uiManager.SwitchMenu(currentPhase);
        _uiManager.Turn.SetVisible(true);
        _uiManager.Phase.SetVisible(true);
        // 準備時間終了後の処理を登録
        _elapsedTimer.OnTimerComplete += EnterActionPhase;

        return Task.CompletedTask;
    }

    private void ValidateAndShowDialog(int playerHqCount)
    {
        if (playerHqCount < _mapManager.maxHqCount)
        {
            bool isActiveInfo = _infomationController.gameObject.activeSelf;
            if (isActiveInfo)
            {
                _infomationController.UpdateMessage(initMessage);
            }
            else
            {
                _infomationController.Open(initMessage);
            }
        }

        if (playerHqCount == _mapManager.maxHqCount)
        {
            _infomationController.Close();
            _dialogController.Open(
                isConfirm: true,
                title: "Confirm",
                message: "Setup OK?",
                onConfirm: () =>
                {
                    // アクションイベントの後片付け
                    _mapManager.OnHqCountChanged -= ValidateAndShowDialog;
                    _uiManager.ElapsedTime.SetVisible(true);
                    _uiManager.Timeline.SetVisible(true);
                    EnterPreparationPhase();
                },
                onCancel: () =>
                {
                    DestroyAllUnit();
                    _infomationController.Open(initMessage);
                }
            ); 
        }
    }

    

    // TODO: MapManagerに置くべきか検討する
    private void DestroyAllUnit()
    {
        for (int y = 0; y < _mapManager.mapHeight; y++)
        {
            for (int x = 0; x < _mapManager.mapWidth; x++)
            {
                if (_mapManager.playerMapData[x, y].unitObject)
                {
                    _mapManager.playerMapData[x, y].DestroyUnit();
                }
            }
        }
    }

    private void EnterPreparationPhase()
    {
        // ターン加算
        Turn++;
        // ステータスのリジェネ開始
        _playerManager.StartRegen();
        // タイマー設定
        _elapsedTimer.OnTimerComplete += EnterActionPhase;
        _elapsedTimer.Start(180.0f);
        // UIの切り替え
        SwitchPhase(Phase.PREPARATION);
    }

    private async void EnterActionPhase()
    {
        IsInputLocked = false;
        _playerManager.StopRegen();
        _elapsedTimer.Reset();
        SwitchPhase(Phase.ACTION);
        // await PhaseAnnouncerView.Open()

        if (_attackManager.TimelineCount > 0)
        {
            await _attackManager.ProcessTimeline();
            _infomationController.Open("All attacks processed.");
            await Task.Delay(2000);
        }
        else
        {
            _infomationController.Open("No pending attacks.");
            await Task.Delay(2000);
        }

        _infomationController.Close();
        CameraMovement.Instance.SetDestination(TileManager.Instance.PlayerMapLastViewedPosition);
        EnterPreparationPhase();
        IsInputLocked = true;
    }

    public void SwitchPhase(Phase phase)
    {
        currentPhase = phase;
        _uiManager.SwitchMenu(phase);
        _uiManager.UpdatePhase(phase.ToString());
    }
}
