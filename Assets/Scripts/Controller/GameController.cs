using System.Collections.Generic;
using Controller;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [FoldoutGroup("UI")] [SerializeField] private Transform lifeUpPosition;
    [FoldoutGroup("UI")] [SerializeField] private GameObject lifeUpAlert;
    [FoldoutGroup("Score")] public int score;
    [FoldoutGroup("Score")] public TMP_Text scoreText;

    [FoldoutGroup("Score")] [SerializeField]
    private Leaderboard leaderboard;

    [FoldoutGroup("Prompts")] public StartPrompt startPrompt;
    [FoldoutGroup("Prompts")] public GameOverPrompt endPrompt;

    [FoldoutGroup("Lives")] public int lifeCount = 2;

    [FoldoutGroup("Lives")] [SerializeField]
    private List<GameObject> lifeCounters;

    [FoldoutGroup("levels")] [SerializeField]
    private int currentLevel;

    [FoldoutGroup(("levels"))] [SerializeField]
    private List<Level> levels;

    [FoldoutGroup("Entities")] [SerializeField]
    private PlayerPaddle player;

    [FoldoutGroup("Entities")] [SerializeField]
    private Ball ball;

    [FoldoutGroup("Options")] [SerializeField]
    private bool gainLifeViaScore;

    [FoldoutGroup("Options")] [SerializeField] [ShowIf(@"gainLifeViaScore")]
    private int gainLifeScoreThreshold;

    [FoldoutGroup("Options")] [SerializeField]
    private bool gainLifeViaLevelComplete;

    [FoldoutGroup("MaxLevels")] [SerializeField]
    private int maxLives;

    [FoldoutGroup("Audio")] [SerializeField]
    private AudioController audioController;


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

    /// <summary>
    /// Increases the score by a set amount and deals with life gain through score thresholds.
    /// </summary>
    private void IncreaseScore()
    {
        //Update score
        score += 100;
        scoreText.SetText(score.ToString());

        //If a life is to be gained through a score threshold, check for it.
        if (gainLifeViaScore && (score % gainLifeScoreThreshold == 0))
        {
            GainLife();
        }
    }

    /// <summary>
    /// Game Start Logic
    /// </summary>
    private void GameStart()
    {
        startPrompt.gameObject.SetActive(false);
    }

    /// <summary>
    /// Game Reset Logic
    /// </summary>
    public void GameReset()
    {
        EventController.LifeEnd?.Invoke();
        score = 0;
        scoreText.SetText(score.ToString());
        lifeCount = 2;
    }


    /// <summary>
    /// Game Over Logic
    /// </summary>
    private void GameOver()
    {
        currentLevel = -1;
        ActivateCurrentLevel();
        ball.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        endPrompt.GameOver(score);

        leaderboard.Display();
    }


    /// <summary>
    /// Loads the Current Level
    /// Resets bricks & Plays Music
    /// </summary>
    private void ActivateCurrentLevel()
    {
        for (var i = 0; i < levels.Count; i++)
        {
            levels[i].gameObject.SetActive(i == currentLevel);
        }

        levels[currentLevel].Reset();
        audioController.PlayMusic(levels[currentLevel].levelMusic);
    }

    /// <summary>
    /// Gains a life and updates the visuals
    /// </summary>
    private void GainLife()
    {
        if (lifeCount < maxLives)
        {
            audioController.PlaySoundEffect(BreakoutEnums.SoundEffect.LifeGained);
            Instantiate(lifeUpAlert, lifeUpPosition.position, Quaternion.identity, lifeUpPosition);
            lifeCount++;
            RefreshLifeCounterVisuals();
        }
    }

    /// <summary>
    /// Progress to the next level
    /// </summary>
    private void NextLevel()
    {
        //If life is gained on level completion, do it.
        if (gainLifeViaLevelComplete)
        {
            GainLife();
        }

        //Advance to the next level and reset entities.
        currentLevel++;
        audioController.PlaySoundEffect(BreakoutEnums.SoundEffect.NextLevel);
        ball.ResetBall();
        player.ResetPaddle();

        //If we've reached the end of the game, show game over.
        if (currentLevel >= levels.Count)
        {
            //Game Complete
            GameOver();
        }
        else //Otherwise set the game to 'almost start'
        {
            startPrompt.GameStart();
            ActivateCurrentLevel();
        }
    }

    /// <summary>
    /// Life Loss Logic
    /// </summary>
    private void OnLifeEnd()
    {
        //Decrease life count and update visuals
        audioController.PlaySoundEffect(BreakoutEnums.SoundEffect.LifeLost);
        lifeCount--;
        RefreshLifeCounterVisuals();

        //Determine if game over, or just life loss.
        if (lifeCount <= 0)
        {
            endPrompt.GameOver(score);
        }
        else
        {
            startPrompt.OnLifeEnd();
        }
    }

    /// <summary>
    /// Reset entire game logic
    /// </summary>
    public void GameRestart()
    {
        lifeCount = 3;
        RefreshLifeCounterVisuals();
        score = 0;
        scoreText.SetText("0");

        player.ResetPaddle();
        ball.ResetBall();

        leaderboard.gameObject.SetActive(false);
        startPrompt.gameObject.SetActive(true);
        endPrompt.gameObject.SetActive(false);

        currentLevel = 0;
        ActivateCurrentLevel();
    }

    /// <summary>
    /// Updates the visuals of the life counter
    /// </summary>
    private void RefreshLifeCounterVisuals()
    {
        for (var i = 0; i < lifeCounters.Count; i++)
        {
            lifeCounters[i]?.SetActive(i < lifeCount);
        }
    }
}