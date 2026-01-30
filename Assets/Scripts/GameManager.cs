using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEditor.Overlays;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("フェーズ管理")]
    public Phase currentPhase = Phase.INIT;
    public enum Phase
    {
        INIT,
        PREPARATION,
        BATTLE,
        GAMEOVER,
    };

    [Header("操作管理")]
    [Tooltip("ローディング状態")]
    public bool IsLoading;
    
    // [Tooltip("メインビュー操作可否")]
    // public bool isMainViewEnabled = true;

    // 準備フェーズ共通メッセージ
    private string initMessage => "Please place the remaining " + (_mapManager.maxHqCount - _mapManager.AllyHqCount) + " headquarters units.";

    // 依存関係
    private MapManager _mapManager;
    private UIManager _uiManager;
    private DialogController _dialogController;
    private InfomationController _infomationController;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 依存関係処理
        ResolveDependencies();
        // INITフェーズ時に条件満たした際に実行する処理の登録
        _mapManager.OnHqCountChanged += ValidateAndShowDialog;
        // インフォメーションの表示
        _infomationController.Open(initMessage);
        // メニューの初期化
        _uiManager.SwitchMenu(currentPhase);
    }

    private void ResolveDependencies()
    {
        _mapManager = MapManager.Instance;
        _uiManager = UIManager.Instance;
        _dialogController = DialogController.Instance;
        _infomationController = InfomationController.Instance;
    }

    private void ValidateAndShowDialog(int allyHqCount)
    {
        if (allyHqCount < _mapManager.maxHqCount)
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

        if (allyHqCount == _mapManager.maxHqCount)
        {
            _infomationController.Close();
            _dialogController.Open(
                isConfirm: true,
                message: "Setup OK?",
                onConfirm: () =>
                {
                    _mapManager.OnHqCountChanged -= ValidateAndShowDialog;
                    ChangePhase(Phase.PREPARATION);
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
        for (int x = 0; x < _mapManager.mapWidth; x++)
        {
            for (int z = 0; z < _mapManager.mapHeight; z++)
            {
                if (_mapManager.playerMapData[x, z].unitObject)
                {
                    _mapManager.playerMapData[x, z].DestroyUnit();
                }
            }
        }
    }

    private void ChangePhase(Phase phase)
    {
        currentPhase = phase;
        _uiManager.SwitchMenu(phase);
    }
}
