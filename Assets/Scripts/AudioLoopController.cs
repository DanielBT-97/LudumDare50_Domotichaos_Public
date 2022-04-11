using System;
using UnityEngine;
using System.Collections;
 
[RequireComponent(typeof(AudioSource))]
public class AudioLoopController : MonoBehaviour
{
    public AudioSource musicSource = null;
    public AudioClip engineStartClip;
    public AudioClip engineLoopClip;

    private float _manualTiming = 47.3f;
    private float _timeLeft = 0f;
    private bool _switchedAudio = false;
    private Vector2 _pitchMinMax = new Vector2(1, 1.5f);
    
    void Start()
    {
        //StartCoroutine(playEngineSound());
        
        musicSource.clip = engineStartClip;
        musicSource.Play();
        //musicSource.time = 30f;
        _timeLeft = musicSource.clip.length;
        //_timeLeft = _manualTiming;
    }

    private void Update() {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused) return;
        
        _timeLeft -= Time.deltaTime;

        if (!_switchedAudio && _timeLeft <= 0) {
            _switchedAudio = true;
            musicSource.clip = engineLoopClip;
            musicSource.Play();
        }

        if (_switchedAudio) {
            musicSource.pitch = Mathf.Lerp(_pitchMinMax.x, _pitchMinMax.y, GameManager.Instance.DifficultyValue);
        }
    }

    IEnumerator playEngineSound()
    {
        musicSource.clip = engineStartClip;
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.clip = engineLoopClip;
        musicSource.Play();
    }
}
