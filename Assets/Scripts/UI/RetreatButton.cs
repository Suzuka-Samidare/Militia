using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapId = MapManager.MapId;

public class RetreatButton : BaseButton
{
    // private MapManager _mapManager;
    private TileManager _tileManager;

    void Start()
    {
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

    private void OnTileDeleteRequested()
    {
        try
        {
            GameManager.Instance.IsLoading = true;
            Debug.Log($"タイル更新開始");

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
            GameManager.Instance.IsLoading = false;
            Debug.Log("タイル更新処理終了（後片付け完了）");
        }
    }

    private void CheckButtonInteractable()
    {
        if (_tileManager.selectedTile == null
            || _tileManager.GetSelectedTileMapId() == MapId.Empty
            || _tileManager.GetSelectedTileMapId() == MapId.Headquarter)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
