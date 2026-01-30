using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfomationController : MonoBehaviour
{
    public static InfomationController Instance { get; private set; }
    public TextMeshProUGUI messageText;

    private VisibilityController _visibility;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _visibility = GetComponent<VisibilityController>();
    }

    public void Open(string message)
    {
        messageText.text = message;
        _visibility.Show();
    }

    public void Close()
    {
        _visibility.Hide();
    }

    public void UpdateMessage(string message)
    {
        messageText.text = message;
    }
}
