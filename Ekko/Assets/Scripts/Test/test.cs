using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Rigidbody2D rb;
    private float MoveInput, curSpeed, SpeedFix = 18f;
    private bool xtraspeed, moving;
    
    public float jumpForce = 13f;
    public float jumpTime = 0.25f;
    public float jumpTimeCounter;
    private float jumpCount;
    private float doubleJump = 2;
    public bool isJumping;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curSpeed = SpeedFix;
    }
    void Update()
    {
        jump();
    }
    private void FixedUpdate()
    {
        move();
    }
    
    private void move()
    {
        MoveInput = Input.GetAxisRaw("Horizontal");

        if(xtraspeed)
        {
            curSpeed -= 0.3f;
            if(curSpeed <= SpeedFix)
            {
                curSpeed = SpeedFix;
                xtraspeed = false;
            }
        }

        rb.velocity = new Vector2(MoveInput * curSpeed, rb.velocity.y);
    }
    
    public Transform lugar;
    public GameObject henge;
    private void jump()
    {
        if(jumpCount > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            jumpCount--;
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up ;
            henge.transform.position = lugar.position;
        }

        if(isJumping && Input.GetKey(KeyCode.Space))
        {
            if(jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce*3;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if(isJumping)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                rb.velocity = rb.velocity / 2;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }
    private IEnumerator jumpLess()
    {
        yield return new WaitForSeconds(0.075f);
        jumpCount--;
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.collider.gameObject.tag == "Ground" || other.collider.gameObject.tag == "Chest" || other.collider.gameObject.tag == "GroundDestructable")
        {  
            jumpCount = doubleJump;
        }
    }
}
