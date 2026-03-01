using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapId = MapManager.MapId;

public class AttackButton : BaseButton
{
    private TileManager _tileManager;
    private CameraMovement _cameraMovement;

    void Start()
    {
        ResolveDependencies();
    }

    void Update()
    {
        CheckButtonInteractable();
    }

    public void Onclick()
    {
        _cameraMovement.isReconMode =  !_cameraMovement.isReconMode;
    }

    private void ResolveDependencies()
    {
        _tileManager = TileManager.Instance;
        _cameraMovement = CameraMovement.Instance;
    }

    private void CheckButtonInteractable()
    {
        if (_tileManager.selectedTile == null ||
            _tileManager.GetSelectedTileMapId() == MapId.Empty ||
            !_tileManager.selectedTileController.unitStats.profile.canAttack)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
