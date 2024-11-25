using TMPro;
using UnityEngine;

public class ScoreEntry : MonoBehaviour
{
    public void SetData(int newOrder, string newName, int newScore)
    {
        order.SetText(newOrder.ToString());
        name.SetText(newName);
        score.SetText(newScore.ToString());
    }

    [SerializeField] private TMP_Text order;
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text score;
}