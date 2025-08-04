using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConeCursorController : MonoBehaviour
{
    private Renderer objectRenderer;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        UpdatePosition();
        RotateAnimation();
    }

    void UpdatePosition()
    {
        if (Selection.activeGameObject && Selection.activeGameObject.CompareTag("Tile"))
        {
            // Debug.Log("true");
            objectRenderer.enabled = true;
            Vector3 SelectedTilePos = Selection.activeGameObject.transform.position;
            transform.position = new Vector3(SelectedTilePos.x, transform.position.y, SelectedTilePos.z);
        }
        else
        {
            // Debug.Log("false");
            objectRenderer.enabled = false;
        }
    }

    void RotateAnimation()
    {
        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
    }
}
