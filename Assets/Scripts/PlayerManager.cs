using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public TextMeshProUGUI fpText;
    public TextMeshProUGUI energyText;

    [Header("アニマルポイント関連")]
    [Tooltip("所持アニマルポイント")] private float _animalPoint;
    [Tooltip("所持アニマルポイント")]
    public float animalPoint
    {
        get { return _animalPoint; }
        set { _animalPoint = Mathf.Clamp(value, 0, 999); }
    }
    [Tooltip("アニマルポイント回復値")] public float regenAnimalPoint;
    [Tooltip("アニマルポイント回復速度")] public float regenAnimalPointSpeed;

    [Header("エネルギー関連")]
    [Tooltip("所持エネルギー")] private float _energy;
    [Tooltip("所持エネルギー")]
    public float energy
    {
        get { return _energy; }
        set { _energy = Mathf.Clamp(value, 0, 999); }
    }
    [Tooltip("エネルギー回復値")] public float regenEnergy;
    [Tooltip("エネルギー回復速度")] public float regenEnergySpeed;

    void Start()
    {
        StartCoroutine(IncreaseEnergy());
        StartCoroutine(IncreaseAnimalPoint());
    }
    void Update()
    {
        UpdateAnimalPointText();
        UpdateEnergyText();
    }

    public void useAnimalPoint(float value)
    {
        animalPoint += value;
    }

    public void useEnergy(float value)
    {
        energy += value;
    }

    private IEnumerator IncreaseEnergy()
    {
        while (true) // 無限ループ
        {
            // 2秒インターバル
            yield return new WaitForSeconds(regenEnergySpeed);
            // エネルギーを付与
            energy += regenEnergy;
        }
    }

    private IEnumerator IncreaseAnimalPoint()
    {
        while (true) // 無限ループ
        {
            // 2秒インターバル
            yield return new WaitForSeconds(regenAnimalPointSpeed);
            // フレンドポイントを付与
            animalPoint += regenAnimalPoint;
        }
    }

    private void UpdateAnimalPointText()
    {
        if (fpText != null)
        {
            fpText.text = animalPoint.ToString();
        }
        else
        {
            Debug.Log("Textパーツが参照されていません");
        }
    }

    private void UpdateEnergyText()
    {
        if (energyText != null)
        {
            energyText.text = energy.ToString();
        }
        else
        {
            Debug.Log("Textパーツが参照されていません");
        }
    }
}
