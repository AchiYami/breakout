using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class StartPrompt : MonoBehaviour
{
    private Keyboard _keyboard;

    void Start()
    {
        _keyboard = Keyboard.current;
    }

    private void Update()
    {
        if (_keyboard.spaceKey.wasPressedThisFrame)
        {
            OnGameStart();
        }
    }

    private void OnGameStart()
    {
        gameObject.SetActive(false);
        EventController.GameStart?.Invoke();
    }

    public void GameStart()
    {
        gameObject.SetActive(true);
    }

    public void OnLifeEnd()
    {
        gameObject.SetActive(true);
    }
}