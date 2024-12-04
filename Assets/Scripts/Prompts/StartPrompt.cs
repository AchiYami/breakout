using UnityEngine;
using UnityEngine.InputSystem;


public class StartPrompt : MonoBehaviour
{
    //Internal reference to keyboard and Gamepad
    private Keyboard _keyboard;
    private Gamepad _gamepad;

    private void Start()
    {
        _keyboard = Keyboard.current;
        _gamepad = Gamepad.current;
    }

    private void Update()
    {
        //Checks for Game Start -- This could/should be moved into an InputAction listener.
        if (_keyboard.spaceKey.wasPressedThisFrame)
        {
            OnGameStart();
        }

        //Checks for Gamepad controller start
        if (_gamepad != null && (_gamepad.buttonSouth.wasPressedThisFrame || _gamepad.startButton.wasPressedThisFrame))
        {
            OnGameStart();
        }
    }

    /// <summary>
    /// Game Start Event Logic
    /// </summary>
    private void OnGameStart()
    {
        gameObject.SetActive(false);
        EventController.GameStart?.Invoke();
    }

    /// <summary>
    /// Game Start Logic
    /// </summary>
    public void GameStart()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Life Loss Logic
    /// </summary>
    public void OnLifeEnd()
    {
        gameObject.SetActive(true);
    }
}