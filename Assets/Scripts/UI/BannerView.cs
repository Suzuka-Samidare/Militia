using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class BannerView : MonoBehaviour
{
    private TextMeshProUGUI _bannerText;
    private Animator _animator;
    private VisibilityController _visibility;

    void Awake()
    {
        _bannerText = GetComponentInChildren<TextMeshProUGUI>();
        _animator = GetComponent<Animator>();
        _visibility = GetComponent<VisibilityController>();

        if (_bannerText == null || _animator == null || _visibility == null)
        {
            throw new Exception("バナーパーツの初期化処理に失敗しました。");
        }
    }

    public async UniTask PlayAnnouncement(string text)
    {
        _bannerText.text = text;

        _visibility.Show();
        _animator.SetTrigger("Play");

        await UniTask.WaitUntil(() => IsAnimationFinished("Close"));

        _animator.Rebind();
        _animator.Update(0f);
    }

    private bool IsAnimationFinished(string stateName)
    {
        // 現在のステート情報を取得（0はBase Layer）
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 1. 指定したステート名であること
        // 2. normalizedTime（再生率）が 1.0（100%）を超えていること
        // 3. 遷移中（Transition）ではないこと
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f && !_animator.IsInTransition(0);
    }
}
