using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum Phase
    {
        INIT,
        PREPARATION,
        BATTLE,
        GAMEOVER,
    };
    public Phase phase;
    public Boolean isLoading;
    public Boolean isMainViewEnabled = true;

    private MapManager _mapManager;
    private DialogController _dialogController;
    private LoadingOverlay _loadingOverlay;

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

        phase = Phase.INIT;
    }

    void Start()
    {
        _mapManager = MapManager.Instance;
        _dialogController = DialogController.Instance;
        _loadingOverlay = LoadingOverlay.Instance;

        if (_dialogController == null) Debug.Log("ダイアログインスタンスの取得失敗");
    }

    void Update()
    {
        if (phase == Phase.INIT)
        {
            if (_mapManager.AllyHqCount == 2)
            {
                isMainViewEnabled = false;
                _dialogController.Open(
                    isConfirm: true,
                    message: "Setup OK?",
                    onConfirm: () =>
                    {
                        Debug.Log("CONFIRM!!");
                        ChangePhase(Phase.PREPARATION);
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
