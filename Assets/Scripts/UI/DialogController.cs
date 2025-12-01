using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public enum DialogType
    {
        Alert,    // OKボタンのみ
        Confirm   // OKボタンとキャンセルボタン
    }

    // UIコンポーネントへの参照（Inspectorで設定）
    public TextMeshProUGUI messageText; // メッセージテキスト
    public Button okButton; // OKボタン
    public Button cancelButton;  // キャンセルボタン

    // 外部から処理を受け取るためのデリゲート
    private Action onConfirmAction;
    private Action onCancelAction;
    public static DialogController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameObject.SetActive(false);
    }

    // 外部からデータを渡してダイアログを開くためのメインメソッド
    public void Open(bool isConfirm, string message, Action onConfirm, Action onCancel = null)
    {
        // 1. データ（表示内容）の設定
        messageText.text = message;

        // 2. コールバック（実行される処理）の設定
        onConfirmAction = onConfirm;
        // AlertタイプでonCancelがnullの場合に備えて、null合体演算子で処理
        onCancelAction = onCancel ?? (() => {}); // nullの場合は空の処理を設定

        // Alertの場合、キャンセルボタンを非表示にする
        cancelButton.gameObject.SetActive(isConfirm);
        
        // Confirmタイプの場合、OKボタンとCancelボタンの間隔を調整する処理を追加できます
        // 例: レイアウトコンポーネントの調整など...
        
        // 4. UIリスナーの登録（共通処理）
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        
        okButton.onClick.AddListener(OnConfirmButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);

        // ダイアログの表示
        gameObject.SetActive(true);
    }
    
    // ボタンクリック時の処理（変更なし）
    private void OnConfirmButtonClicked()
    {
        onConfirmAction?.Invoke(); 
        Close();
    }
    
    private void OnCancelButtonClicked()
    {
        onCancelAction?.Invoke(); 
        Close();
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
