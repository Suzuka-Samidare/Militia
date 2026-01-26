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
    public bool isLoading;
    // [Tooltip("メインビュー操作可否")]
    // public bool isMainViewEnabled = true;

    private MapManager _mapManager;
    private DialogController _dialogController;
    private LoadingOverlay _loadingOverlay;
    private InfomationController _infomationController;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ResolveDependencies();
        _mapManager.OnHqCountChanged += ValidateAndShowDialog;
        _infomationController.Open("Please place the remaining " + (_mapManager.maxHqCount - _mapManager.AllyHqCount) + " headquarters units.");
    }

    void Update()
    {
        UpdateLoadingOverlay();
    }
    // switch(phase)
    // {
    //     case Phase.INIT:
    //         // Debug.Log("Phase: INIT");
    //         break;
    //     case Phase.PREPARATION:
    //         // Debug.Log("Phase: PREPARATION");
    //         break;
    //     case Phase.BATTLE:
    //         // Debug.Log("Phase: BATTLE");
    //         break;
    //     case Phase.GAMEOVER:
    //         // Debug.Log("Phase: GAMEOVER");
    //         break;
    //     default:
    //         throw new Exception("Phase: ERROR");
    // }

    private void ResolveDependencies()
    {
        _mapManager = MapManager.Instance;
        _dialogController = DialogController.Instance;
        _loadingOverlay = LoadingOverlay.Instance;
        _infomationController = InfomationController.Instance;
    }

    private void ValidateAndShowDialog(int allyHqCount)
    {
        if (allyHqCount < _mapManager.maxHqCount)
        {
            bool isActiveInfo = _infomationController.gameObject.activeSelf;
            string message = "Please place the remaining " + (_mapManager.maxHqCount - allyHqCount) + " headquarters units.";
            if (isActiveInfo)
            {
                _infomationController.UpdateMessage(message);
            }
            else
            {
                _infomationController.Open(message);
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

    private void ChangePhase(Phase nextPhase)
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
