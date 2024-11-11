using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private float initialForce;

    [SerializeField] private float speed = 10f;
    
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(Vector2.up * initialForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        rb2d.linearVelocity = rb2d.linearVelocity.normalized * speed;
    }
}
