using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct AnimationName
{
    // 中身の文字列
    private readonly string _value;

    // コンストラクタをprivateにして、勝手な文字列作成を禁止する
    private AnimationName(string value) => _value = value;

    // 型から文字列への暗黙的な変換（これがあるとstringとしてそのまま使える！）
    public static implicit operator string(AnimationName path) => path._value ?? "";

    public static readonly AnimationName Attack = new AnimationName("Attack");
    public static readonly AnimationName Bounce = new AnimationName("Bounce");
    public static readonly AnimationName Clicked = new AnimationName("Clicked");
    public static readonly AnimationName Death = new AnimationName("Death");
    public static readonly AnimationName Eat = new AnimationName("Eat");
    public static readonly AnimationName Fear = new AnimationName("Fear");
    public static readonly AnimationName Fly = new AnimationName("Fly");
    public static readonly AnimationName Hit = new AnimationName("Hit");
    public static readonly AnimationName IdleA = new AnimationName("Idle_A");
    public static readonly AnimationName IdleB = new AnimationName("Idle_B");
    public static readonly AnimationName IdleC = new AnimationName("Idle_C");
    public static readonly AnimationName Jump = new AnimationName("Jump");
    public static readonly AnimationName Roll = new AnimationName("Roll");
    public static readonly AnimationName Run = new AnimationName("Run");
    public static readonly AnimationName Sit = new AnimationName("Sit");
    public static readonly AnimationName SpinSplash = new AnimationName("Spin/Splash");
    public static readonly AnimationName Swim = new AnimationName("Swim");
    public static readonly AnimationName Walk = new AnimationName("Walk");

    // EqualsとかToStringもオーバーライド
    public override string ToString() => _value;
}
