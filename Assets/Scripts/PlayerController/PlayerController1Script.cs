using System;
using UnityEngine;

public class PlayerController1Script : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _runMultiplier = 1.5f;
    [SerializeField] private bool _canRun = true;
    [Space(10)]
    [SerializeField] private float _riseSpeed = 2f;
    [SerializeField] private float _fallSpeed = 2f;

    [Header("Camera Settings")]
    [SerializeField] private float _mouseSensitivityHorizontal = 10f;
    [SerializeField] private float _mouseSensitivityVertical = 10f;
    [SerializeField] private float _upDownRange = 80.0f;
    [SerializeField] private Transform _camPivot;

    [Header("Player Input Handler")]
    [SerializeField] private PlayerInputHandler _inputHandler;

    /*TO ADD: 
     * Stamina?
     * Impulse Dash?
     * Convert to State Machine
     * Smoother movement (decelleration and acceleration between states)
     * 
     * If there's anything else you want me to add, please let me know and I will do my best to implement it.
    */
    private CharacterController _characterController;
    private Camera _mainCam;

    private Vector3 _currentMovement = Vector3.zero;

    private float _horizontalRotation;
    private float _verticalRotation;

    private void Awake()
    {
        // Get the CharacterController and Camera components
        _characterController = GetComponent<CharacterController>();
        _mainCam = GetComponentInChildren<Camera>();

        // Get the PlayerInputHandler instance
        if (_inputHandler == null)
            _inputHandler = PlayerInputHandler.Instance;

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame (You know what Update() is lol)
    void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        // Update the horizontal and vertical rotation based on the LookInput from the PlayerInputHandler
        _horizontalRotation += _inputHandler.LookInput.x * _mouseSensitivityHorizontal * Time.deltaTime;
        _verticalRotation -= _inputHandler.LookInput.y * _mouseSensitivityVertical * Time.deltaTime;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_upDownRange, _upDownRange);

        // Apply the rotation to the player and camera
        transform.rotation = Quaternion.Euler(0, _horizontalRotation, 0);

        // Apply the vertical rotation to the camera pivot or main camera if the pivot is not assigned
        if (_camPivot != null)
        {
            _camPivot.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        }
        else if (_mainCam != null)
        {
            _mainCam.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        }
    }

    private void HandleMovement()
    {
        // Check for Rise or Sink input and set the vertical movement accordingly uses mathf to add the "slippery" feeling



        float targetVerticalMovement = 0f;

        if (_inputHandler.RiseInput)
        {
            targetVerticalMovement = _riseSpeed;
        }
        else if (_inputHandler.SinkInput)
        {
            targetVerticalMovement = -_fallSpeed;
        }

        _currentMovement.y = Mathf.MoveTowards(
            _currentMovement.y,
            targetVerticalMovement,
            _acceleration * Time.deltaTime
        );

        // Transform the MoveInput from local space to world space and apply the movement speed
        Vector3 localInput = new Vector3(_inputHandler.MoveInput.x, 0f, _inputHandler.MoveInput.y);
        Vector3 worldInput = transform.TransformDirection(localInput) * _moveSpeed;

 

        // Set the horizontal movement based on the MoveInput from the PlayerInputHandler and uses mathf to add the "slippery" feeling
        _currentMovement.x = Mathf.MoveTowards(
            _currentMovement.x,
            worldInput.x,
            _acceleration * Time.deltaTime
        );

        _currentMovement.z = Mathf.MoveTowards(
            _currentMovement.z,
            worldInput.z,
            _acceleration * Time.deltaTime
        );

        // Apply the movement to the CharacterController
        _characterController.Move(_currentMovement * Time.deltaTime);
    }
}
