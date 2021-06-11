using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 direction;
    public Vector2 initialPosition;
    public float speed = 0.1f;
    public float maxDistance = 50f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = direction * speed;


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer != 10)
        {
            Destroy(gameObject);
        }
    }
}
