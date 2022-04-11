using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class InteractableObject : MonoBehaviour {
    [SerializeField] private bool _alwaysOn = false;
    [SerializeField] private Transform _interactCenterTrans = null;
    public Transform InteractableTransform => _interactCenterTrans;
    
    [SerializeField] private float _vatsUsage = 1f;
    [SerializeField] private string _alexaMessage = "MESSAGE";
    [SerializeField] private float _accelerationPercentage = 0.02f;
    [SerializeField] private AnimationCurve _difficultyCurve;
    public float _distanceToInteract = 2f;
    [SerializeField] private Vector2 _turnOnDelayMinMax = new Vector2(4f, 10f);
    private float _startingTurnOnDelay = 0f;
    private float _turnOnDelay = 0f;
    [SerializeField] private Vector2 _countdownDelayMinMax = new Vector2(4f, 10f);
    private float _countdownDelay = 0f;
    
    [SerializeField] private UnityEvent OnActorEnter, OnActorExit, OnActorInteracted, OnActorActivated, OnActorDeactivated;

    private bool _isTurnedOn = false;
    private float _currentCountdown = 0f;
    private float _difficultyValue = 0f;

    public void PlayerEnteredArea() {
        OnActorEnter?.Invoke();
    }
    
    public void PlayerExitedArea() {
        OnActorExit?.Invoke();
    }
    
    public void Interact() {
        if (!_isTurnedOn && !_alwaysOn) return;
        Deactivate();
        OnActorInteracted?.Invoke();
    }

    [ContextMenu("Activate")]
    public void Activate() {
        _isTurnedOn = true;
        GameManager.Instance.ObjectHasBeenTurnedOn(_vatsUsage, _alexaMessage);
        OnActorActivated?.Invoke();
    }
    
    [ContextMenu("Deactivate")]
    public void Deactivate() {
        if (_alwaysOn) return;

        _difficultyValue = _difficultyCurve.Evaluate(GameManager.Instance.DifficultyValue);    // 0 --> 1
        //_difficultyValue = Mathf.Clamp01(_difficultyValue * (1 + _accelerationPercentage)); // 0 --> 1
        _turnOnDelay = _startingTurnOnDelay * Mathf.Clamp(1 - _difficultyValue, 0.1f, 1);
        _isTurnedOn = false;
        _currentCountdown = _turnOnDelay;
        GameManager.Instance.ObjectHasBeenTurnedOff(_vatsUsage);
        OnActorDeactivated?.Invoke();
    }

    private void Awake() {
        if (_interactCenterTrans == null) _interactCenterTrans = this.transform;
        
        _startingTurnOnDelay = Random.Range(_turnOnDelayMinMax.x, _turnOnDelayMinMax.y);
        _turnOnDelay = _startingTurnOnDelay;
        _countdownDelay = Random.Range(_countdownDelayMinMax.x, _countdownDelayMinMax.y);
        
        //if(_alwaysOn) Activate();
    }

    private void Start() {
        _currentCountdown = _turnOnDelay;
    }

    private void Update() {
        if (!_alwaysOn && (!_isTurnedOn && _currentCountdown <= 0)) {
            Activate();
        }

        if(!GameManager.Instance.IsGamePaused && !GameManager.Instance.IsGameOver) _currentCountdown -= Time.deltaTime;

    }
}
