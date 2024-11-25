using UnityEngine;

public class Brick : MonoBehaviour
{

    private Level _level;

    public void Initialize(Level level)
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.transform.CompareTag("Ball"))
        {
            gameObject.SetActive(false);
            EventController.OnBrickDestroyed?.Invoke();
        }
    }
}