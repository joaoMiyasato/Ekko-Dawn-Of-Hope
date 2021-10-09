using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 direction;
    public Vector2 initialPosition;
    public int arrowDamage;
    public float arrowSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private float t;
    private void Update()
    {
        t += Time.deltaTime;
        if(t < 0.4f)
        {
            rb.velocity = direction * arrowSpeed;
        }
        else
        {
            direction = new Vector2(direction.x, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<EnemyBase>().takeDamage(arrowDamage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
