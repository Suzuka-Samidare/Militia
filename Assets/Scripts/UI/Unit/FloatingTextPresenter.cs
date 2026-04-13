using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextPresenter : MonoBehaviour
{
    public static FloatingTextPresenter Instance { get; private set; }

    [Header("Settings")]
    public GameObject Prefab;
    public Transform CanvasTransform;

    [Header("Color Palette")]
    public Color damageColor = Color.red;     // インスペクターで好きな赤を選んでね！
    public Color recoveryColor = Color.green; // インスペクターで好きな緑を！

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 呼び出し用のメソッドを2つ作っちゃうのが一番分かりやすい！
    public void SpawnDamage(Transform target, float amount)
    {
        Create(target, amount, damageColor);
    }

    public void SpawnRecovery(Transform target, float amount)
    {
        Create(target, amount, recoveryColor);
    }

    // 共通の生成処理
    private void Create(Transform target, float amount, Color color)
    {
        GameObject floatingText = Instantiate(Prefab, CanvasTransform);
        FloatingTextView floatingTextView = floatingText.GetComponent<FloatingTextView>();
        floatingTextView.Setup(target, amount, color);
    }
}
