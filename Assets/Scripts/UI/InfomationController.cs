using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfomationController : MonoBehaviour
{
    public static InfomationController instance;
    public TextMeshProUGUI messageText; // メッセージテキスト
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameObject.SetActive(false);
    }

    public void Open(string message)
    {
        messageText.text = message;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
