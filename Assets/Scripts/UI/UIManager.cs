using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Phase = GameManager.Phase;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvas")]
    [SerializeField] private VisibilityController _announcePanel;
    // [SerializeField] private VisibilityController _sidebar;

    [Header("MainView")]
    public VisibilityController Timeline;

    // インスペクターから各パネルを登録
    [Header("Sidebar")]
    public VisibilityController Turn;
    public VisibilityController Phase;
    public VisibilityController ElapsedTime;
    [SerializeField] private VisibilityController _initMenu;
    [SerializeField] private VisibilityController _preparationMenu;
    [SerializeField] private VisibilityController _commandMenu;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateTurn(string text)
    {
        TextMeshProUGUI turnText = ElapsedTime.GetComponentInChildren<TextMeshProUGUI>();
        turnText.text = text;
    }

    public void UpdatePhase(string text)
    {
        TextMeshProUGUI phaseText = Phase.GetComponentInChildren<TextMeshProUGUI>();
        phaseText.text = text;
    }

    public void UpdateElapsedTime(string text)
    {
        TextMeshProUGUI elapsedTimeText = ElapsedTime.GetComponentInChildren<TextMeshProUGUI>();
        elapsedTimeText.text = text;
    }

    public void SwitchMenu(Phase phase)
    {
        _initMenu.SetVisible(phase == GameManager.Phase.INIT);
        _preparationMenu.SetVisible(phase == GameManager.Phase.PREPARATION);
        _commandMenu.SetVisible(phase == GameManager.Phase.COMMAND);
    }

    public async Task PlayAnnouncement(string text)
    {
        TextMeshProUGUI announceText = _announcePanel.GetComponentInChildren<TextMeshProUGUI>();
        Animator animator = _announcePanel.GetComponent<Animator>();
        // if (animator == null) throw new System.Exception("animatorがありません。");

        announceText.text = text;

        _announcePanel.Show();
        animator.SetTrigger("Play");

         // 0番目のレイヤー（Base Layer）の情報を取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // アニメーションの「素の長さ」（秒）を取得
        float duration = stateInfo.length;
        Debug.Log(duration);
    }
}
