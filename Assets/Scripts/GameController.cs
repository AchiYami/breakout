using System;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Score;
    public TMP_Text scoreText;

    private void Start()
    {
        EventController.OnBrickDestroyed += IncreaseScore;
    }

    private void OnDestroy()
    {
        EventController.OnBrickDestroyed -= IncreaseScore;
    }

    private void IncreaseScore()
    {
        Score += 100;
        scoreText.SetText(Score.ToString());
    }
}