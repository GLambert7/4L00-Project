using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

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
    [SerializeField] private GameObject _playerModel; // Reference to the player model for rotation purposes

    [Header("Camera Settings")]
    [SerializeField] private float _mouseSensitivityHorizontal = 10f;
    [SerializeField] private float _mouseSensitivityVertical = 10f;
    [SerializeField] private float _controllerSensitivityHorizontal = 100f;
    [SerializeField] private float _controllerSensitivityVertical = 100f;

    [SerializeField] private float _upDownRange = 80.0f;
    [SerializeField] private Transform _camPivot;

    [Header("Dash Settings")]
    [SerializeField] private float _dashMult = 3f;
    [SerializeField] private float _dashDuration = 1f;
    [SerializeField] private float _dashCooldown = 3f;
    private float _timeSinceLastDash = 99f;


    [Header("Player Input Handler")]
    [SerializeField] private PlayerInputHandler _inputHandler;

    /*TO ADD: 
     * Stamina?
     * Convert to State Machine
     * Smoother movement (decelleration and acceleration between states)
     * 
     * If there's anything else you want me to add, please let me know and I will do my best to implement it.
    */
    private CharacterController _characterController;
    private Camera _mainCam;
    private float _manualVerticalMovement = 0f;

    private Vector3 _currentMovement = Vector3.zero;
    private Vector3 _dashInput = Vector3.zero;

    private float _horizontalRotation;
    private float _verticalRotation;

    // Device Type Detection
    public enum DeviceType { Keyboard, Controller}
    public DeviceType currentMode { get; private set; }
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
        _timeSinceLastDash += Time.deltaTime;
        HandleControllerType();
        HandleRotation();
        HandleMovement();
    }

    private void HandleControllerType()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        // Only check when an action is performed/triggered
        if (change != InputActionChange.ActionStarted && change != InputActionChange.ActionPerformed)
            return;

        InputAction action = obj as InputAction;
        if (action == null || action.activeControl == null) return;

        // Get the device associated with the active control
        InputDevice device = action.activeControl.device;

        // Determine the device type based on its class/layout
        if (device is Gamepad)
        {
            SwitchInputMode(DeviceType.Controller);
        }
        else if (device is Keyboard || device is Mouse)
        {
            SwitchInputMode(DeviceType.Keyboard);
        }
    }

    private void SwitchInputMode(DeviceType newMode)
    {
        if (currentMode == newMode) return;

        currentMode = newMode;
        Debug.Log($"Switched Input Mode to: {currentMode}");

        // This can also control the UI changes if we want
    }


    #endregion

    #region Movement Methods
    private void HandleRotation()
    {
        if (currentMode == DeviceType.Controller)
        {
            // Update the horizontal and vertical rotation based on the LookInput from the PlayerInputHandler
            _horizontalRotation += _inputHandler.LookInput.x * _controllerSensitivityHorizontal * Time.deltaTime;
            _verticalRotation -= _inputHandler.LookInput.y * _controllerSensitivityVertical * Time.deltaTime;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -_upDownRange, _upDownRange);

            // Apply the rotation to the player and camera
            transform.rotation = Quaternion.Euler(0, _horizontalRotation, 0);
        }
        else
        {
            // Update the horizontal and vertical rotation based on the LookInput from the PlayerInputHandler
            _horizontalRotation += _inputHandler.LookInput.x * _mouseSensitivityHorizontal * Time.deltaTime;
            _verticalRotation -= _inputHandler.LookInput.y * _mouseSensitivityVertical * Time.deltaTime;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -_upDownRange, _upDownRange);

            // Apply the rotation to the player and camera
            transform.rotation = Quaternion.Euler(0, _horizontalRotation, 0);

        }
        
        // Apply the vertical rotation to the camera pivot or main camera if the pivot is not assigned
        if (_camPivot != null)
        {
            _camPivot.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        }
        else if (_mainCam != null)
        {
            _mainCam.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        }

        // Handle player model rotation: Horizontal if moving, vertical if stationary
        if (_inputHandler.MoveInput.magnitude > 0.1f)
        {
            Vector3 modelRotation = new Vector3(_verticalRotation+75, _horizontalRotation, 0);
            _playerModel.transform.rotation = Quaternion.Euler(modelRotation);
        }
        else
        {
            Vector3 modelRotation = new Vector3(0, _horizontalRotation, 0);
            _playerModel.transform.rotation = Quaternion.Euler(modelRotation);
            
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

        // Check for Dash input, and if it's been long enough since the last dash, or if we are currently in a Dash state. If true, run the dash movement, otherwise run the regular movement code
        if ((_inputHandler.DashInput && _timeSinceLastDash > _dashCooldown) || _timeSinceLastDash < _dashDuration)
        {
            // Reset time since last dash on dash use
            if (_timeSinceLastDash > _dashCooldown) { _timeSinceLastDash = 0f; }

            // If we just started the dash, apply the burst movement
            if (_timeSinceLastDash == 0f)
            {
                _dashInput = ((cameraRight * _inputHandler.MoveInput.x) + (cameraForward * _inputHandler.MoveInput.y)).normalized * _moveSpeed;


                _currentMovement.x = Mathf.MoveTowards(
                    _currentMovement.x * _dashMult,
                    _dashInput.x,
                    _acceleration * Time.deltaTime
                );

                _currentMovement.y = Mathf.MoveTowards(
                    _currentMovement.y * _dashMult,
                    _dashInput.y ,
                    _acceleration * Time.deltaTime
                );

                _currentMovement.z = Mathf.MoveTowards(
                    _currentMovement.z * _dashMult,
                    _dashInput.z,
                    _acceleration * Time.deltaTime
                );
            }
            // If not, just apply normal movement towards the saved dash direction
            else
            {
                _currentMovement.x = Mathf.MoveTowards(
                    _currentMovement.x,
                    _dashInput.x,
                    _acceleration * Time.deltaTime
                );

                _currentMovement.y = Mathf.MoveTowards(
                    _currentMovement.y,
                    _dashInput.y,
                    _acceleration * Time.deltaTime
                );

                _currentMovement.z = Mathf.MoveTowards(
                    _currentMovement.z,
                    _dashInput.z,
                    _acceleration * Time.deltaTime
                );
            }

        }
        else
        {

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

            // Calculate vertical movement based on whether or not the player is holding Rise or Sink, or if we are moving using the camera's look direction

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
        }

        // Apply the movement to the CharacterController
        _characterController.Move(_currentMovement * Time.deltaTime);
    }
    #endregion

}