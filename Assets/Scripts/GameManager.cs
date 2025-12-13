using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum Phase
    {
        INIT,
        PREPARATION,
        BATTLE,
        GAMEOVER,
    };
    public Phase phase;
    public Boolean isInputDisabled;

    private MapManager _mapManager;
    private DialogController _dialogController;

    void Awake()
    {
        phase = Phase.INIT;
        isInputDisabled = false;
    }

    void Start()
    {
        _mapManager = MapManager.Instance;
        _dialogController = DialogController.Instance;

        if (_dialogController == null) Debug.Log("ダイアログインスタンスの取得失敗");
    }

    void Update()
    {
        if (phase == Phase.INIT)
        {
            if (_mapManager.AllyHqCount == 2)
            {
                _dialogController.Open(
                    isConfirm: true,
                    message: "Are You Okey?",
                    onConfirm: () =>
                    {
                        Debug.Log("CONFIRM!!");
                    },
                    onCancel: () =>
                    {
                        Debug.Log("CANCEL!!");
                    }
                ); 
            }
        }
    }

    void ChangePhase(Phase nextPhase)
    {
        phase = nextPhase;
    }
}
