using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public static UnityAction OnBrickDestroyed;
    public static UnityAction GameStart;
    public static UnityAction GameOver;
    public static UnityAction GameReset;
    public static UnityAction LifeEnd;
    public static UnityAction NextLevel;
}