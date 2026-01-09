using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("オブジェクト関連")]
    public TextMeshProUGUI fpText;
    public TextMeshProUGUI energyText;

    [Header("アニマルポイント関連")]
    [SerializeField, Tooltip("所持アニマルポイント")]
    private float _animalPoint;
    public float animalPoint
    {
        get { return _animalPoint; }
        set { _animalPoint = Mathf.Clamp(value, 0, 999); }
    }
    [SerializeField, Tooltip("アニマルポイント回復値")]
    private float apRegenValue = 10.0f;
    [SerializeField, Tooltip("アニマルポイント回復速度")]
    private float apRegenRate = 2.0f;

    [Header("エネルギー関連")]
    [SerializeField, Tooltip("所持エネルギー")]
    private float _energy;
    public float energy
    {
        get { return _energy; }
        set { _energy = Mathf.Clamp(value, 0, 999); }
    }
    [SerializeField, Tooltip("エネルギー回復値")]
    private float energyRegenValue = 4.0f;
    [SerializeField, Tooltip("エネルギー回復速度")]
    private float energyRegenRate = 2.0f;

    [Header("コルーチン参照用")]
    private IEnumerator _increaseEnergy;
    private IEnumerator _increaseAnimalPoint;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _increaseAnimalPoint = IncreaseStatus(apRegenRate, apRegenValue, AddAnimalPoint);
        _increaseEnergy = IncreaseStatus(energyRegenRate, energyRegenValue, AddEnergy);
    }

    void Update()
    {
        UpdateAnimalPointText();
        UpdateEnergyText();
    }

    public void AddAnimalPoint(float value)
    {
        animalPoint += value;
    }

    public void AddEnergy(float value)
    {
        energy += value;
    }

    public void UseAnimalPoint(float value)
    {
        animalPoint -= value;
    }

    public void UseEnergy(float value)
    {
        energy -= value;
    }

    public void StartRegen()
    {
        if (_increaseAnimalPoint == null || _increaseEnergy == null)
        {
            throw new Exception("AP/Energy regeneration process is missing.");
        }

        StartCoroutine(_increaseAnimalPoint);
        StartCoroutine(_increaseEnergy);
    }

    public void StopRegen()
    {
        if (_increaseAnimalPoint == null || _increaseEnergy == null)
        {
            throw new Exception("AP/Energy regeneration process is missing.");
        }

        StopCoroutine(_increaseAnimalPoint);
        StopCoroutine(_increaseEnergy);
    }

    private IEnumerator IncreaseStatus(float interval, float value, Action<float> addAction)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            addAction(value);
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
