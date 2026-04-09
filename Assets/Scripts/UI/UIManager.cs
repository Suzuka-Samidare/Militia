using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using State = GameManager.State;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("MainView")]
    [SerializeField] private VisibilityController Timeline;

    // インスペクターから各パネルを登録
    [Header("Sidebar")]
    [SerializeField] private VisibilityController initMenu;
    [SerializeField] private VisibilityController preparationMenu;
    [SerializeField] private VisibilityController commandMenu;
    [SerializeField] private TextMeshProUGUI _elapsedTime;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateElapsedTime(string text)
    {
        _elapsedTime.text = text;
    }

    public void SwitchMenu(State state)
    {
        initMenu.SetVisible(state == State.INIT);
        preparationMenu.SetVisible(state == State.PREPARATION);
        commandMenu.SetVisible(state == State.COMMAND);
        Timeline.SetVisible(state != State.INIT);
    }
}
