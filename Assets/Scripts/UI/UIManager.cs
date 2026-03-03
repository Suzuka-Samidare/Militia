using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mode = GameManager.Mode;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // インスペクターから各パネルを登録
    [SerializeField] private VisibilityController initMenu;
    [SerializeField] private VisibilityController preparationMenu;
    [SerializeField] private VisibilityController attackMenu;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SwitchMenu(Mode mode)
    {
        initMenu.SetVisible(mode == Mode.INIT);
        preparationMenu.SetVisible(mode == Mode.PREPARATION);
        attackMenu.SetVisible(mode == Mode.ATTACK);
    }
}
