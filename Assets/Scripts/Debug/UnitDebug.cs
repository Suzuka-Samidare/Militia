using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unityエディタ時のみusingする必要がある
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UnitDebug : MonoBehaviour
{
    // Unityエディタ時のみGizmo表示を行う
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // オブジェクトの位置に「オブジェクト名 : 位置」という形式でラベル表示
        Handles.Label(transform.position, $"{name} : {transform.position}");
    }
#endif
}
