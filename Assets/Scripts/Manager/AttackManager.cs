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
    private List<AttackCommand> timeline = new List<AttackCommand>();

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

    /// <summary>
    /// 攻撃の実行
    /// </summary>
    private void ExecuteAttack(AttackCommand command)
    {
        Debug.Log("TODO: 攻撃実行");

        for (int i = 0; i < command.Targets.Count; i++)
        {
            TileController target = command.Targets[i];
            if (target.isExistUnit)
            {
                Debug.Log($"{target} に {command.Damage} ダメージ！ ぶちかましたよ！✨");
                target.unitController.ApplyDamage(command.Damage);
            }
            else
            {
                Debug.Log("ターゲットがもういないみたい。攻撃スカった！");
            }
        }
        // // マップ参照
        // TileController targetTileController = _mapManager.enemyMapData[command.Target.y, command.Target.x];
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
        timeline.Add(newAttack);
        timeline.Sort((a, b) => b.time.CompareTo(a.time));
        _timelinePresenter.UpdateTimeline(timeline);
        
        Debug.Log($"攻撃予約完了！");
    }
}
