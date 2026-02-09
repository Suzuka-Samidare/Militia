using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    // private List<string> animationList = new List<string> {
    //     "Attack",
    //     "Bounce",
    //     "Clicked",
    //     "Death",
    //     "Eat",
    //     "Fear",
    //     "Fly",
    //     "Hit",
    //     "Idle_A", "Idle_B", "Idle_C",
    //     "Jump",
    //     "Roll",
    //     "Run",
    //     "Sit",
    //     "Spin/Splash",
    //     "Swim",
    //     "Walk"
    // };
    private bool isAnimating = false;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayOnce(string stateName) {
        if (isAnimating) return;
        StartCoroutine(PlayAndReturnRoutine(stateName));
    }

    private IEnumerator PlayAndReturnRoutine(string stateName) {
        isAnimating = true;
        // 1. アニメーションを再生
        _animator.Play(stateName, 0, 0f);

        // 2. 1フレーム待機（Animatorの更新を待たないと、前のステート情報が取れちゃうから！）
        yield return null;

        // 3. 再生が終わるまで待機
        while (true) {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            // 指定したアニメーションが再生中で、かつ再生時間が1.0（100%）を超えたらループ1回分終了！
            if (stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f) {
                break;
            }
            yield return null;
        }

        // 4. 元のアニメーションに戻す
        _animator.Play("Idle_A");
        isAnimating = false;
        Debug.Log("1回再生したから元に戻したよ！✨");
    }
}
