using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPrompt : MonoBehaviour
{

    //Text to display the final score
    [SerializeField] private TMP_Text scoreText;
    
    //Button to allow for score submission
    [SerializeField] private Button submitButton;
    
    //Input field for Player name
    [SerializeField] private TMP_InputField nameInputField;

    //Leaderboard - displays all scores
    [SerializeField] private Leaderboard leaderboard;

    //Internal tracking of Score & Name
    private int _gameOverScore;
    private string _gameOverName;

    public void Start()
    {
        //Hide the Game Over Prompt immediate
        gameObject.SetActive(false);
        
        //Create input listeners
        submitButton.onClick.AddListener(() => SubmitScore(_gameOverName, _gameOverScore));
        nameInputField.onEndEdit.AddListener(UpdateName);
    }

    /// <summary>
    /// Updates the Name of the Player
    /// </summary>
    /// <param name="playerName">The Name of the Player</param>
    private void UpdateName(string playerName)
    {
        _gameOverName = playerName;
    }

    /// <summary>
    /// Game Over Logic
    /// </summary>
    /// <param name="score">The final score of the player</param>
    public void GameOver(int score)
    {
        //Show Game Over Prompt
        gameObject.SetActive(true);
        
        //Set the Score 
        scoreText.SetText(score.ToString());
        _gameOverScore = score;
        
        //Show the Input Fields
        submitButton.gameObject.SetActive(true);
        nameInputField.text = "";
        nameInputField.gameObject.SetActive(true);
    }

    /// <summary>
    /// Submits the Score, and shows the Leaderboard.
    /// </summary>
    /// <param name="playerName">The Player's name.</param>
    /// <param name="score">The Player's final score.</param>
    private void SubmitScore(string playerName, int score)
    {
        leaderboard.SubmitScore(playerName, score);
        submitButton.gameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);
    }
}