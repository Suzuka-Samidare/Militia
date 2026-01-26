using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using MapId = MapManager.MapId;

public class TileController : MonoBehaviour
{
    [Header("タイルステータス")]
    public bool isSelected;

    [Header("タイル情報")]
    [Tooltip("ワールド座標")]
    public Vector3 globalPos;
    [Tooltip("グリッド座標X")]
    public int gridPosX;
    [Tooltip("グリッド座標Z")]
    public int gridPosZ;

    [Header("ユニット情報")]
    [Tooltip("ユニットオブジェクト")]
    public GameObject unitObject;
    [Tooltip("ユニットコントローラ")]
    public UnitController unitController => unitObject ? unitObject.GetComponent<UnitController>() : null;
    [Tooltip("ユニットコントローラ（呼出中）")]
    public CallingUnitController calllingUnitController => unitObject ? unitObject.GetComponent<CallingUnitController>() : null;

    [Tooltip("マップ情報")]
    public MapId unitMapId =>
        unitController ? unitController.profile.id :
        calllingUnitController ? calllingUnitController.profile.id :
        MapId.Empty;

    [Tooltip("ユニットの有無")]
    public bool isExistUnit => unitObject;


    [Header("フォーカスビジュアル関連")]
    private Renderer objectRenderer;
    public Color defaultColor;
    public Color focusColor;
    public Color currentColor;
    private float startTime;
    private bool isFadingToReverse;

    [Header("Asset")]
    public GameObject tempUnit;

    private MapManager _mapManager;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        defaultColor = objectRenderer.material.color;
        startTime = Time.time;
    }

    void Start()
    {
        ResolveDependencies();
    }

    void Update()
    {
        UpdateFocusVisual();
    }

    private void ResolveDependencies()
    {
        _mapManager = MapManager.Instance;
    }

    public void SpawnUnitDelayed(BaseUnitData unitData)
    {
        if (unitObject != null) return;

        // 呼び出し中の仮ユニットの作成
        Vector3 tilePosition = gameObject.transform.position;
        Vector3 TempUnitPosition = new Vector3(tilePosition.x, 0.75f, tilePosition.z);
        unitObject = Instantiate(tempUnit, TempUnitPosition, Quaternion.identity, gameObject.transform);
        // マップデータの更新を促す
        _mapManager.isDirty = true;

        // 呼び出し完了時の処理
        Action onCompleteCallback = async () =>
        {
            // 仮ユニットの除去
            Destroy(unitObject);
            while (unitObject != null) {
                await Task.Yield();
            }
            // 本命ユニットの作成
            SpawnUnit(unitData);
        };
        // 仮ユニットの初期化処理
        calllingUnitController.Initialize(unitData.callingProfile, onCompleteCallback);
    }

    public void SpawnUnit(BaseUnitData unitData)
    {
        if (unitObject != null) return;

        Vector3 tilePosition = gameObject.transform.position;
        Vector3 UnitPosition = new Vector3(tilePosition.x, unitData.initPos.y, tilePosition.z);
        unitObject = Instantiate(unitData.prefab, UnitPosition, Quaternion.identity, gameObject.transform);
        unitController.Initialize(unitData.profile);

        // マップデータの更新を促す
        _mapManager.isDirty = true;

        Debug.Log("本オブジェクト配置完了");
    }

    public async void DestroyUnit()
    {
        Destroy(unitObject);
        while (unitObject != null) {
            await Task.Yield();
        }
        // マップデータの更新を促す
        _mapManager.isDirty = true;
    }

    private void UpdateFocusVisual()
    {
        if (isSelected)
        {
            Blink();
        }
        else
        {
            objectRenderer.material.color = defaultColor;
        }
    }

    private void Blink()
    {
        float duration = 1.0f;
        float elapsedTime = (Time.time - startTime) / duration;

        // LERPを使ってカラーを補間
        if (!isFadingToReverse)
        {
            currentColor = Color.Lerp(defaultColor, focusColor, elapsedTime);
        }
        else
        {
            currentColor = Color.Lerp(focusColor, defaultColor, elapsedTime);
        }

        // マテリアルカラーを更新
        objectRenderer.material.color = currentColor;

        // フェードが完了したら、次のフェードの準備
        if (elapsedTime >= 1.0f)
        {
            // 次にフェードする方向を切り替える
            isFadingToReverse = !isFadingToReverse;
            // 新しいフェードの開始時間をリセット
            startTime = Time.time;
        }
    }
}
