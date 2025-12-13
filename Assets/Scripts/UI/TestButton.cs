using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    private DialogController _dialogController;
    private MapManager _mapManager;
    
    void Start()
    {
        // _dialogController = DialogController.Instance;
        _mapManager = MapManager.Instance;
    }

    public void Onclick()
    {
        Debug.Log("TestButton");
        Debug.Log(_mapManager.AllyHqCount);

        // if (_dialogController)
        // {
        //     _dialogController.Open(
        //         isConfirm: true,
        //         message: "TESTEST",
        //         onConfirm: () =>
        //         {
        //             Debug.Log("CONFIRM!!");
        //         },
        //         onCancel: () =>
        //         {
        //             Debug.Log("CANCEL!!");
        //         }
        //     );   
        // }
        // else
        // {
        //     Debug.Log(_dialogController);
        // }
    }
}
