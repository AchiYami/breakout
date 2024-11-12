using UnityEngine;

public class Brick : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.transform.CompareTag("Ball"))
        {
            Destroy(gameObject);
            EventController.OnBrickDestroyed?.Invoke();
        }
    }
}