using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using File = System.IO.File;


public class Leaderboard : MonoBehaviour
{
    //The UI for the Scoreboard
    [SerializeField] private GameObject scoreboard;

    //A single entry in the score board
    [SerializeField] private GameObject scoreEntryPrefab;
    
    //A list of all entries
    [SerializeField] private List<ScoreEntry> scoreEntries;

    //The panel to spawn entries on to
    [SerializeField] private Transform content;

    /// <summary>
    /// Loads & Displays all scores
    /// </summary>
    public void Display()
    {
        //Load the Scores
        var scoreData = LoadLeaderboardData();

        //Ensure there are no scores from previous viewing
        ClearLeaderboardEntries();

        //Show the Leaderboard
        gameObject.SetActive(true);
        DisplayLeaderboardEntries(scoreData);
    }

    /// <summary>
    /// Submits a Score & Writes to File
    /// </summary>
    /// <param name="name">The Player's Name</param>
    /// <param name="score">The Player's Final Score</param>
    public void SubmitScore(string name, int score)
    {

        //Load the Scores
        var scoreData = LoadLeaderboardData();
        
        //Add the new Score
        scoreData.Add(new ScoreData
        {
            Name = name,
            Score = score
        });
        
        //Write the updated leaderboard to file & display
        WriteLeaderboardData(scoreData);
        Display();
    }

    /// <summary>
    /// Removes all previous leaderboard entries (display only)
    /// </summary>
    private void ClearLeaderboardEntries()
    {
        for (var i = scoreboard.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(scoreboard.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Shows all individual leader board entries
    /// </summary>
    /// <param name="scoreData">The leaderboard scores</param>
    private void DisplayLeaderboardEntries(List<ScoreData> scoreData)
    {
        for (var i = 0; i < scoreData.Count; i++)
        {
            var score = scoreData[i];
            var newScore = Instantiate(scoreEntryPrefab, content).GetComponent<ScoreEntry>();
            newScore.SetData(i + 1, score.Name, score.Score);
        }
    }

    /// <summary>
    /// Loads the leaderboard from a file
    /// </summary>
    /// <returns></returns>
    private static List<ScoreData> LoadLeaderboardData()
    {
        //If the file doesn't exist, return a new leaderboard.
        if (!File.Exists(Application.persistentDataPath + "/Leaderboard.json")) return new List<ScoreData>();

        //Otherwise, get the current leaderboard string.
        var leaderboardStr = File.ReadAllText(Application.persistentDataPath + "/leaderboard.json");

        try//If the file cannot be deserialized, return an empty list.
        {
            //Convert from JSON 
            var leaderBoard = JsonConvert.DeserializeObject<List<ScoreData>>(leaderboardStr);

            //Ensure the dictionary is ordered from highest to lowest.
            return leaderBoard.OrderByDescending(x => x.Score).ToList();
        }
        catch (JsonSerializationException e)
        {
            return new List<ScoreData>();
        }
    }

    /// <summary>
    /// Writes a list of scores to a file
    /// </summary>
    /// <param name="leaderboardData"></param>
    private static void WriteLeaderboardData(List<ScoreData> leaderboardData)
    {
        //Get the path
        var fileName = Application.persistentDataPath + "/leaderboard.json";

        //Convert to json string
        var leaderboardString = JsonConvert.SerializeObject(leaderboardData);
        
        //Write out
        File.WriteAllText(fileName, leaderboardString);
    }
}