using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapId = MapManager.MapId;

public class RetreatButton : BaseButton
{
    private MapManager _mapManager;
    private TileManager _tileManager;

    void Start()
    {
        _mapManager = MapManager.Instance;
        _tileManager = TileManager.Instance;
    }

    void Update()
    {
        CheckButtonInteractable();
    }
    
    public void Onclick()
    {
        OnTileDeleteRequested();
    }

    private async void OnTileDeleteRequested()
    {
        try
        {
            LoadingOverlay.Instance.Open();
            GameManager.Instance.isLoading = true;
            Debug.Log($"タイル更新開始");

            await MapManager.Instance.UpdateTileAsync(MapId.Empty);

            TileManager.Instance.ClearSelectedTileOnUnit();

            Debug.Log("タイル更新成功");
        }
        catch (Exception ex)
        {
            // 通信エラーやタイムアウトのハンドリング
            Debug.LogError($"タイル更新失敗: {ex.Message}");
            // 必要に応じてユーザーに通知（ダイアログ表示など）
        }
        finally
        {
            // 3. JSのfinallyと同じ：成否に関わらず必ず状態を戻す
            GameManager.Instance.isLoading = false;
            LoadingOverlay.Instance.Close();
            Debug.Log("タイル更新処理終了（後片付け完了）");
        }
    }

    private void CheckButtonInteractable()
    {
        if (GameManager.Instance.isMainViewEnabled
            || _tileManager.selectedTile == null
            || _mapManager.GetSelectedTileId() == MapId.Empty
            || _mapManager.GetSelectedTileId() == MapId.Headquarter)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
