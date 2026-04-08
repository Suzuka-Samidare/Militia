using UnityEngine;
using TMPro;

public class PanelView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textObj;

    private void Start()
    {
        textObj = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateText(string text)
    {
        textObj.text = text;
    }
}
