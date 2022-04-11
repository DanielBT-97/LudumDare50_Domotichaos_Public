using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {
    #region Events
    public UnityEvent OnTimerEnded;
    #endregion
    
    #region Type Declaration
    #endregion

    #region Private Variables
    private float _timeLeft = 0f;
    private bool _timerStarted = false;
    private bool _running = false;
    #endregion

    #region Getters & Setters
    public float TimeLeft => _timeLeft;

    public float CountdownTime {
        get => _countdownTime;
        set {
            _countdownTime = value;
            UpdateTimerUI();
        }
    }

    #endregion

    #region Serialized Variables
    [SerializeField] private float _countdownTime = 10f;
    [SerializeField] private bool _startOnEnable = false;
    [SerializeField] private bool _hasUI = false;
    [SerializeField] private TextMeshProUGUI _timerText = null;
    #endregion

    #region Private Functions
    private void UpdateTimerUI() {
        _timerText.text = Mathf.FloorToInt(_timeLeft).ToString("00");
    }
    #endregion

    #region API Methods
    public float GetTimePassed() {
        return _countdownTime - _timeLeft;
    }

    public void ToggleTimer() {
        _running = !_running;
    }

    [ContextMenu("StartTimer")]
    public void StartTimer() {
        if(_hasUI) _timerText.gameObject.SetActive(true);
        _timeLeft = _countdownTime;
        _timerStarted = true;
        _running = true;
    }

    public void ResumeTimer() {
        _running = true;
    }

    public void PauseTimer() {
        _running = false;
    }
    
    public void StopTimer(bool resetTime = false) {
        if (_hasUI) _timerText.gameObject.SetActive(false);
        _running = false;
        _timerStarted = false;
        _timeLeft = resetTime ? _countdownTime : 0;
    }
    
    public void ResetTimer(bool startTimer = false) {
        _running = false;
        _timerStarted = false;
        _timeLeft = _countdownTime;
        _timerStarted = _running = startTimer;
    }

    public void SetTime(float timer) {
        _countdownTime = timer;
    }
    #endregion

    #region Unity Cycle
    private void OnEnable() {
        _timeLeft = _countdownTime;
        if(_startOnEnable) StartTimer();
    }

    private void Update() {
        if (_timerStarted && _running) _timeLeft -= Time.deltaTime;

        if (_running && _timeLeft <= 0) {
            StopTimer();
            OnTimerEnded?.Invoke();
        }

        if(_hasUI) UpdateTimerUI();
    }
    #endregion

    #region Courrutines
    #endregion
}