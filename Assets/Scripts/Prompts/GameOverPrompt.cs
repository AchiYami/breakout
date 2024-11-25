using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameOverPrompt : MonoBehaviour
{
    private Keyboard _keyboard;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_InputField nameInputField;

    [SerializeField] private Leaderboard leaderboard;


    private int gameOverScore;
    private string gameOverName;

    public void Start()
    {
        _keyboard = Keyboard.current;
        gameObject.SetActive(false);

        submitButton.onClick.AddListener(() => SubmitScore(gameOverName, gameOverScore));
        nameInputField.onEndEdit.AddListener(UpdateName);
    }

    private void UpdateName(string name)
    {
        gameOverName = name;
    }

    public void GameOver(int score)
    {
        gameObject.SetActive(true);
        scoreText.SetText(score.ToString());
        gameOverScore = score;
        submitButton.gameObject.SetActive(true);
        nameInputField.text = "";
        nameInputField.gameObject.SetActive(true);
    }

    private void SubmitScore(string name, int score)
    {
        leaderboard.SubmitScore(name, score);

        submitButton.gameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);
    }
}