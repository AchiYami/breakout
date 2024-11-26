using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Brick> bricks;
    [SerializeField] public AudioClip levelMusic;
    [SerializeField] public int BricksRemaining;
    public bool LevelClear => bricks.Count(x => x.gameObject.activeSelf) <= 0;

    private void Start()
    {
        EventController.OnBrickDestroyed += UpdateTick;
        BricksRemaining = bricks.Count(x => x.gameObject.activeSelf);
    }

    private void OnDisable()
    {
        EventController.OnBrickDestroyed -= UpdateTick;
    }

    public void UpdateTick()
    {
        BricksRemaining = bricks.Count(x => x.gameObject.activeSelf);
        if (LevelClear)
        {
            //Level Complete Trigger
            EventController.NextLevel?.Invoke();
        }
    }

    public void Reset()
    {
        foreach (var brick in bricks)
        {
            brick.gameObject.SetActive(true);
        }
    }
}