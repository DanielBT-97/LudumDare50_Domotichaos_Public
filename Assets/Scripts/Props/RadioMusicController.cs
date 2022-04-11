using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioMusicController : MonoBehaviour {
    [SerializeField] private AudioSource _audioSource = null;
    private bool _isRadioActivated = false;

    public void RadioActivated() {
        _isRadioActivated = true;
        _audioSource.Play();
    }
    
    public void RadioDeactivated() {
        _isRadioActivated = false;
        _audioSource.Stop();
    }

    public void PauseRadioMusic() {
        _audioSource.Pause();
    }
    
    public void ResumeRadioMusic() {
        if (!_isRadioActivated) return;
        _audioSource.Play();
    }
}
