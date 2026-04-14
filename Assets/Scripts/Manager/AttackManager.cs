using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TileOwner = TileController.TileOwner;

public class AttackManager : MonoBehaviour, IInitializable
{
    public static AttackManager Instance { get; private set; }

    // 攻撃の情報をまとめたクラス
    [System.Serializable]
    public class AttackCommand
    {
        public TileOwner Owner;
        public string UnitName;
        public List<TileController> Targets;    // 攻撃対象の中心タイル
        public float Damage;        // ダメージ量
        public float time; // 経過時間 + 適用必要時間

        public AttackCommand(TileOwner owner, string unitName, List<TileController> tiles, float damage, float delay)
        {
            Owner = owner;
            UnitName = unitName;
            Targets = tiles;
            Damage = damage;
            time = delay;
        }
    }

    [SerializeField, Tooltip("攻撃タイムライン")]
    private List<AttackCommand> _timeline = new List<AttackCommand>();

    [Header("Refs")]
    private TileManager _tileManager;
    private TimelinePresenter _timelinePresenter;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && _timeline.Count > 0)
        {
            ProcessTimeline();
        }
    }

    private void ResolveDependencies()
    {
        _tileManager = TileManager.Instance;
        _timelinePresenter = TimelinePresenter.Instance;
    }

    public Task Initialize()
    {
        ResolveDependencies();
        return Task.CompletedTask;
    }

    private async void ProcessTimeline()
    {
        while (_timeline.Count > 0)
        {
            await ExecuteCommandAsync(_timeline[0]);
            _timeline.RemoveAt(0);
            _timelinePresenter.UpdateTimeline(_timeline);

            Debug.Log("コマンド完了、リストから消したよ！✨");
        }

        _timeline.Clear();
    }

    /// <summary>
    /// 攻撃の実行
    /// </summary>
    private async Task ExecuteCommandAsync(AttackCommand command)
    {
        // 演出を管理するタスクのリストを用意（後で全部終わったかチェックするため）
        List<Task> animationTasks = new List<Task>();

        foreach (var target in command.Targets)
        {
            if (target.isExistUnit)
            {
                Task damageTask = target.unitController.ApplyDamageAsync(command.Damage, target.transform);
                // あとで一括待機するためにリストに入れておく
                animationTasks.Add(damageTask);
            }
            else
            {
                // TODO: MISS表記を入れたい -> ユニットによる位置基準ではダメ
                Debug.Log("ターゲットがもういないみたい。攻撃スカった！");
            }
        }

        await Task.WhenAll(animationTasks);
    }

    /// <summary>
    /// 攻撃を予約する（外部から呼ぶ）
    /// </summary>
    public void RegisterAttack()
    {
        UnitProfile profile = _tileManager.selectedTileController.unitStats.profile;
        // 攻撃内容を作成してキューに追加
        AttackCommand newAttack = new AttackCommand(
            _tileManager.selectedTileController.owner,
            profile.unitName,
            new List<TileController>(_tileManager.targetTiles),
            profile.power,
            profile.atkDelay
        );
        _timeline.Add(newAttack);
        _timeline.Sort((a, b) => b.time.CompareTo(a.time));
        _timelinePresenter.UpdateTimeline(_timeline);
        
        Debug.Log($"攻撃予約完了！");
    }
}
