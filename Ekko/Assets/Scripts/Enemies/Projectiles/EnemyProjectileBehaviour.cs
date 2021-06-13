using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 direction;
    public Vector2 initialPosition;
    public float speed = 20f;
    public int damage = 0;
    public bool speedDebuff = false;
    public float amountSpdDebuff, amountJumpDebuff, amountTime;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private float t;
    void Update()
    {
        t += Time.deltaTime;
        if(t < 0.4f)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            direction = new Vector2(direction.x, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
