using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public static UnityAction OnBrickDestroyed;
    public static UnityAction GameStart;
    public static UnityAction<int> GameOver;
    public static UnityAction GameReset;
    public static UnityAction LifeEnd;
}
