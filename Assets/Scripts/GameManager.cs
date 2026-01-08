using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEditor.Overlays;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("フェーズ管理")]
    public Phase phase = Phase.INIT;
    public enum Phase
    {
        INIT,
        PREPARATION,
        BATTLE,
        GAMEOVER,
    };

    [Header("操作管理")]
    [Tooltip("ローディング状態")]
    public Boolean isLoading;
    [Tooltip("メインビュー操作可否")]
    public Boolean isMainViewEnabled = true;

    private MapManager _mapManager;
    private DialogController _dialogController;
    private LoadingOverlay _loadingOverlay;
    private InfomationController _infomationController;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _mapManager = MapManager.Instance;
        _dialogController = DialogController.Instance;
        _loadingOverlay = LoadingOverlay.Instance;
        _infomationController = InfomationController.Instance;

        _infomationController.Open("Please place the remaining " + (_mapManager.maxHqCount - _mapManager.AllyHqCount) + " headquarters units.");
    }

    void Update()
    {
        if (phase == Phase.INIT)
        {
            if (_mapManager.AllyHqCount == _mapManager.maxHqCount)
            {
                isMainViewEnabled = false;
                _dialogController.Open(
                    isConfirm: true,
                    message: "Setup OK?",
                    onConfirm: () =>
                    {
                        ChangePhase(Phase.PREPARATION);
                        PlayerManager.Instance.StartRegen();
                    },
                    onCancel: () =>
                    {
                        _dialogController.Close();
                    }
                ); 
            }
        }

        UpdateLoadingOverlay();
    }

    void ChangePhase(Phase nextPhase)
    {
        phase = nextPhase;
    }

    private void UpdateLoadingOverlay()
    {
        if (isLoading)
        {
            _loadingOverlay.Open();
        }
        else
        {
            _loadingOverlay.Close();
        }
    }
}
