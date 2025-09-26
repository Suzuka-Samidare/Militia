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
        GameObject selectedTile = TileManager.Instance.selectedTile;
        if (selectedTile != null)
        {
            objectRenderer.enabled = true;
            Vector3 SelectedTilePos = selectedTile.transform.position;
            transform.position = new Vector3(SelectedTilePos.x, transform.position.y, SelectedTilePos.z);
        }
        else
        {
            objectRenderer.enabled = false;
        }
    }

    void RotateAnimation()
    {
        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
    }
}
