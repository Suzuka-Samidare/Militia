using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour, IInitializable
{
    public static GameManager Instance { get; private set; }

    [Header("ステート管理")]
    public State currentState = State.INIT;
    public enum State
    {
        INIT,
        PREPARATION,
        ATTACK,
        GAMEOVER,
    };

    [Header("操作管理")]
    [Tooltip("ローディング状態")]
    public bool IsLoading;
    
    // [Tooltip("メインビュー操作可否")]
    // public bool isMainViewEnabled = true;

    // 準備フェーズ共通メッセージ
    private string initMessage => "Please place the remaining " + (_mapManager.maxHqCount - _mapManager.PlayerHqCount) + " headquarters units.";

    [Header("Refs")]
    private MapManager _mapManager;
    private UIManager _uiManager;
    private DialogController _dialogController;
    private InfomationController _infomationController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void ResolveDependencies()
    {
        _mapManager = MapManager.Instance;
        _uiManager = UIManager.Instance;
        _dialogController = DialogController.Instance;
        _infomationController = InfomationController.Instance;
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
        _uiManager.SwitchMenu(currentState);

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
                message: "Setup OK?",
                onConfirm: () =>
                {
                    _mapManager.OnHqCountChanged -= ValidateAndShowDialog;
                    SwitchState(State.PREPARATION);
                    PlayerManager.Instance.StartRegen();
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

    public void SwitchState(State state)
    {
        currentState = state;
        _uiManager.SwitchMenu(state);
    }
}
