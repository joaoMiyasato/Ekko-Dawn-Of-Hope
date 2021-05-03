using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFugitivoBehaviour : MonoBehaviour
{
    private bool stop;

    private Rigidbody2D rb;
    [SerializeField]
    private float runAwaySpeed = 18f, radius = 0.3f, patrolSpeed = 2f;
    private Transform player;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool groundedR, groundedL, justGo;
    [SerializeField]
    private Transform checkR,checkL;
    private Vector2 go;

    private bool change, facingRight = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    float tt;
    void Update()
    {
        if(running)
        {
            tt += Time.deltaTime;
            if(tt > 1f)
            {
                // if(groundedR && groundedL)
                // {
                    run();
                // }
            }
            else if(tt > 4f)
            {
                running = false;
                tt = 0;
            }
        }
        if(!GetComponentInChildren<EnemyVision>().getIsHidding())
        {
            if(!GetComponentInChildren<EnemyActionArea>().getTakeAction() && GetComponentInChildren<EnemyAttentionArea>().getGotAttention() && !running)
            {       
                //Animação do bicho com medo
            }
            else if(GetComponentInChildren<EnemyActionArea>().getTakeAction())
            {      
                if(groundedR && groundedL)
                {
                    // Flip();
                    run();
                }
                else if(!groundedR || !groundedL)
                {
                    stopMoving();
                    suicideJump();
                    change = true;
                }
            }
            else
            {
                t = 0;
                justGo = false;
            }
        }
        if(!GetComponentInChildren<EnemyActionArea>().getTakeAction() && !GetComponentInChildren<EnemyAttentionArea>().getGotAttention() 
        || GetComponentInChildren<EnemyVision>().getIsHidding())
        {
            patrol();
        }
        dieFromFalling();
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
                Flip();
                rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
            }
        }
    }

    private void run()
    {
        stop = true;
        if(player.position.x <= this.transform.position.x)
        {
            runAwaySpeed = Mathf.Abs(runAwaySpeed);
        }
        else
        {
            runAwaySpeed = -Mathf.Abs(runAwaySpeed);
        }
        rb.velocity = new Vector2(runAwaySpeed, rb.velocity.y);
    }

    float t;
    bool onGround, running;
    private void suicideJump()
    {
        t += Time.deltaTime;
        if(t > 1.5f) justGo = true;
        if(justGo && t < 1.6f && t > 1.5f && onGround)
        {
            if((this.transform.position.x - player.position.x) < 0)
            {
                go = new Vector2(-12f, 25f);
            }
            else if((this.transform.position.x - player.position.x) >= 0)
            {
                go = new Vector2(12f, 25f);
            }
            if(GetComponentInChildren<EnemyActionArea>().getTakeAction() && !GetComponentInChildren<EnemyVision>().getIsHidding())
            {
                rb.AddForce(go, ForceMode2D.Impulse);
                running = true;
            }
        }
    }

    private float dyingHeight = 0.6f, curAirTime;
    private bool dieOnCollision = false;
    private void dieFromFalling()
    {
        if(rb.velocity.y < 0)
        {   
            curAirTime += Time.deltaTime;
            if(curAirTime >= dyingHeight)
            {
                dieOnCollision = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 13)
        {
            if(dieOnCollision)
            {
                Destroy(gameObject);
            }
            curAirTime = 0;
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 13)
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 13)
        {
            onGround = false;
            t = 0;
        }
    }
    
    private void Flip()
    {
        if(change)
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
            patrolSpeed *= -1;
            change = false;
        }
    }
    
    private void stopMoving()
    {
        if(stop)
        {
            stop = false;
            rb.velocity = Vector2.zero;
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
