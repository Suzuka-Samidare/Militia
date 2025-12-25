using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    [HideInInspector] public Button button;

    void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }
}
