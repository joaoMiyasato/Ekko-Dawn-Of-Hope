using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumperBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float radius = 0.3f, patrolSpeed = 2f, chaseSpeed;
    private Transform player;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool groundedR, groundedL, onGround;
    [SerializeField]
    private Transform checkR,checkL;

    private Vector2 jump;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        chaseSpeed = patrolSpeed + 1f;
    }

    void Update()
    {
        if(!GetComponentInChildren<EnemyVision>().getIsHidding())
        {
            if(!GetComponentInChildren<EnemyActionArea>().getTakeAction() && GetComponentInChildren<EnemyAttentionArea>().getGotAttention() && !isJumping)
            {
                inChase = true;
            }
            else if(GetComponentInChildren<EnemyActionArea>().getTakeAction() || isJumping)
            {
                jumpAttack();
            }
        }
        if(!GetComponentInChildren<EnemyActionArea>().getTakeAction() && !GetComponentInChildren<EnemyAttentionArea>().getGotAttention()
        || GetComponentInChildren<EnemyVision>().getIsHidding())
        {
            patrol();
        }
        if(inChase)
        {
            chasing();
        }
    }
    
    private void FixedUpdate()
    {
        groundedR = Physics2D.OverlapCircle(checkR.position, radius, whatIsGround);
        groundedL = Physics2D.OverlapCircle(checkL.position, radius, whatIsGround);
    }


    
    private float curCanTurn = 1f, canTurn = 1f, turn = 1f, curTurn = 1f;
    private void patrol()
    {
        curCanTurn -= Time.deltaTime;
        if(groundedR && groundedL && curCanTurn > 0)
        {
            curTurn = turn;
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
        }
        else if(groundedR && !groundedL 
            || groundedR && !groundedL 
            || curCanTurn < 0)
        {
            if(curTurn > 0)
            {
                canTurn = Random.Range(5f,10f);
                curTurn -= Time.deltaTime;
                change = true;
            }
            else
            {
                curCanTurn = canTurn;
                turn = Random.Range(2f, 4f);
                flip();
                rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
            }
        }
    }

    private bool inChase;
    private void chasing()
    {
        if(transform.position.x <= player.position.x)
        {
            rb.velocity = new Vector2(chaseSpeed, rb.velocity.y);
        }
        else if(transform.position.x > player.position.x)
        {
            rb.velocity = new Vector2(-chaseSpeed, rb.velocity.y);
        }
        if(transform.position.x <= player.position.x)
        {
            if(!facingRight)
            {
                change = true;
                flip();
            }
        }
        else if(transform.position.x > player.position.x)
        {
            if(facingRight)
            {
                change = true;
                flip();
            }
        }
    }

    float waitTime = 2.7f;
    bool waiting = true, isJumping;
    private void jumpAttack()
    {
        if(onGround && !waiting)
        {
            if(facingRight)
            {
                jump = new Vector2(14f,33f);
            }
            else if(!facingRight)
            {
                jump = new Vector2(-14f,33f);
            }
            rb.velocity = jump;
            inAction = true;
            waiting = true;
        }
        if(waiting)
        {
            waitTime += Time.deltaTime;
            if(waitTime > 0.8f && waitTime < 2.2f && !isJumping)
            {
                inChase = true;
            }
            else if(waitTime >= 2.2f)
            {
                inChase = false;
                isJumping = true;
            }
            if(waitTime > 3f)
            {
                waiting = false;
                waitTime = 0;
            }
            if(transform.position.x <= player.position.x)
            {
                if(!facingRight)
                {
                    change = true;
                    flip();
                }
            }
            else if(transform.position.x > player.position.x)
            {
                if(facingRight)
                {
                    change = true;
                    flip();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Ground")
        {
            isJumping = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.layer == 8)
        {
            onGround = true;
            inAction = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.layer == 8)
        {
            onGround = false;
            inAction = true;
        }
    }
    bool change, inAction, facingRight = true;
    private void flip()
    {
        if(change && !inAction)
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
            patrolSpeed *= -1;
            change = false;
        }
    }

    public bool show;
    private void OnDrawGizmosSelected()
    {
        if(show)
        {
            Gizmos.DrawWireSphere(checkR.position, radius);
            Gizmos.DrawWireSphere(checkL.position, radius);
        }
    }
}
