using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_IA_4_1 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 myNormal;
    private EnemyBase enemyBase;
    public Transform right, left, up, down, right2, left2, up2, down2;
    private bool isGroundedR, isGroundedL, isGroundedU, isGroundedD;
    public LayerMask whatIsGround;
    private float gravity = 80f;
    private Vector2 dir;
    public float speed = 1f;
    public bool Sright = true;
    public bool Sup = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyBase = GetComponent<EnemyBase>();

        if(!Sup)
        {
            myNormal = Vector2.up;
        }
        else
        {
            myNormal = Vector2.down;
        }
    }

    void Update()
    {
        if(Sright)
        {
            if(isGroundedD && !isGroundedR)
            {
                //DIREITA
                dir.x = 1*speed;
                dir.y = rb.velocity.y;
                myNormal = Vector2.up;
            }
            else if(isGroundedU && !isGroundedL)
            {
                //ESQUERDA
                dir.x = -1*speed;
                dir.y = rb.velocity.y;
                myNormal = Vector2.down;
            }
            else if(isGroundedR || isGroundedR && isGroundedD)
            {
                //CIMA
                dir.x = rb.velocity.x;
                dir.y = 1*speed;
                myNormal = Vector2.left;
            }
            else if(isGroundedL || isGroundedL && isGroundedU)
            {
                //BAIXO
                dir.x = rb.velocity.x;
                dir.y = -1*speed;
                myNormal = Vector2.right;
            }
            else
            {
                dir.x = rb.velocity.x;
                dir.y = rb.velocity.y;
            }
        }
        else
        {
            if(isGroundedD && !isGroundedL)
            {
                //ESQUERDA
                dir.x = -1*speed;
                dir.y = rb.velocity.y;
                myNormal = Vector2.up;
            }
            else if(isGroundedU && !isGroundedR)
            {
                //DIREITA
                dir.x = 1*speed;
                dir.y = rb.velocity.y;
                myNormal = Vector2.down;
            }
            else if(isGroundedR || isGroundedL && isGroundedU)
            {
                //BAIXO
                dir.x = rb.velocity.x;
                dir.y = -1*speed;
                myNormal = Vector2.left;
            }
            else if(isGroundedL || isGroundedR && isGroundedD)
            {
                //CIMA
                dir.x = rb.velocity.x;
                dir.y = 1*speed;
                myNormal = Vector2.right;
            }
            else
            {
                dir.x = rb.velocity.x;
                dir.y = rb.velocity.y;
            }
        }
    }

    private void FixedUpdate()
    {
        if(!enemyBase.Recover)
        {
            rb.velocity = dir;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        rb.AddForce(-gravity*rb.mass*myNormal);

        detecRayCollision();
    }

    private void detecRayCollision()
    {
        RaycastHit2D L, R, D, U, L2, R2, D2, U2;

        L = Physics2D.Raycast(left.position, Vector2.left, 0.5f, whatIsGround);
        R = Physics2D.Raycast(right.position, Vector2.right, 0.5f, whatIsGround);
        D = Physics2D.Raycast(down.position, Vector2.down, 0.5f, whatIsGround);
        U = Physics2D.Raycast(up.position, Vector2.up, 0.5f, whatIsGround);
        L2 = Physics2D.Raycast(left2.position, Vector2.left, 0.5f, whatIsGround);
        R2 = Physics2D.Raycast(right2.position, Vector2.right, 0.5f, whatIsGround);
        D2 = Physics2D.Raycast(down2.position, Vector2.down, 0.5f, whatIsGround);
        U2 = Physics2D.Raycast(up2.position, Vector2.up, 0.5f, whatIsGround);

        if(L.collider != null || L2.collider != null)
        {
            isGroundedL = true;
        }
        else
        {
            isGroundedL = false;
        }
        if(R.collider != null || R2.collider != null)
        {
            isGroundedR = true;
        }
        else
        {
            isGroundedR = false;
        }
        if(D.collider != null || D2.collider != null)
        {
            isGroundedD = true;
        }
        else
        {
            isGroundedD = false;
        }
        if(U.collider != null || U2.collider != null)
        {
            isGroundedU = true;
        }
        else
        {
            isGroundedU = false;
        }
    }

    private void OnDrawGizmos()
    {    
        Gizmos.color = Color.red;
        Gizmos.DrawLine(left.position, left.position + transform.right*-1);
        Gizmos.DrawLine(right.position, right.position + transform.right*1);
        Gizmos.DrawLine(up.position, up.position + transform.up*1);
        Gizmos.DrawLine(down.position, down.position + transform.up*-1);
        Gizmos.DrawLine(left2.position, left2.position + transform.right*-1);
        Gizmos.DrawLine(right2.position, right2.position + transform.right*1);
        Gizmos.DrawLine(up2.position, up2.position + transform.up*1);
        Gizmos.DrawLine(down2.position, down2.position + transform.up*-1);
    }
    }
