using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackSubmit : MonoBehaviour, IButtonAction
{
    [Header("Refs")]
    private AttackManager _attackManager;

    private void Start()
    {
        _attackManager = AttackManager.Instance;
    }

    public void Execute()
    {
        _attackManager.RegisterAttack();
    }
}
