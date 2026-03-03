using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSubmit : MonoBehaviour, IButtonAction
{
    public void Execute()
    {
        Debug.Log("Attack");
    }
}
