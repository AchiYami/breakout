using Controller;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    //Physics - Rigidbody 2D 
    [SerializeField] private Rigidbody2D ballRigidBody;

    //The initial 'kick' provided to the ball on game start
    [SerializeField] private float initialForce;

    //The constant speed of the ball
    [SerializeField] private float speed = 10f;

    //The position of ball of game start
    private Vector2 _initialPosition;

    [SerializeField] private AudioController audioController;

    private void Start()
    {
        //Get Components
        ballRigidBody = GetComponent<Rigidbody2D>();

        //Event Subscription
        EventController.GameStart += GameStart;
        EventController.LifeEnd += ResetBall;

        //Track initial position
        _initialPosition = transform.position;
    }

    private void OnDestroy()
    {
        //Unsubscribe from events
        EventController.GameStart -= GameStart;
        EventController.LifeEnd -= ResetBall;
    }

    private void FixedUpdate()
    {
        //Movement
        ballRigidBody.linearVelocity = ballRigidBody.linearVelocity.normalized * speed;
    }

    /// <summary>
    /// Game Start Logic
    /// </summary>
    private void GameStart()
    {
        //Set a random angle for the ball to move in initially
        float angle = Random.Range(-5, 5);
        //Apply upwards direction
        var upwardsVelocity = new Vector2(angle, 1);
        //Hit the ball
        ballRigidBody.AddForce(upwardsVelocity * initialForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Resets the ball to it's initial state
    /// </summary>
    public void ResetBall()
    {
        //Stop movement
        ballRigidBody.linearVelocity = Vector2.zero;
        //Reset position
        transform.position = _initialPosition;
    }

    /// <summary>
    /// Collision Logic
    /// </summary>
    /// <param name="other">The source of the collision</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Determines which sound to play, if any.
        switch (other.transform.tag)
        {
            case "Wall":
                audioController.PlaySoundEffect(BreakoutEnums.SoundEffect.WallHit);
                break;
            case "Player":
                audioController.PlaySoundEffect(BreakoutEnums.SoundEffect.WallHit);
                break;
            case "KillBox":
                audioController.PlaySoundEffect(BreakoutEnums.SoundEffect.LifeLost);
                break;
            case "Brick":
                audioController.PlaySoundEffect(BreakoutEnums.SoundEffect.BrickHit);
                break;
        }
    }
}