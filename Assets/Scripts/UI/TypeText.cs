using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TypeText : MonoBehaviour {
    public UnityEvent OnTextFinished;
    
    public TMPro.TextMeshProUGUI _targetTextTMP;
    public UnityEngine.UI.Text _targetText;
    public float _delay = 0.1f, _disappearanceDelay;
    
    private string _currentText = String.Empty;
    private string fullText = String.Empty;

    private IEnumerator _typingCorroutine = null;

    public void StartTyping(string text) {
        if (_typingCorroutine != null) {
            StopCoroutine(_typingCorroutine);
            _currentText = String.Empty;
        }

        fullText = text;
        _currentText = String.Empty;
        _typingCorroutine = ShowText();
        StartCoroutine(_typingCorroutine);
    }

    IEnumerator ShowText() {
        if(_targetTextTMP != null) _targetTextTMP.gameObject.SetActive(true);
        else _targetText.gameObject.SetActive(true);

        //fullText = fullText.Replace('~', '\n');
        
        for (int i = 0; i <= fullText.Length; ++i) {
            _currentText = fullText.Substring(0, i);
            _currentText = _currentText.Replace('~', '\n');
            if(_targetTextTMP != null) _targetTextTMP.text = _currentText;
            else _targetText.text = _currentText;
            yield return new WaitForSeconds(_delay);
        }
        
        yield return new WaitForSeconds(_disappearanceDelay);
        if(_targetTextTMP != null) _targetTextTMP.gameObject.SetActive(false);
        else _targetText.gameObject.SetActive(false);
    }
}
