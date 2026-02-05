using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(VisibilityController))]
public class UnitDetailController : VisibilityController
{
    public static UnitDetailController Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private VisibilityController _isCallingBadge;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;

    private VisibilityController _visibility;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _visibility = GetComponent<VisibilityController>();
    }

    public void Open(string unitName, float maxHp, float currenthp, bool isCalling)
    {
        _nameText.text = unitName;
        _isCallingBadge.SetVisible(isCalling);
        _hpSlider.maxValue = maxHp;
        _hpSlider.value = currenthp;
        _hpText.text = $"{currenthp} / {maxHp}";

        _visibility.Show();
    }

    public void Close()
    {
        _visibility.Hide();
    }

    // public void UpdateDetail(string unitName, int maxHp, int currenthp, bool isCalling)
    // {
    //     _nameText.text = unitName;
    //     _isCallingBadge.SetVisible(isCalling);
    //     _hpSlider.maxValue = maxHp;
    //     _hpSlider.value = currenthp;
    //     _hpText.text = $"{currenthp} / {maxHp}";
    // }
}
