using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using MapId = MapManager.MapId;

public class TileController : MonoBehaviour
{
    public enum TileOwner { Ally, Enemy }
    
    [Header("視認タイマー関連")]
    [Tooltip("視認時間"), SerializeField]
    private float revealDuration = 5.0f;
    private float _timer = 5.0f;

    [Header("タイルステータス")]
    [Tooltip("選択状態の有無")]
    public bool isSelected;
    [Tooltip("敵から視認可能か"), SerializeField]
    private bool isRevealed = false;
    [Tooltip("タイルの陣地種別"), SerializeField]
    private TileOwner owner;

    [Header("タイル座標情報")]
    [Tooltip("ワールド座標")]
    public Vector3 globalPos;
    [Tooltip("マップ上座標X")]
    public int gridPosX;
    [Tooltip("マップ上座標Y")]
    public int gridPosY;

    [Header("ユニット情報")]
    [Tooltip("ユニットオブジェクト"), SerializeField]
    private GameObject _unitObject;
    public GameObject unitObject
    {
        get => _unitObject;
        set
        {
            if (_unitObject == value) return;
            _unitObject = value;
            RefreshComponents();
        }
    }
    [field: SerializeField]
    public UnitStats unitStats { get; private set; }
    [field: SerializeField]
    public CallingUnitController callingUnitController { get; private set; }
    [Tooltip("ユニットの有無")]
    public bool isExistUnit => unitObject;
    [Tooltip("マップID")]
    public MapId unitMapId => unitStats ? unitStats.profile.id : MapId.Empty;
    

    [Header("ビジュアル関連")]
    public Color defaultColor;
    public Color focusColor;
    public Color invisibleColor;
    [SerializeField] private Color currentColor;
    private Renderer objectRenderer;
    [SerializeField] private float blinkStartTime;
    [SerializeField] private bool isFadingToReverse;

    private MapManager _mapManager;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        defaultColor = objectRenderer.material.color;
        blinkStartTime = Time.time;
    }

    void Start()
    {
        ResolveDependencies();
    }

    void Update()
    {
        UpdateTileVisual();
        UpdateRevealTimer();
    }

    private void ResolveDependencies()
    {
        _mapManager = MapManager.Instance;
    }

    private void RefreshComponents()
    {
        if (_unitObject != null)
        {
            unitStats = _unitObject.GetComponent<UnitStats>();
            callingUnitController = _unitObject.GetComponent<CallingUnitController>();
        }
        else
        {
            unitStats = null;
            callingUnitController = null;
        }
    }

    public void SetOwner(TileOwner owner)
    {
        this.owner = owner;
    }

    public void Reveal()
    {
        // すでに true の場合でも、タイマーを初期値（最大値）にリセットする
        isRevealed = true;
        _timer = revealDuration;
        
        Debug.Log($"{gameObject.name} が表示されました。タイマーリセット。");
    }

    private void UpdateRevealTimer()
    {
        // 表示中でない、または一時停止中なら何もしない
        if (!isRevealed) return;

        // タイマーを減らす
        _timer -= Time.deltaTime;

        // 0になったら非表示に戻す
        if (_timer <= 0)
        {
            isRevealed = false;
            _timer = 0;
            Debug.Log($"{gameObject.name} が隠れました。");
        }
    }

    public void SpawnUnitDelayed(BaseUnitData unitData)
    {
        if (unitObject != null) return;

        // 呼び出し中の仮ユニットの作成
        Vector3 tilePosition = gameObject.transform.position;
        Vector3 TempUnitPosition = new Vector3(tilePosition.x, 0.75f, tilePosition.z);
        unitObject = Instantiate(unitData.callingPrefab, TempUnitPosition, Quaternion.identity, gameObject.transform);
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
        unitStats.Initialize(unitData.callingProfile);
        callingUnitController.StartTimer(unitData.callTime, onCompleteCallback);
    }

    public void SpawnUnit(BaseUnitData unitData)
    {
        if (unitObject != null) return;

        Vector3 tilePosition = gameObject.transform.position;
        Vector3 UnitPosition = new Vector3(tilePosition.x, unitData.initPos.y, tilePosition.z);
        unitObject = Instantiate(unitData.prefab, UnitPosition, Quaternion.identity, gameObject.transform);
        unitStats.Initialize(unitData.profile);

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

    private void UpdateTileVisual()
    {
        if (isSelected)
        {
            Blink();
        }
        else if (owner == TileOwner.Enemy && !isRevealed)
        {
            objectRenderer.material.color = invisibleColor;
        }
        else
        {
            objectRenderer.material.color = defaultColor;
        }
    }

    private void Blink()
    {
        float duration = 1.0f;
        float elapsedTime = (Time.time - blinkStartTime) / duration;

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
            blinkStartTime = Time.time;
        }
    }
}
