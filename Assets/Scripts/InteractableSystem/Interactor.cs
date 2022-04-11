using UnityEngine;

public class Interactor : MonoBehaviour {
    [SerializeField] private Transform _playerTransform = null;
    [SerializeField] private InteractableObject _currentClosestInteractable = null;
    [SerializeField] private InteractableObject[] _interactables;
    [SerializeField] private float _maxDistanceToInteractable = 1f;
    [SerializeField] private bool _hasInteractableInRange = false;
    
    // private void OnTriggerEnter(Collider other) {
    //     Debug.Log(other);
    //     InteractableObject interactableInArea = null;
    //     if (other.TryGetComponent<InteractableObject>(out interactableInArea)) {
    //         float distanceToCurrentInteractable = float.MaxValue;
    //         if (_closestInteractable != null) {
    //             distanceToCurrentInteractable = Vector3.Distance(this.transform.position, _closestInteractable.transform.position);
    //         }
    //         float distanceToInteractableInArea = Vector3.Distance(this.transform.position, other.transform.position);
    //         if (distanceToInteractableInArea < distanceToCurrentInteractable) {
    //             //if(_hasInteractableInRange) _closestInteractable.PlayerExitedArea();
    //             if(_closestInteractable != null) _closestInteractable.PlayerExitedArea();
    //             _closestInteractable = interactableInArea;
    //             _closestInteractable.PlayerEnteredArea();
    //             //_hasInteractableInRange = true;
    //         }
    //     }
    // }

    private bool CheckForClosestInteractable() {
        InteractableObject currentClosestInteractable = _currentClosestInteractable;
        InteractableObject closestInteractable = null;
        float closestInteractableDistance = float.MaxValue;
        bool foundInteractableInRange = false;

        foreach (InteractableObject interactableObject in _interactables) {
            //float distanceToInteractable = Vector3.Distance(_playerTransform.position, interactableObject.transform.position);
            float distanceToInteractable = Vector3.SqrMagnitude(interactableObject.InteractableTransform.position - _playerTransform.position);
            if (distanceToInteractable <= (interactableObject._distanceToInteract * interactableObject._distanceToInteract) && distanceToInteractable <= closestInteractableDistance) {
                closestInteractable = interactableObject;
                closestInteractableDistance = distanceToInteractable;

                foundInteractableInRange = true;
            }
            
            Debug.DrawLine(_playerTransform.position, interactableObject.InteractableTransform.position, Color.red, 0.1f);
            Debug.DrawLine(_playerTransform.position, _playerTransform.position + (interactableObject.InteractableTransform.position - _playerTransform.position).normalized * interactableObject._distanceToInteract, Color.green, 0.1f);
        }

        if (!foundInteractableInRange && _hasInteractableInRange) { //If no interactables are in range and we had one registered we tell it we exited its area.
            _currentClosestInteractable.PlayerExitedArea();
            _currentClosestInteractable = null;
            _hasInteractableInRange = false;
        } else if (foundInteractableInRange) {  //If we found a interactable close enough
            //If there was already a registered interactable and is a different one to the closest one we "exit" its area.
            if(_hasInteractableInRange && closestInteractable != _currentClosestInteractable) _currentClosestInteractable.PlayerExitedArea();
            _currentClosestInteractable = closestInteractable;
            if(closestInteractable == _currentClosestInteractable) _currentClosestInteractable.PlayerEnteredArea();
        }

        return foundInteractableInRange;
    }

    // private void OnTriggerExit(Collider other) {
    //     Debug.Log(other);
    //     if (other.gameObject == _closestInteractable.gameObject) _closestInteractable = null;
    // }

    public void TurnOffEverything() {
        foreach (InteractableObject interactableObject in _interactables) {
            interactableObject.Deactivate();
        }
    }

    private void Awake() {
        if (_playerTransform == null) _playerTransform = this.transform;
    }

    private void Update() {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused) return;
        
        if (/*Input.GetKeyDown(KeyCode.F) || */Input.GetButtonDown("Submit")) {
            if(_hasInteractableInRange) _currentClosestInteractable.Interact();
            else {
                Debug.Log("No Interactable Nearby");}
        }
    }

    private void FixedUpdate() {
        _hasInteractableInRange = CheckForClosestInteractable();
    }
}
