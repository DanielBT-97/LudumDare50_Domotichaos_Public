using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricityBarController : MonoBehaviour {
    private static ElectricityBarController _instance = null;
    public static ElectricityBarController Instance => _instance;
    
    [SerializeField] private Image _electricityBarImage = null;
    [SerializeField] private Animator _animator = null;
    //[SerializeField] private float _maximumSpriteHeight = 500;
    private float _currentElectricityValue = 0;

    /// <summary>
    /// Value has to be 0 to 1.
    /// </summary>
    public float CurrentElectricityValue {
        get => _currentElectricityValue;
        set => _currentElectricityValue = value;
    }

    private void Awake() {
        _instance = this;
    }

    private void Update() {
        //_electricityBarImage.fillAmount = Mathf.Lerp(0, _maximumSpriteHeight, _currentElectricityValue);
        _electricityBarImage.fillAmount = _currentElectricityValue;
        _animator.SetFloat("FilledValue", _currentElectricityValue);
    }
}
