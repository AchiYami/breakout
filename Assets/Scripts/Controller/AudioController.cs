using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    
    [SerializeField]
    private AudioSource source;

    [SerializeField] private AudioClip clip;
    
    private void Start()
    {
        EventController.GameStart += GameStart;
        EventController.LifeEnd += LifeEnd;
    }

    private void OnDestroy()
    {
        EventController.GameStart -= GameStart;
        EventController.LifeEnd -= LifeEnd;
    }


    private void GameStart()
    {
        source.clip = clip;
        source.Play();
    }

    private void LifeEnd()
    {
        source.Stop();
    }
}
