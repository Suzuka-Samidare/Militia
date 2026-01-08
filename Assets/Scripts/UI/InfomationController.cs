using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfomationController : MonoBehaviour
{
    public static InfomationController Instance;
    public TextMeshProUGUI messageText;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Open(string message)
    {
        messageText.text = message;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        messageText.text = "";
    }

    public void UpdateMessage(string message)
    {
        messageText.text = message;
    }
}
