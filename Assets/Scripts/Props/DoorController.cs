using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
    [SerializeField] private Transform _doorTransform = null;
    [SerializeField] private Animator _animator = null;

    private float dot = 0f;
    private string _currentAnimation = "OpenInwards";
    private bool _isOpened = false;

    public void Interact() {
        Transform playerTransform = CharacterMovement.Instance.PlayerTransform;
        dot = Vector2.Dot(new Vector2(playerTransform.position.x, playerTransform.position.z) - new Vector2(_doorTransform.position.x, _doorTransform.position.z)
                                , _doorTransform.up);

        if (Mathf.Sign(dot) <= 0) {
            OpenInwards();
        }
        else {
            OpenOutwards();
        }
    }
    
    public void PlayerEnteredArea() {
        _animator.SetBool("PlayerIsInArea", true);
    }

    public void PlayerLeftArea() {
        //CloseDoor();
        _animator.SetBool("PlayerIsInArea", false);
        _isOpened = false;
    }

    private void OpenInwards() {
        if (_isOpened) return;
        
        _isOpened = true;
        _currentAnimation = "OpenInwards";
        _animator.Play("OpenInwards");
        //_animator.speed = 1;
    }

    private void OpenOutwards() {
        if (_isOpened) return;
        
        _isOpened = true;
        _currentAnimation = "OpenOutwards";
        _animator.Play("OpenOutwards");
        //_animator.speed = 1;
    }

    public void DoorClosed() {
        _isOpened = false;
    }
}
