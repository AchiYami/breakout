using System.Collections.Generic;
using Controller;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    //The position to spawn the Life Up Alert
    [FoldoutGroup("UI")] [FoldoutGroup("UI/Alerts")] [SerializeField]
    private Transform lifeUpPosition;

    //The Life Up Alert Prefab
    [FoldoutGroup("UI")] [FoldoutGroup("UI/Alerts")] [SerializeField]
    private GameObject lifeUpAlert;

    //UI that Displays the score
    [FoldoutGroup("UI")] [FoldoutGroup("UI/Score")]
    public TMP_Text scoreText;

    //Leaderboard showing all scores
    [FoldoutGroup("UI")] [FoldoutGroup("UI/Score")] [SerializeField]
    private Leaderboard leaderboard;

    //The "Press to Start" Prompt
    [FoldoutGroup("UI")] [FoldoutGroup("UI/Prompts")]
    public StartPrompt startPrompt;

    //The Gme Over Prompt
    [FoldoutGroup("UI")] [FoldoutGroup("UI/Prompts")]
    public GameOverPrompt endPrompt;

    //How many lives the player currently has
    [FoldoutGroup("Data")] [FoldoutGroup("Data/Lives")]
    public int lifeCount = 2;

    //Internal tracking of Life Indicator visuals
    [FoldoutGroup("Data")] [FoldoutGroup("Data/Lives")] [SerializeField]
    private List<GameObject> lifeCounters;

    //Which level are we curently on?
    [FoldoutGroup("Data")] [FoldoutGroup("Data/Levels")] [SerializeField]
    private int currentLevel;

    //The list of all levels
    [FoldoutGroup("Data")] [FoldoutGroup("Data/Levels")] [SerializeField]
    private List<Level> levels;

    //Reference to the player paddle
    [FoldoutGroup("Data")] [FoldoutGroup("Data/Player")] [SerializeField]
    private PlayerPaddle player;

    //Reference to the player ball
    [FoldoutGroup("Data")] [FoldoutGroup("Data/Player")] [SerializeField]
    private Ball ball;

    //Internal Tracking of Player's Score
    [FoldoutGroup("Data")] [FoldoutGroup("Data/Player")]
    public int score;

    //Do we gain life via a score threshold?
    [FoldoutGroup("Configuration")] [SerializeField]
    private bool gainLifeViaScore;

    //The threshold in which we gain a life.
    [FoldoutGroup("Configuration")] [SerializeField] [ShowIf(@"gainLifeViaScore")]
    private int gainLifeScoreThreshold;

    //Do we gain a life by completing a level?
    [FoldoutGroup("Configuration")] [SerializeField]
    private bool gainLifeViaLevelComplete;

    //The maximum amount of lives a player can hold
    [FoldoutGroup("Configuration")] [SerializeField]
    private int maxLives;

    //The audio controller for music and sound effects.
    [FoldoutGroup("Audio")] [SerializeField]
    private AudioController audioController;

    //Reference to Keyboard
    private Keyboard _keyboard;

    private void Start()
    {
        //Subscribe to Events
        EventController.OnBrickDestroyed += IncreaseScore;
        EventController.GameStart += GameStart;
        EventController.LifeEnd += OnLifeEnd;
        EventController.GameReset += GameReset;
        EventController.GameOver += GameOver;
        EventController.NextLevel += NextLevel;

        //Grab the current Keyboard
        _keyboard = Keyboard.current;

        //Start at the first level
        currentLevel = 0;
        ActivateCurrentLevel();
    }

    private void OnDestroy()
    {
        //Unsubscribe from events
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
        //Reset Lives
        lifeCount = 3;
        RefreshLifeCounterVisuals();

        //Reset Score
        score = 0;
        scoreText.SetText("0");

        //Reset Player
        player.ResetPaddle();
        ball.ResetBall();

        //Reset UI
        leaderboard.gameObject.SetActive(false);
        startPrompt.gameObject.SetActive(true);
        endPrompt.gameObject.SetActive(false);

        //Reset Level
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

    //Debug Update to allow for on demand level switching
    private void Update()
    {
        if (_keyboard.digit1Key.wasPressedThisFrame)
        {
            GameRestart();
            currentLevel = 0;
            ActivateCurrentLevel();
        }

        if (_keyboard.digit2Key.wasPressedThisFrame)
        {
            GameRestart();
            currentLevel = 1;
            ActivateCurrentLevel();
        }

        if (_keyboard.digit3Key.wasPressedThisFrame)
        {
            GameRestart();
            currentLevel = 2;
            ActivateCurrentLevel();
        }

        if (_keyboard.digit4Key.wasPressedThisFrame)
        {
            GameRestart();
            currentLevel = 3;
            ActivateCurrentLevel();
        }
    }
}