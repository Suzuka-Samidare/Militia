using System;
using UnityEngine;

public class UnitRemove : MonoBehaviour, IButtonAction
{
    public void Execute() {
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
}
