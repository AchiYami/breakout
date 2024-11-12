using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [FoldoutGroup("Score")] public int Score;
    [FoldoutGroup("Score")] public TMP_Text scoreText;

    [FoldoutGroup("Prompts")] public StartPrompt startPrompt;
    [FoldoutGroup("Prompts")] public GameOverPrompt endPrompt;

    [FoldoutGroup("Lives")] public int LifeCount = 2;

    [FoldoutGroup("Lives")] [SerializeField]
    private List<GameObject> lifeCounters;

    private void Start()
    {
        EventController.OnBrickDestroyed += IncreaseScore;
        EventController.GameStart += GameStart;
        EventController.LifeEnd += OnLifeEnd;
        EventController.GameReset += GameReset;
    }

    private void OnDestroy()
    {
        EventController.OnBrickDestroyed -= IncreaseScore;
        EventController.GameStart -= GameStart;
        EventController.LifeEnd -= OnLifeEnd;
        EventController.GameReset -= GameReset;
    }

    private void IncreaseScore()
    {
        Score += 100;
        scoreText.SetText(Score.ToString());
    }

    private void GameStart()
    {
        startPrompt.gameObject.SetActive(false);
    }

    private void GameReset()
    {
        EventController.LifeEnd?.Invoke();
        Score = 0;
        scoreText.SetText(Score.ToString());
        LifeCount = 2;
        
    }
    
    private void OnLifeEnd()
    {
        if (LifeCount <= 0)
        {
            endPrompt.GameOver(Score);
        }
        else
        {
            startPrompt.OnLifeEnd();

            LifeCount--;
            for (var i = 0; i < lifeCounters.Count; i++)
            {
                print($"Life Counter is now {LifeCount}, Setting {i} to {i <= LifeCount}");
                lifeCounters[i]?.SetActive(i <= LifeCount);
            }
        }
    }
}