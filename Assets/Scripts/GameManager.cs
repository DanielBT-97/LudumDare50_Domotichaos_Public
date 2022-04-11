using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _instance = null;
    public static GameManager Instance => _instance;

    [Header("MenuManager")] 
    [SerializeField] private EventSystem _eventSystem = null;
    [SerializeField] private GameObject _pauseMenuFirstButton = null;
    [SerializeField] private GameObject _gameOverMenuFirstButton = null;
    
    [Header("TimePlayed")] 
    [SerializeField] private Animator _playerAnimator = null;
    [SerializeField] private TMPro.TextMeshProUGUI _timePlayedText = null;
    [SerializeField] private bool _gameStarted = false;
    [SerializeField] private float _timePlayedFullDifficulty = 300f;
    [SerializeField] private float _timePlayedFullAngry = 300f;
    private float _timePlayed = 0f;
    private float _difficultyValue = 0f;
    private float _howAngry = 0f;

    [Header("ElectricityMechanic")] 
    [SerializeField] private float _maximumVats = 10f;
    [SerializeField] private TypeText _typeTextController = null;
    [SerializeField] private UnityEvent OnVatsExceded, OnVatsRestored;
    private bool _isVatsExceeded = false;
    private float _currentVats = 0f;

    [Header("GAME OVER")] 
    [SerializeField] private UnityEvent OnGameOver;
    [SerializeField] private float _delayBetweenVatsExceededAndGameOver = 5f;
    [SerializeField] private bool _gameOverReached = false;
    private IEnumerator _gameOverDelayedRoutine = null;

    [Header("PAUSE")]
    [SerializeField] private bool _isGamePaused = false;
    [SerializeField] private UnityEvent OnPause, OnUnpause;
    
    public float DifficultyValue => _difficultyValue;
    public bool IsGameOver => _gameOverReached;
    public bool IsGamePaused => _isGamePaused;

    public void ObjectHasBeenTurnedOn(float vatsUsage, string message) {
        _currentVats = Mathf.Clamp(_currentVats + vatsUsage, 0, float.MaxValue);
        _typeTextController.StartTyping(message);

        if (!_gameOverReached && !_isVatsExceeded && _currentVats >= _maximumVats) {
            OnVatsExceded?.Invoke();
            _isVatsExceeded = true;
            if (_gameOverDelayedRoutine == null) {
                _gameOverDelayedRoutine = DelayedGameOver(_delayBetweenVatsExceededAndGameOver);
                StartCoroutine(_gameOverDelayedRoutine);
            }
        }
    }
    
    public void ObjectHasBeenTurnedOff(float vatsUsage) {
        _currentVats = Mathf.Clamp(_currentVats - vatsUsage, 0, float.MaxValue);
        
        if (!_gameOverReached && _isVatsExceeded && _currentVats < _maximumVats) {
            _isVatsExceeded = false;
            OnVatsRestored?.Invoke();
            if (_gameOverDelayedRoutine != null) {
                StopCoroutine(_gameOverDelayedRoutine);
                _gameOverDelayedRoutine = null;
            }
        }
    }

    private void ManageTimePlayed() {
        if (!_gameStarted || _isGamePaused || _gameOverReached) return;
        
        _timePlayed += Time.deltaTime;
        _timePlayedText.text = $"{_timePlayed.ToString("0")}s";
        _howAngry = Mathf.Clamp01(_timePlayed / _timePlayedFullAngry);
        _difficultyValue = Mathf.Clamp01(_timePlayed / _timePlayedFullDifficulty);
        
        _playerAnimator.SetFloat("HowAngry", _howAngry);
    }

    public void StartGame() {
        _gameStarted = true;
    }

    private void PauseGame() {
        _isGamePaused = true;
        _eventSystem.SetSelectedGameObject(_pauseMenuFirstButton);
        OnPause?.Invoke();
    }

    public void ResumeGame() {
        _isGamePaused = false;
        OnUnpause?.Invoke();
    }

    private void GameOver() {
        if (_gameOverReached) return;
        
        Debug.Log("WELP... YOU TRIED :)");
        _eventSystem.SetSelectedGameObject(_gameOverMenuFirstButton);
        OnGameOver?.Invoke();
        _gameOverReached = true;
        //StartCoroutine(DelayedGoToMainMenu(2f));
    }
    
    public void ReplayGame() {
        if (SceneManager.sceneCount < 1) SceneManager.LoadScene(0); 
        SceneManager.LoadScene(1);
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene(0);
    }

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        StartGame();
    }

    private void Update() {
        if (Input.GetButtonDown("PauseToggle") || (_isGamePaused && Input.GetButtonDown("Cancel"))) {
            _isGamePaused = !_isGamePaused;
            if (_isGamePaused) {
                PauseGame();
            } else {
                ResumeGame();
            }
        }

        if (_isGamePaused) return;

        if(ElectricityBarController.Instance) ElectricityBarController.Instance.CurrentElectricityValue = Mathf.Clamp01(_currentVats / _maximumVats);
        ManageTimePlayed();
    }

    private IEnumerator DelayedGameOver(float delay = 2f) {
        Debug.Log("TOO MANY VATS QUICKLY TURN OFF SOME STUFF");
        float timeLeft = delay;
        while (timeLeft > 0f) {
            if (!_isGamePaused) {
                timeLeft -= Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }

        //yield return new WaitForSeconds(delay);
        GameOver();
    }
    
    private IEnumerator DelayedGoToMainMenu(float delay = 2f) {
        Debug.Log("LOAD MAIN MENU");
        yield return new WaitForSeconds(delay);
        GoToMainMenu();
    }
}
