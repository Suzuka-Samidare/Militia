using UnityEngine;
using State = GameManager.State;

public class SwitchState : MonoBehaviour, IButtonAction
{
    [SerializeField] private State _nextState;

    public void Execute() {
        GameManager.Instance.SwitchState(_nextState);
    }
}
