using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckMouseDown();
        }
    }

    void CheckMouseDown()
    {
        // // UIがクリックされている場合は、ゲームオブジェクトのクリック処理をしない
        // if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     Debug.Log("UIクリック判定");
        //     return;
        // }

        // ポインターがUIの上にない場合、ゲームオブジェクトのクリック処理を実行
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Ray判定あり");

            if (hit.collider.gameObject == this.gameObject)
            {
                // このゲームオブジェクトがクリックされた
                Debug.Log("ゲームオブジェクトがクリックされました！");
            }
        }
        else
        {
             Debug.Log("Ray判定なし");
        }
    }
}
