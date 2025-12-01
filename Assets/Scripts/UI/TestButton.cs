using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    private DialogController _dialogController;
    
    void Start()
    {
        _dialogController = DialogController.instance;
    }

    public void Onclick()
    {
        Debug.Log("TestButton");

        if (_dialogController)
        {
            _dialogController.Open(
                isConfirm: true,
                message: "TESTEST",
                onConfirm: () =>
                {
                    Debug.Log("CONFIRM!!");
                },
                onCancel: () =>
                {
                    Debug.Log("CANCEL!!");
                }
            );   
        }
        else
        {
            Debug.Log(_dialogController);
        }
    }
}
