using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPaddle : MonoBehaviour
{
    //The speed at which the paddle moves
    [SerializeField] private float speed;

    //Unity Input System actions
    private InputSystem_Actions _playerActions;
    private InputSystem_Actions.PlayerActions _playerInputMap;

    //The direction in which the paddle is moving
    private float _moveDirection = 0;

    //Is the paddle active?
    private bool _active;

    //The initial position of the paddle on game start.
    private Vector2 _initialPosition;

    private void Start()
    {
        //Event Subscription
        EventController.GameStart += OnGameStart;
        EventController.LifeEnd += OnLifeEnd;
        EventController.GameReset += ResetPaddle;

        //Set initial position
        _initialPosition = transform.position;

        //Enable controls
        InitializeControls();
    }


    private void OnDestroy()
    {
        //Disable Controls
        ReleaseControls();

        //Unsubscribe from Events
        EventController.GameStart -= OnGameStart;
        EventController.LifeEnd -= OnLifeEnd;
        EventController.GameReset -= ResetPaddle;
    }

    /// <summary>
    /// Creates & Enables Unity Input System Controls for the Paddle
    /// </summary>
    private void InitializeControls()
    {
        _playerActions = new InputSystem_Actions();

        _playerInputMap = _playerActions.Player;
        _playerInputMap.Enable();

        _playerInputMap.Movement.performed += HandleMovement;
        _playerInputMap.Movement.canceled += HandleMovement;
    }

    /// <summary>
    /// Disables  the Unity Input System Controls for the Paddle
    /// </summary>
    private void ReleaseControls()
    {
        _playerInputMap.Movement.performed -= HandleMovement;
        _playerInputMap.Movement.canceled -= HandleMovement;
    }

    private void LateUpdate()
    {
        //Early Escape
        if (!_active) return;

        //Movement
        if (_moveDirection != 0)
        {
            transform.Translate(Vector2.right * (_moveDirection * speed * Time.deltaTime));
        }
    }

    /// <summary>
    /// Movement Listener for the Unity Input System
    /// </summary>
    /// <param name="context">Unity Input System Context</param>
    private void HandleMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _moveDirection = context.ReadValue<Vector2>().x;
        }

        if (context.canceled)
        {
            _moveDirection = 0;
        }
    }

    /// <summary>
    /// Game Start Logic
    /// </summary>
    private void OnGameStart()
    {
        _active = true;
    }

    /// <summary>
    /// Life Loss Logic
    /// </summary>
    private void OnLifeEnd()
    {
        _active = false;
        transform.position = _initialPosition;
    }

    /// <summary>
    /// Resets Paddle to it's initial state
    /// </summary>
    public void ResetPaddle()
    {
        transform.position = _initialPosition;
    }
}