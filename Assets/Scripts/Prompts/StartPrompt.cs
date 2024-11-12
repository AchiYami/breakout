using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartPrompt : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        OnGameStart();
    }

    private void Start()
    {
      //  EventController.LifeEnd += OnLifeEnd;
    }

    private void OnDestroy()
    { 
       // EventController.LifeEnd -= OnLifeEnd;
    }

    private void OnGameStart()
    {
        gameObject.SetActive(false);
        EventController.GameStart?.Invoke();
    }

    public void OnLifeEnd()
    {
        gameObject.SetActive(true);
    }
}