using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [FoldoutGroup("UI")] [SerializeField] private Transform lifeUpPosition;
    [FoldoutGroup("UI")] [SerializeField] private GameObject lifeUpAlert;
    [FoldoutGroup("Score")] public int Score;
    [FoldoutGroup("Score")] public TMP_Text scoreText;

    [FoldoutGroup("Score")] [SerializeField]
    private Leaderboard leaderboard;

    [FoldoutGroup("Prompts")] public StartPrompt startPrompt;
    [FoldoutGroup("Prompts")] public GameOverPrompt endPrompt;

    [FoldoutGroup("Lives")] public int LifeCount = 2;

    [FoldoutGroup("Lives")] [SerializeField]
    private List<GameObject> lifeCounters;

    [FoldoutGroup("levels")] [SerializeField]
    private int currentLevel = 0;

    [FoldoutGroup(("levels"))] [SerializeField]
    private List<Level> levels;

    [FoldoutGroup("Entities")] [SerializeField]
    private PlayerPaddle player;

    [FoldoutGroup("Entities")] [SerializeField]
    private Ball ball;

    [FoldoutGroup("Audio")] [SerializeField]
    private AudioSource audioSource;

    [FoldoutGroup("Audio")] [SerializeField]
    private AudioClip nextLevelClip;

    [FoldoutGroup("Audio")] [SerializeField]
    private AudioClip gainLifeClip;

    [FoldoutGroup("Audio")] [SerializeField]
    private AudioClip loseLifeClip;

    [FoldoutGroup("Audio")] [SerializeField]
    private AudioSource musicSource;

    [FoldoutGroup("Options")] [SerializeField]
    private bool gainLifeViaScore;

    [FoldoutGroup("Options")] [SerializeField] [ShowIf(@"gainLifeViaScore")]
    private int gainLifeScoreThreshold;

    [FoldoutGroup("Options")] [SerializeField]
    private bool gainLifeViaLevelComplete;

    [FoldoutGroup("MaxLevels")] [SerializeField]
    private int maxLives;


    private void Start()
    {
        EventController.OnBrickDestroyed += IncreaseScore;
        EventController.GameStart += GameStart;
        EventController.LifeEnd += OnLifeEnd;
        EventController.GameReset += GameReset;
        EventController.GameOver += GameOver;
        EventController.NextLevel += NextLevel;

        currentLevel = 0;
        ActivateCurrentLevel();
    }

    private void OnDestroy()
    {
        EventController.OnBrickDestroyed -= IncreaseScore;
        EventController.GameStart -= GameStart;
        EventController.LifeEnd -= OnLifeEnd;
        EventController.GameReset -= GameReset;
        EventController.NextLevel -= NextLevel;
        EventController.GameOver -= GameOver;
    }

    private void IncreaseScore()
    {
        Score += 100;
        scoreText.SetText(Score.ToString());


        if (gainLifeViaScore && (Score % gainLifeScoreThreshold == 0))
        {
            GainLife();
        }
    }

    private void GameStart()
    {
        startPrompt.gameObject.SetActive(false);
    }

    public void GameReset()
    {
        EventController.LifeEnd?.Invoke();
        Score = 0;
        scoreText.SetText(Score.ToString());
        LifeCount = 2;
    }

    private void GameOver()
    {
        currentLevel = -1;
        ActivateCurrentLevel();
        ball.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        endPrompt.GameOver(Score);

        leaderboard.Display();
    }


    private void ActivateCurrentLevel()
    {
        for (var i = 0; i < levels.Count; i++)
        {
            levels[i].gameObject.SetActive(i == currentLevel);
        }

        levels[currentLevel].Reset();
        musicSource.clip = levels[currentLevel].levelMusic;
        musicSource.Play();
    }

    private void GainLife()
    {
        if (LifeCount < maxLives)
        {
            audioSource.PlayOneShot(gainLifeClip);
            Instantiate(lifeUpAlert, lifeUpPosition.position, Quaternion.identity, lifeUpPosition);
            LifeCount++;
            RefreshLifeCounterVisuals();
        }
    }

    private void NextLevel()
    {
        if (gainLifeViaLevelComplete)
        {
            GainLife();
        }

        currentLevel++;
        audioSource.PlayOneShot(nextLevelClip);
        ball.ResetBall();
        player.ResetPaddle();

        if (currentLevel >= levels.Count)
        {
            //Game Complete
            GameOver();
        }
        else
        {
            startPrompt.GameStart();
            ActivateCurrentLevel();
        }
    }

    private void OnLifeEnd()
    {
        audioSource.PlayOneShot(loseLifeClip);
        LifeCount--;
        RefreshLifeCounterVisuals();

        if (LifeCount <= 0)
        {
            endPrompt.GameOver(Score);
        }
        else
        {
            startPrompt.OnLifeEnd();
        }
    }

    public void GameRestart()
    {
        LifeCount = 3;
        RefreshLifeCounterVisuals();
        Score = 0;
        scoreText.SetText("0");

        player.ResetPaddle();
        ball.ResetBall();

        leaderboard.gameObject.SetActive(false);
        startPrompt.gameObject.SetActive(true);
        endPrompt.gameObject.SetActive(false);

        currentLevel = 0;
        ActivateCurrentLevel();
    }

    private void RefreshLifeCounterVisuals()
    {
        for (var i = 0; i < lifeCounters.Count; i++)
        {
            lifeCounters[i]?.SetActive(i < LifeCount);
        }
    }
}