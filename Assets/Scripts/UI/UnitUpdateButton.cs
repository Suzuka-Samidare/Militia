using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEngine;

public class UnitUpdateButton : BaseButton
{
    public BaseUnitData unitData;

    void Update()
    {
        CheckButtonInteractable();
    }

    public void Onclick()
    {
        OnTileUpdateRequested();
    }

    public void OnTileUpdateRequested()
    {
        if (GameManager.Instance.isLoading) return;

        try
        {
            Debug.Log($"タイル更新開始");
            GameManager.Instance.isLoading = true;

            // Unity側の更新 (ここは必ずメインスレッドで動く)
            TileManager.Instance.SetSelectedTileOnUnit(unitData);
            
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
            // LoadingOverlay.Instance.Close();
            Debug.Log("タイル更新処理終了（後片付け完了）");
        }
    }

    // ボタンの有効化制御
    private void CheckButtonInteractable()
    {
        if (GameManager.Instance.isMainViewEnabled && TileManager.Instance.selectedTile != null)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
