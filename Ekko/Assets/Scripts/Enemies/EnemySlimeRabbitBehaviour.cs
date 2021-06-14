using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlimeRabbitBehaviour : MonoBehaviour
{
    private bool stop;

    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField]
    private float runAwaySpeed = 18f, radius = 0.3f, patrolSpeed = 4f;
    private Transform player;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool groundedR, justGo;
    [SerializeField]
    private Transform checkR;
    private Vector2 go;

    private bool change, facingRight = true;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    float tt;
    void Update()
    {
        if(!GetComponentInChildren<EnemyActionArea>().getTakeAction() && !GetComponentInChildren<EnemyAttentionArea>().getGotAttention() && !running
        || GetComponentInChildren<EnemyVision>().getIsHidding() && !running)
        {
            patrol();
        }
        
        if(running)
        {
            tt += Time.deltaTime;
            GetComponentInChildren<EnemyActionArea>().setTakeAction(true);
            if(tt > 3.5f)
            {
                GetComponentInChildren<EnemyActionArea>().setTakeAction(false);
                running = false;
            }
        }
        else{tt = 0;}

        if(!GetComponentInChildren<EnemyVision>().getIsHidding() || running)
        {
            if(!GetComponentInChildren<EnemyActionArea>().getTakeAction() && GetComponentInChildren<EnemyAttentionArea>().getGotAttention() && !running)
            {
                if(transform.position.x > player.position.x)
                {
                    if(facingRight)
                    {
                        change = true;
                        flip();
                    }
                }
                else if(transform.position.x <= player.position.x)
                {
                    if(!facingRight)
                    {
                        change = true;
                        flip();
                    }
                }
                //Animação do bicho com medo
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if(GetComponentInChildren<EnemyActionArea>().getTakeAction())
            {
                if(groundedR && transform.position.x >= player.position.x
                || groundedR && transform.position.x <= player.position.x)
                {
                    run();
                }
                else if(!groundedR && transform.position.x >= player.position.x
                    || !groundedR && transform.position.x <= player.position.x)
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
        
        dieFromFalling();
    }
    private void FixedUpdate()
    {
        groundedR = Physics2D.OverlapCircle(checkR.position, radius, whatIsGround);
    }

    private float curCanTurn = 1f, canTurn = 1f, turn = 1f, curTurn = 1f, curSpd;
    private float waitToJump = 0;
    private void patrol()
    {
        curCanTurn -= Time.deltaTime;
        if(groundedR && curCanTurn > 0)
        {
            curTurn = turn;
            curSpd = patrolSpeed;
            if(!onGround)
            {
                rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
                waitToJump = 0;
            }
            else
            {
                anim.SetTrigger("jumping");
                anim.SetBool("onGround", false);
                waitToJump += Time.deltaTime;
                if(waitToJump > 0.24f)
                    rb.velocity = new Vector2(patrolSpeed, 30f);
            }
        }
        else if(!groundedR
            || curCanTurn < 0)
        {
            if(curTurn > 0)
            {
                if(facingRight)
                {
                    curSpd -= 0.25f;
                    if(curSpd < 0) curSpd = 0;
                }
                else
                {
                    curSpd += 0.25f;
                    if(curSpd > 0) curSpd = 0;
                }
                rb.velocity = new Vector2(curSpd, rb.velocity.y);
                canTurn = Random.Range(2f,5f);
                curTurn -= Time.deltaTime;
                change = true;
            }
            else
            {
                curCanTurn = canTurn;
                turn = Random.Range(5f, 10f);
                flip();
                anim.SetTrigger("jumping");
                anim.SetBool("onGround", false);
                if(waitToJump > 0.12f)
                    rb.velocity = new Vector2(patrolSpeed, 30f);
            }
        }
    }

    private float runWaitToJump;
    private void run()
    {
        stop = true;
        if(transform.position.x >= player.position.x)
        {
            if(!facingRight)
            {
                change = true;
                flip();
            }
        }
        else if(transform.position.x < player.position.x)
        {
            if(facingRight)
            {
                change = true;
                flip();
            }
        }
        if(player.position.x <= this.transform.position.x)
        {
            runAwaySpeed = Mathf.Abs(runAwaySpeed);
        }
        else
        {
            runAwaySpeed = -Mathf.Abs(runAwaySpeed);
        }
        if(onGround)
        {
            runWaitToJump += Time.deltaTime;
            anim.SetTrigger("jumping");
            anim.SetBool("onGround", false);
            if(runWaitToJump > 0.24f)
            {
                rb.velocity = new Vector2(runAwaySpeed, 30f);
            }
        }
        else
        {
            runWaitToJump = 0;
            rb.velocity = new Vector2(runAwaySpeed, rb.velocity.y);
        }
    }

    float t;
    bool onGround, running;
    private void suicideJump()
    {
        t += Time.deltaTime;
        if(t > 0.6f) justGo = true;
        if(justGo && t < 0.8f && t > 0.6f && onGround)
        {
            if((this.transform.position.x - player.position.x) < 0)
            {
                go = new Vector2(-10f, 25f);
            }
            else if((this.transform.position.x - player.position.x) >= 0)
            {
                go = new Vector2(10f, 25f);
            }
            if(GetComponentInChildren<EnemyActionArea>().getTakeAction())
            {
                rb.velocity = go;
                running = true;
            }
        }
    }

    private float dyingHeight = 0.5f, curAirTime;
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
            anim.SetTrigger("landing");
            anim.SetBool("onGround", true);
            if(dieOnCollision)
            {
                Destroy(gameObject);
            }
            curAirTime = 0;
            t = 0;
            justGo = false;
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 13)
        {
            onGround = true;
            anim.SetBool("onGround", true);
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
    
    private void flip()
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
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public bool show;
    private void OnDrawGizmos()
    {
        if(show)
        {
            Gizmos.DrawWireSphere(checkR.position, radius);
        }
    }
}
