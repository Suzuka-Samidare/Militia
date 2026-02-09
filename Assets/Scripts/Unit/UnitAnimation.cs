using System.Collections;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    public AnimationName currentAnimation;

    // private bool isAnimating = false;
    public bool isBusyAnimating = false;
    public bool isPause = false;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Play(AnimationName stateName)
    {
        _animator.Play(stateName);
    }

    public void Resume()
    {
        _animator.speed = 1;
        isPause = false;
    }

    public void Pause()
    {
        _animator.speed = 0;
        isPause = true;
    }

    public void PlayOnce(AnimationName stateName) {
        if (isBusyAnimating) return;
        StartCoroutine(PlayAndReturnRoutine(stateName));
    }

    private IEnumerator PlayAndReturnRoutine(AnimationName stateName) {
        isBusyAnimating = true;
        // 1. アニメーションを再生
        _animator.Play(stateName, 0, 0f);

        // 2. 1フレーム待機（Animatorの更新を待たないと、前のステート情報が取れてしまう）
        yield return null;

        // 3. 再生が終わるまで待機
        while (true) {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            // 指定したアニメーションが再生中で、かつ再生時間が1.0（100%）を超えたらループ1回分終了
            if (stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f) {
                break;
            }
            yield return null;
        }

        // 4. 元のアニメーションに戻す
        _animator.Play(AnimationName.IdleA);
        isBusyAnimating = false;
    }
}
