using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPaddle : MonoBehaviour
{
    [SerializeField] private float speed;

    private InputSystem_Actions _playerActions;
    private InputSystem_Actions.PlayerActions _playerInputMap;

    private float moveDirection = 0;

    private void Start()
    {
        InitializeControls();
    }


    private void OnDestroy()
    {
        ReleaseControls();
    }


    private void InitializeControls()
    {
        _playerActions = new InputSystem_Actions();

        _playerInputMap = _playerActions.Player;
        _playerInputMap.Enable();

        _playerInputMap.Movement.performed += MoveLeft;
        _playerInputMap.Movement.canceled += MoveLeft;

    }

    private void ReleaseControls()
    {
        _playerInputMap.Movement.performed -= MoveLeft;
        _playerInputMap.Movement.canceled -= MoveLeft;
    }

    private void LateUpdate()
    {
        if (moveDirection != 0)
        {
            transform.Translate(Vector2.right * (moveDirection * speed * Time.deltaTime));
        }
    }


    private void MoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>().x;
        }

        if (context.canceled)
        {
            moveDirection = 0;
        }
    }

 
}