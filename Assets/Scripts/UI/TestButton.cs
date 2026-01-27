using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public void Onclick()
    {
        GameManager.Instance.IsLoading = !GameManager.Instance.IsLoading;
    }
}
