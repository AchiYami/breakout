using UnityEngine;
using UnityEngine.InputSystem;


public class StartPrompt : MonoBehaviour
{
    //Internal reference to keyboard
    private Keyboard _keyboard;

    private void Start()
    {
        _keyboard = Keyboard.current;
    }

    private void Update()
    {
        //Checks for Game Start -- This could/should be moved into an InputAction listener.
        if (_keyboard.spaceKey.wasPressedThisFrame)
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