using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSubmitCondition : MonoBehaviour, IButtonCondition
{
    private TileManager _tileManager;

    private void Start()
    {
        _tileManager = TileManager.Instance;
    }

    public bool CanInteract()
    {
         if ( _tileManager.selectedTile == null) return false;

         return true;
    }
}
