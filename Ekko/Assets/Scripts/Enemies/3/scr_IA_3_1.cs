using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_IA_3_1 : MonoBehaviour
{
    public Rigidbody2D rb;
    private GameObject Player;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public Transform Rblock;
    public Transform Lblock;
    public Transform Rfeet;
    public Transform Lfeet;
    //////////////////////Ray//////////////////////////////////
    public LayerMask whatIsRay;
    public Transform ray1, ray2;
    public float Vision = 4f;
    public bool showRay = true;
    /////////////////////Patrolling///////////////////////////
    private bool troca = false;
    private bool facingRight = true;
    private bool facingWall = false;
    private bool isGroundedL;
    private bool isGroundedR;
    private bool wallR;
    private bool wallL;
    private float turn;
    private float curTurn;
    private float canTurn;
    private float curCanTurn;
    [Range(1,3)] public float Speed = 1f;
    public Vector2 Recovering;
    private float initiatePatrolling = 0.8f;
    private float curInitiatePatrolling;
    private bool patrolling = false;
    //////////////////////Attacking////////////////////////
    private bool attacking = false;
    private float attackTime = 0.8f;
    private float curAttackTime;
    public Vector2 attack;
    private int A = 12, B = 9, C = 6, D = 3;
    void Start()
    {
        turn = Random.Range(1f, 4f);
        curTurn = turn;
        canTurn = Random.Range(2f,8f);
        curCanTurn = canTurn;
        Recovering.x *= -1;
        curAttackTime = attackTime;

        Player = GameObject.Find("Player");
    }

    void Update()
    {
        if(isGroundedL || isGroundedR)
        {
            curInitiatePatrolling -= Time.deltaTime;
            if(curInitiatePatrolling <= 0)
            {
                patrolling = true;
            }
        }
        else
        {
            patrolling = false;
            curInitiatePatrolling = initiatePatrolling;
        }

        if(wallL || wallR)
        {
            facingWall = true;
        }
        else
        {
            facingWall = false;
        }

        if(patrolling && !GetComponent<EnemyBase>().Recover && !attacking)
        {
            Patrolling();
        }
        else if(GetComponent<EnemyBase>().Recover)
        {
            Back();
        }
    }

    private void FixedUpdate()
    {
        isGroundedR = Physics2D.OverlapCircle(Rfeet.position, 0.05f, whatIsGround);
        isGroundedL = Physics2D.OverlapCircle(Lfeet.position, 0.05f, whatIsGround);
        
        wallR = Physics2D.OverlapCircle(Rblock.position, 0.05f, whatIsWall);
        wallL = Physics2D.OverlapCircle(Lblock.position, 0.05f, whatIsWall);

        detectRayCollision();

        if(Mathf.Abs(Player.transform.position.x - this.transform.position.x) > 8)
        {
            attack.x = A;
            attack.y = 30;
        }
        else if(Mathf.Abs(Player.transform.position.x - this.transform.position.x) <= 8 && Mathf.Abs(Player.transform.position.x - this.transform.position.x) > 6)
        {
            attack.x = B;
            attack.y = 26;
        }
        else if(Mathf.Abs(Player.transform.position.x - this.transform.position.x) <= 6 && Mathf.Abs(Player.transform.position.x - this.transform.position.x) > 4)
        {
            attack.x = C;
            attack.y = 22;
        }
        else if(Mathf.Abs(Player.transform.position.x - this.transform.position.x) <= 4)
        {
            attack.x = D;
            attack.y = 20;
        }
        if(!GetComponent<EnemyBase>().Recover && attacking)
        {
            Attacking();
        }
    }

    private void Patrolling()
    {
        curCanTurn -= Time.deltaTime;
        if(isGroundedL && isGroundedR && !facingWall && curCanTurn > 0)
        {
            curTurn = turn;
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }
        else if(isGroundedL && !isGroundedR && !facingWall || isGroundedR && !isGroundedL && !facingWall || facingWall || curCanTurn < 0)
        {
            if(curTurn > 0)
            {
                canTurn = Random.Range(8f,18f);
                curTurn -= Time.deltaTime;
                troca = true;
            }
            else
            {
                curCanTurn = canTurn;
                turn = Random.Range(4f, 8f);
                Flip();
                rb.velocity = new Vector2(Speed, rb.velocity.y);
            }
        }
    }
    
    private void Attacking()
    {
        curAttackTime -= Time.fixedDeltaTime;
        if(curAttackTime < 0 && curAttackTime >= -0.02)
        {
            rb.AddForce(attack,ForceMode2D.Impulse);
        }
        else if(curAttackTime < -1f)
        {
            attacking = false;
            curAttackTime = attackTime;
        }
    }
    private void Back()
    {
        if(GetComponent<EnemyBase>().Back)
        {
            attacking = false;
            curAttackTime = attackTime;
            if(Player.transform.position.x > this.gameObject.transform.position.x)
            {
                if(Recovering.x > 0)
                {
                    Recovering.x *= -1;
                }
            }
            else if(Player.transform.position.x < this.gameObject.transform.position.x)
            {
                if(Recovering.x < 0)
                {
                    Recovering.x *= -1;
                }
            }
            if(Player.transform.position.y < this.transform.position.y)
            {
                rb.AddForce(Recovering,ForceMode2D.Impulse);
            }
        }
        GetComponent<EnemyBase>().Back = false;
    }

    void detectRayCollision()
    {
        RaycastHit2D hit1, hit1_2;
        RaycastHit2D hit2, hit2_2;

        hit1 = Physics2D.Raycast(ray1.position, Vector2.right, Vision, whatIsRay);
        hit2 = Physics2D.Raycast(ray1.position, Vector2.left, Vision, whatIsRay);
        hit1_2 = Physics2D.Raycast(ray2.position, Vector2.right, Vision, whatIsRay);
        hit2_2 = Physics2D.Raycast(ray2.position, Vector2.left, Vision, whatIsRay);

        if(hit1.collider != null)
        {
            if(hit1.collider.tag == "Player")
            {
                attacking = true;
                if(!facingRight)
                {
                    troca = true;
                    Flip();
                }
            }
        }
        if(hit2.collider != null)
        {
            if(hit2.collider.tag == "Player")
            {
                attacking = true;
                if(facingRight)
                {
                    troca = true;
                    Flip();
                }
            }
        }
        if(hit1_2.collider != null)
        {
            if(hit1_2.collider.tag == "Player")
            {
                attacking = true;
                if(!facingRight)
                {
                    troca = true;
                    Flip();
                }
            }
        }
        if(hit2_2.collider != null)
        {
            if(hit2_2.collider.tag == "Player")
            {
                attacking = true;
                if(facingRight)
                {
                    troca = true;
                    Flip();
                }
            }
        }
    }
    
    private void Flip()
    {
        if(troca)
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
            Speed *= -1;
            attack.x *= -1;
            A *= -1;
            B *= -1;
            C *= -1;
            D *= -1;
            troca = false;
        }
    }

    private void OnDrawGizmos() 
    {
        if(showRay)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray1.position, ray1.position + transform.right*Vision);
            Gizmos.DrawLine(ray1.position, ray1.position + transform.right*-Vision);
            Gizmos.DrawLine(ray2.position, ray2.position + transform.right*Vision);
            Gizmos.DrawLine(ray2.position, ray2.position + transform.right*-Vision);
        }
    }
}
