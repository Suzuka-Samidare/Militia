using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AnimationName))]
public class AnimationNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 構造体の中の変数名 "m_value" と完全に一致させること！
        SerializedProperty valueProp = property.FindPropertyRelative("m_value");

        // 保存されている値が正常に取得できなかった場合エラー文を描画
        if (valueProp == null)
        {
            EditorGUI.LabelField(position, label.text, "Error: m_value not found");
            return;
        }

        // プルダウンリストの取得
        string[] options = AnimationName.GetOptions;
        // 保存されている値がリストの何番目か
        int currentIndex = System.Array.IndexOf(options, valueProp.stringValue);
        // 一致する値が無い場合は一番上にある値の番号で初期化する
        if (currentIndex == -1) currentIndex = 0;

        // 描画開始
        EditorGUI.BeginProperty(position, label, property);
        
        // プルダウンの表示と選択した値を取得
        int nextIndex = EditorGUI.Popup(position, label.text, currentIndex, options);

        // 保存されている値と異なっている場合データを更新
        if (nextIndex != currentIndex)
        {
            valueProp.stringValue = options[nextIndex];
        }

        // 描画終了
        EditorGUI.EndProperty();
    }
}
