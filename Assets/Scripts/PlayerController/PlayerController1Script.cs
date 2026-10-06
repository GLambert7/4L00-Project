using System;
using UnityEngine;

public class PlayerController1Script : MonoBehaviour
{
    #region Variables
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
    private float _manualVerticalMovement = 0f;

    private Vector3 _currentMovement = Vector3.zero;

    private float _horizontalRotation;
    private float _verticalRotation;
    #endregion

    #region Unity Methods
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
    #endregion

    #region Movement Methods
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

    #region Secret Message
    // If you see this comment and mention it to me (Emily), I'll buy you something from Tims.
    // Sincerely, Emilyyyyyyyyyyyyyyy
    #endregion

    private void HandleMovement()
    {
        // Check for Rise or Sink input and set the vertical movement accordingly uses mathf to add the "slippery" feeling

        if (_inputHandler.RiseInput)
        {
            _manualVerticalMovement = _riseSpeed;
        }
        else if (_inputHandler.SinkInput)
        {
            _manualVerticalMovement = -_fallSpeed;
        }

        

        // Get the camera's forward direction
        Vector3 cameraForward = _mainCam.transform.forward;
        Vector3 cameraRight = _mainCam.transform.right;

        // Transform the MoveInput to world space and apply the movement speed
        Vector3 worldInput = ((cameraRight * _inputHandler.MoveInput.x) + (cameraForward * _inputHandler.MoveInput.y)).normalized * _moveSpeed;

        // If the player is able to run and holding the run input, apply the run multiplier to the movement speed
        if (_canRun && _inputHandler.RunInput)
        {
            worldInput *= _runMultiplier;
        }

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

        // Calculate vertical movement

        if (_inputHandler.RiseInput || _inputHandler.SinkInput)
        {
        _currentMovement.y = Mathf.MoveTowards(
            _currentMovement.y,
            _manualVerticalMovement,
            _acceleration * Time.deltaTime
        );        
        }
        else
        {
            _currentMovement.y = Mathf.MoveTowards(
                _currentMovement.y,
                worldInput.y,
                _acceleration * Time.deltaTime
            ); 
        }

        // Apply the movement to the CharacterController
        _characterController.Move(_currentMovement * Time.deltaTime);
    }
    #endregion

}