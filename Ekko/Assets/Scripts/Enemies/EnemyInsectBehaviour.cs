using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInsectBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public float gravityIntensity = 80f;
    public bool isOnRightWall, isOnLeftWall, isOnGround, isOnCeiling;
    private Vector2 newGravity;
    
    [SerializeField]
    private float radius = 0.3f, patrolSpeed = 2f, chaseSpeed;
    private Transform player;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool groundedR, groundedL, wallR, wallL;
    [SerializeField]
    private Transform checkR,checkL, checkWallR, checkWallL;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(isOnRightWall)
        {
            transform.rotation = Quaternion.Euler(0,0, 90);
            newGravity = Vector2.right;
        }
        else if(isOnLeftWall)
        {
            transform.rotation = Quaternion.Euler(0,0, -90);
            newGravity = Vector2.left;
        }
        else if(isOnGround)
        {
            newGravity = Vector2.down;
        }
        else if(isOnCeiling)
        {
            transform.rotation = Quaternion.Euler(0,0, 180);
            newGravity = Vector2.up;
        }
    }

    void Update()
    {
        if(GetComponentInChildren<EnemyVision>().getIsHidding()
        || !GetComponentInChildren<EnemyAttentionArea>().getGotAttention() && !GetComponentInChildren<EnemyActionArea>().getTakeAction())
        {
            patrol();
        }

        if(GetComponentInChildren<EnemyAttentionArea>().getGotAttention() && !GetComponentInChildren<EnemyActionArea>().getTakeAction())
        {
            if(transform.position.x >= player.transform.position.x && facingRight)
            {
                change = true;
                flip();
            }
            else if(transform.position.x < player.transform.position.x && !facingRight)
            {
                change = true;
                flip();
            }
        }
        else if(GetComponentInChildren<EnemyActionArea>().getTakeAction())
        {
            stringShot();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(newGravity*rb.mass*gravityIntensity);
        
        groundedR = Physics2D.OverlapCircle(checkR.position, radius, whatIsGround);
        groundedL = Physics2D.OverlapCircle(checkL.position, radius, whatIsGround);
        wallR = Physics2D.OverlapCircle(checkWallR.position, radius, whatIsGround);
        wallL = Physics2D.OverlapCircle(checkWallR.position, radius, whatIsGround);

    }

    private float curCanTurn = 1f, canTurn = 1f, turn = 1f, curTurn = 1f;
    private void patrol()
    {
        curCanTurn -= Time.deltaTime;
        if(groundedR && groundedL
        && !wallL && !wallR
        && curCanTurn > 0)
        {
            curTurn = turn;
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
        }
        else if(groundedR && !groundedL 
            || groundedR && !groundedL 
            || wallR || wallL
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

    private float fireRate = 60f, curFireRate = 40f;
    private void stringShot()
    {
        curFireRate++;
        if(curFireRate > fireRate)
        {
            GetComponentInChildren<EnemyProjectileTrigger>().shotTrigger();
            curFireRate = 0;
        }
    }

    bool change, facingRight = true;
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

    public bool show;
    private void OnDrawGizmos()
    {
        if(show)
        {
            Gizmos.DrawWireSphere(checkR.position, radius);
            Gizmos.DrawWireSphere(checkL.position, radius);
            Gizmos.DrawWireSphere(checkWallL.position, radius);
            Gizmos.DrawWireSphere(checkWallR.position, radius);
        }
    }
}
