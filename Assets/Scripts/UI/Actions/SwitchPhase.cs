using UnityEngine;
using Phase = GameManager.Phase;

public class SwitchPhase : MonoBehaviour, IButtonAction
{
    [SerializeField] private Phase _nextPhase;

    public void Execute() {
        GameManager.Instance.SwitchPhase(_nextPhase);
    }
}
