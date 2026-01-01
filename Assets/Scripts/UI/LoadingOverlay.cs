using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingOverlay : VisibilityController
{
    public static LoadingOverlay Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance.isLoading)
        {
            Debug.Log("AAAAAAA");
            Open();
        }
        else
        {
            Debug.Log("BBBBBBB");
            Close();
        }
    }
}
