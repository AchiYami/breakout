using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverPrompt : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    private TMP_Text scoreText;
    
    public void Start()
    {
        EventController.GameOver += GameOver;
        gameObject.SetActive(false);
    }

    public void GameOver(int score)
    {
        gameObject.SetActive(true);
        scoreText.SetText(score.ToString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventController.GameReset?.Invoke();
        gameObject.SetActive(false);
    }
}
