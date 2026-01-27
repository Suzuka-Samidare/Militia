using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phase = GameManager.Phase;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // インスペクターから各パネルを登録
    [SerializeField] private VisibilityController initMenu;
    [SerializeField] private VisibilityController preparationMenu;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SwitchMenu(Phase phase)
    {
        initMenu.SetVisible(phase == Phase.INIT);
        preparationMenu.SetVisible(phase == Phase.PREPARATION);
    }
}
