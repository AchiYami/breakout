using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using File = System.IO.File;


public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject scoreboard;

    [SerializeField] private GameObject scoreEntryPrefab;
    [SerializeField] private List<ScoreEntry> scoreEntries;

    [SerializeField] private Transform content;

    public void Display()
    {
        var scoreData = LoadLeaderboardData();

        ClearLeaderboardEntries();

        gameObject.SetActive(true);
        DisplayLeaderboardEntries(scoreData);
    }

    public void SubmitScore(string name, int score)
    {
        print($"Submitting Score! {name} - {score}");

        var scoreData = LoadLeaderboardData();
        scoreData.Add(new ScoreData
        {
            Name = name,
            Score = score
        });
        WriteLeaderboardData(scoreData);
        Display();
    }

    private void ClearLeaderboardEntries()
    {
        for (var i = scoreboard.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(scoreboard.transform.GetChild(i).gameObject);
        }
    }

    private void DisplayLeaderboardEntries(List<ScoreData> scoreData)
    {
        for (var i = 0; i < scoreData.Count; i++)
        {
            var score = scoreData[i];
            var newScore = Instantiate(scoreEntryPrefab, content).GetComponent<ScoreEntry>();
            newScore.SetData(i + 1, score.Name, score.Score);
        }
    }

    private List<ScoreData> LoadLeaderboardData()
    {
        //If the file doesn't exist, return a new leaderboard.
        if (!File.Exists(Application.persistentDataPath + "/Leaderboard.json")) return new List<ScoreData>();

        //Otherwise, get the current leaderboard string.
        var leaderboardStr = File.ReadAllText(Application.persistentDataPath + "/leaderboard.json");

        try
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

    private void WriteLeaderboardData(List<ScoreData> leaderboardData)
    {
        //Get the path
        var fileName = Application.persistentDataPath + "/leaderboard.json";

        //Convert to json string
        var leaderboardString = JsonConvert.SerializeObject(leaderboardData);

        print($"Writing Scores to : {fileName}");

        //Write out
        File.WriteAllText(fileName, leaderboardString);
    }
}