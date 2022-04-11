using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    private static CharacterMovement _instance = null;
    public static CharacterMovement Instance => _instance;
    
    [SerializeField] private Transform _trans = null;
    [SerializeField] private Rigidbody _rigid = null;
    [SerializeField] private Animator _animator = null;
    
    [SerializeField] private float _movementSpeed = 0.1f;
    [SerializeField] private float _movementSpeedMagnitud = 0f;
    [SerializeField] private Vector2 _movementDirection = new Vector2(0, 0);
    // [SerializeField] private float _accelerationSpeed = 0.01f;
    // [SerializeField] private float _targetMovementSpeed = 0f;
    // [SerializeField] private float _finalMovement = 0f;
    // [SerializeField] private Vector3 _finalMovementVector3 = new Vector3(0, 0, 0);

    [SerializeField] private Vector3 directionlookRotation;
    [SerializeField] private Vector3 inputVector;

    private bool _isWalking = false;

    public Transform PlayerTransform => _trans;
    
    void MovePosition(Vector3 position)
    {
        Vector3 oldVel = _rigid.velocity;
        //Get the position offset
        Vector3 delta = position - _rigid.position;
        //Get the speed required to reach it next frame
        Vector3 vel = delta / Time.fixedDeltaTime;
 
        //If you still want gravity, you can do this
        vel.y = oldVel.y;
 
        //If you want your rigidbody to not stop easily when hit
        //This is however untested, and you should probably use a damper system instead, like using Smoothdamp but only keeping the velocity component
        vel.x = Mathf.Abs(oldVel.x) > Mathf.Abs(vel.x) ? oldVel.x : vel.x;
        vel.z = Mathf.Abs(oldVel.z) > Mathf.Abs(vel.z) ? oldVel.z : vel.z;

        _rigid.velocity = vel;
    }

    private void ManageAnimation() {
        if(_isWalking) _animator.speed = Mathf.Clamp01(_movementDirection.magnitude);
        else _animator.speed = 1;
        
        bool currentAnimatorIsMoving = _animator.GetBool("IsWalking");
        if (currentAnimatorIsMoving != _isWalking) {
            _animator.SetBool("IsWalking", _isWalking);
        }
    }

    private void Awake() {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused) {
            _movementSpeedMagnitud = 0;
            _movementDirection = Vector2.zero;
            ManageAnimation();
            return;
        }
        
        _movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //_movementDirection = _movementDirection.normalized;
        inputVector = new Vector3(-_movementDirection.normalized.y, 0, _movementDirection.normalized.x); // here you must check that this vector in .xy axes is showing in world space in the direction you want (controled by the ABXY controls)!
        Debug.DrawLine(_trans.position, _trans.position + inputVector * 3f, Color.green, 0.1f); // Here some code to check the input vector in scene view
        
        Quaternion lookRotation = _trans.rotation;
        directionlookRotation = inputVector;
        if (_movementDirection.sqrMagnitude <= 0) {
            directionlookRotation = _trans.forward;
            _isWalking = false;
        } else {
            _isWalking = true;
        }
        Quaternion newRotation = Quaternion.LookRotation(directionlookRotation, _trans.up);
        _trans.rotation = Quaternion.Slerp(_trans.rotation, newRotation, Time.deltaTime * 8);

        _movementSpeedMagnitud = _movementDirection.magnitude;
        _movementSpeedMagnitud = Mathf.Clamp01(_movementSpeedMagnitud);
        
        // _targetMovementSpeed = Mathf.Lerp(_targetMovementSpeed, _movementSpeedMagnitud, _accelerationSpeed);
        // _finalMovement = /*_movementSpeedMagnitud * */_targetMovementSpeed * _movementSpeed;
        //_finalMovementVector3 = _trans.forward.normalized * _movementSpeed;
        //_rigid.MovePosition(_trans.position + (_trans.forward.normalized * _movementSpeed));
        //_rigid.MovePosition(_trans.position + (_trans.forward.normalized * (_movementSpeedMagnitud * 0.1f)));

        ManageAnimation();
    }

    private void FixedUpdate() {
        //_rigid.MovePosition(_trans.position + (_trans.forward.normalized * (_movementSpeedMagnitud * _movementSpeed)));
        _rigid.velocity = _trans.forward.normalized * (_movementSpeedMagnitud * _movementSpeed);
    }
}
