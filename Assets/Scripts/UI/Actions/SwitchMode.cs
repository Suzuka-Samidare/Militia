using UnityEngine;
using Mode = GameManager.Mode;

public class SwitchMode : MonoBehaviour, IButtonAction
{
    [SerializeField] private Mode _nextMode;

    public void Execute() {
        GameManager.Instance.SwitchMode(_nextMode);
    }
}
