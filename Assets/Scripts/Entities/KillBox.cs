using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            EventController.LifeEnd?.Invoke();
        }
    }
}