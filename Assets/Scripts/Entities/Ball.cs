using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;

    [SerializeField] private float initialForce;

    [SerializeField] private float speed = 10f;

    private Vector2 initialPosition;

    [SerializeField] [FoldoutGroup("Audio")]
    private AudioClip onHitBrickAudio;

    [SerializeField] [FoldoutGroup("Audio")]
    private AudioClip onHitKillAudio;

    [SerializeField] [FoldoutGroup("Audio")]
    private AudioClip onHitWallAudio;

    [SerializeField] [FoldoutGroup("Audio")]
    private AudioClip onHitPlayerAudio;

    [SerializeField] [FoldoutGroup("Audio")]
    private AudioSource audioSource;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        EventController.GameStart += GameStart;
        EventController.LifeEnd += ResetBall;
        initialPosition = transform.position;
    }

    private void OnDestroy()
    {
        EventController.GameStart -= GameStart;
        EventController.LifeEnd -= ResetBall;
    }

    private void FixedUpdate()
    {
        rb2d.linearVelocity = rb2d.linearVelocity.normalized * speed;
    }

    private void GameStart()
    {
        float angle = Random.Range(-10, 10);
        var randomUpwardsVelocty = new Vector2(angle, 1);
        rb2d.AddForce(randomUpwardsVelocty * initialForce, ForceMode2D.Impulse);
    }

    public void ResetBall()
    {
        rb2d.linearVelocity = Vector2.zero;
        transform.position = initialPosition;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "Wall":
                audioSource.PlayOneShot(onHitWallAudio);
                break;
            case "Player":
                audioSource.PlayOneShot(onHitPlayerAudio);
                break;
            case "KillBox":
                audioSource.PlayOneShot(onHitKillAudio);
                break;
            case "Brick":
                audioSource.PlayOneShot(onHitBrickAudio);
                break;
        }
    }
}