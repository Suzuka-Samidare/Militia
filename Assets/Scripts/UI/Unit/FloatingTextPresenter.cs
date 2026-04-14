using System.Threading.Tasks;
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
    public async Task SpawnDamageAsync(Transform tileTranform, float amount)
    {
        await CreateTextAsync(tileTranform, amount, damageColor);
    }

    public async Task SpawnRecoveryAsync(Transform tileTranform, float amount)
    {
        await CreateTextAsync(tileTranform, amount, recoveryColor);
    }

    // 共通の生成処理
    private async Task CreateTextAsync(Transform tileTranform, float amount, Color color)
    {
        GameObject floatingText = Instantiate(Prefab, CanvasTransform);
        FloatingTextView floatingTextView = floatingText.GetComponent<FloatingTextView>();
        await floatingTextView.SetupAsync(tileTranform, amount, color);
    }
}
