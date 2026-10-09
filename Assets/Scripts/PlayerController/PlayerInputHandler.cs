using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;

    [SerializeField] private string _actionMapName = "Player";

    // Input action names
    [SerializeField] private string _move = "Move";
    [SerializeField] private string _run = "Run";
    [SerializeField] private string _rise = "Rise";
    [SerializeField] private string _sink = "Sink";
    [SerializeField] private string _look = "Look";
    [SerializeField] private string _interact = "Interact";
    [SerializeField] private string _pause = "Pause";
    [SerializeField] private string _dash = "Dash";

    // Input action references
    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _riseAction;
    private InputAction _sinkAction;
    private InputAction _lookAction;
    private InputAction _interactAction;
    private InputAction _pauseAction;
    private InputAction _dashAction;

    // Public values to be accessed by other scripts
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool RunInput { get; private set; }
    public bool RiseInput { get; private set; }
    public bool SinkInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool PauseInput { get; private set; }
    public bool DashInput { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Register input actions
        _moveAction = _inputActions.FindActionMap(_actionMapName).FindAction(_move);
        _runAction = _inputActions.FindActionMap(_actionMapName).FindAction(_run);
        _riseAction = _inputActions.FindActionMap(_actionMapName).FindAction(_rise);
        _sinkAction = _inputActions.FindActionMap(_actionMapName).FindAction(_sink);
        _lookAction = _inputActions.FindActionMap(_actionMapName).FindAction(_look);
        _interactAction = _inputActions.FindActionMap(_actionMapName).FindAction(_interact);
        _pauseAction = _inputActions.FindActionMap(_actionMapName).FindAction(_pause);
        _dashAction = _inputActions.FindActionMap(_actionMapName).FindAction(_dash);
        RegisterInputAction();


    }

    // Register input action callbacks
    void RegisterInputAction()
    {
        _moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _moveAction.canceled += context => MoveInput = Vector2.zero;

        _lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        _lookAction.canceled += context => LookInput = Vector2.zero;

        _runAction.performed += context => RunInput = context.ReadValue<float>() > 0;
        _runAction.canceled += context => RunInput = false;

        _riseAction.performed += context => RiseInput = context.ReadValue<float>() > 0;
        _riseAction.canceled += context => RiseInput = false;

        _sinkAction.performed += context => SinkInput = context.ReadValue<float>() > 0;
        _sinkAction.canceled += context => SinkInput = false;

        /* Running this code in Update() instead of using the InputAction callbacks because the InputAction callbacks can sometimes be unreliable and not always fire when expected. By checking the input state in Update(), we ensure that we always have the most up-to-date input state.
         * 
         * IDK if all that's true it autocorrected it but I think it makes sense. If you want to use the InputAction callbacks instead, you can uncomment the code below and remove / coment the code in Update().
        
        _interactAction.performed += context => InteractInput = true;
        _interactAction.canceled += context => InteractInput = false;

        _pauseAction.performed += context => PauseInput = true;
        _pauseAction.canceled += context => PauseInput = false;
        */
    }

    void Update()
    {
        InteractInput = _interactAction?.IsPressed() ?? false;
        PauseInput = _pauseAction?.IsPressed() ?? false;
        DashInput = _dashAction?.IsPressed() ?? false;
    }

    private void OnEnable()
    {
        _moveAction?.Enable();
        _runAction?.Enable();
        _riseAction?.Enable();
        _sinkAction?.Enable();
        _lookAction?.Enable();
        _interactAction?.Enable();
        _pauseAction?.Enable();
        _dashAction?.Enable();
    }

    private void OnDisable()
    {
        if (_moveAction == null) return;

        _moveAction?.Disable();
        _runAction?.Disable();
        _riseAction?.Disable();
        _sinkAction?.Disable();
        _lookAction?.Disable();
        _interactAction?.Disable();
        _pauseAction?.Disable();
        _dashAction?.Disable();
    }
}
