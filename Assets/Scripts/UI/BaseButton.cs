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

    // void Update()
    // {
    //     CheckButtonInteractable();
    // }

    // private void CheckButtonInteractable()
    // {
    //     if (TileManager.Instance.selectedTile != null)
    //     {
    //         button.interactable = true;
    //     }
    //     else
    //     {
    //         button.interactable = false;
    //     }
    // }
}
