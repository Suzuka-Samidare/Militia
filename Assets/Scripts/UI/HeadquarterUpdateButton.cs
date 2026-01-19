using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEngine;

public class HeadquarterUpdateButton : BaseButton
{
    public BaseUnitData unitData;

    private GameManager _gameManager;
    private MapManager _mapManager;
    private TileManager _tileManager;

    void Start()
    {
        ResolveDependencies();
    }

    void Update()
    {
        CheckButtonInteractable();
    }

    public void Onclick()
    {
        OnTileUpdateRequested();
    }

    private void ResolveDependencies()
    {
        _gameManager = GameManager.Instance;
        _mapManager = MapManager.Instance;
        _tileManager = TileManager.Instance;
    }

    private void OnTileUpdateRequested()
    {
        _tileManager.SetSelectedTileOnUnit(unitData);
        _mapManager.GetPlayerHeadquartersCount();
        Debug.Log("OnTileUpdateRequested");
    }

    // public async void OnTileUpdateRequested()
    // {
    //     if (_gameManager.isLoading) return;

    //     try
    //     {
    //         Debug.Log($"タイル更新開始");
    //         _gameManager.isLoading = true;

    //         // Unity側の更新 (ここは必ずメインスレッドで動く)
    //         await _tileManager.SetSelectedTileOnUnit(unitData);

    //         // _mapManager.AllyHqCount++;
    //         _mapManager.GetPlayerHeadquartersCount();
            
    //         Debug.Log("タイル更新成功");

    //         InfomationController.Instance.UpdateMessage(
    //             "Please place the remaining " + (_mapManager.maxHqCount - _mapManager.AllyHqCount) + " headquarters units."
    //         );
    //     }
    //     catch (Exception ex)
    //     {
    //         // 通信エラーやタイムアウトのハンドリング
    //         Debug.LogError($"タイル更新失敗: {ex.Message}");
    //         // 必要に応じてユーザーに通知（ダイアログ表示など）
    //     }
    //     finally
    //     {
    //         // 3. JSのfinallyと同じ：成否に関わらず必ず状態を戻す
    //         _gameManager.isLoading = false;
    //         // LoadingOverlay.Instance.Close();
    //         Debug.Log("タイル更新処理終了（後片付け完了）");
    //     }
    // }

    // ボタンの有効化制御
    private void CheckButtonInteractable()
    {
        if (_gameManager.isMainViewEnabled && _tileManager.selectedTile != null)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
