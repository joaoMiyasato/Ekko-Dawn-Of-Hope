using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_IA_boss : MonoBehaviour
{
    public GameObject Player;
    public Transform state3;
    private EnemyBase enemyBase;
    private Rigidbody2D rb;
    private int state = 1;
    private float dist;
    [Range(0,99)]public int S;
    private float timer;
    public GameObject bullet;
    //////////////Jump/////////////////////////////////////
    private float curJumpTime;
    private float jumpTime = 0.8f;
    public Vector2 jumpHeight;
    ////////////////Ahead//////////////////////////////////
    private float curAheadTime;
    private float aheadTime = 1f;
    public Vector2 aheadSpeed;
    ////////////////Drop//////////////////////////////////
    private float curDropTime;
    private float dropTime = 0.5f;
    private int qtd;
    private bool drop;
    void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        rb = GetComponent<Rigidbody2D>();
        
        aheadSpeed.x = 15f;
        aheadSpeed.y = rb.velocity.y;
    }

    void Update()
    {
        dist = this.transform.position.x - Player.transform.position.x;
        
        if(curAheadTime > 0)
        {
            if(dist <= 0)
            {
                aheadSpeed.x = 15;
            }
            else if(dist > 0)
            {
                aheadSpeed.x = -15f;
            }
        }
    }

    private void FixedUpdate()
    {
        switchState();
        if(enemyBase.bossActivated)
        {   
            if(state == 0)
            {
                timer += Time.fixedDeltaTime;
                if(timer > 0.5f)
                {
                    timer = 0;
                    S = Random.Range(0,99);
                }
            }
            else if(state == 1)
            {
                Jump();
            }
            else if(state == 2)
            {
                Ahead();
            }
            else if(state == 3)
            {
                Drop();
            }
            detectRayCollision();
        }
    }

    private void Jump()
    {
        curJumpTime -= Time.fixedDeltaTime;
        if(curJumpTime < 0 && curJumpTime >= -0.02)
        {
            if(Mathf.Abs(dist) > 25)
            {
                jumpHeight.x = Random.Range(20,25);
            }
            else if(Mathf.Abs(dist) <= 25 && Mathf.Abs(dist) > 15)
            {
                jumpHeight.x = Random.Range(10,15);
            }
            else if(Mathf.Abs(dist) <= 15)
            {
                jumpHeight.x = Random.Range(5,10);
            }
            if(dist <= 0)
            {
                jumpHeight.x = Mathf.Abs(jumpHeight.x);
            }
            else if(dist > 0)
            {
                jumpHeight.x = Mathf.Abs(jumpHeight.x)*-1;
            }
            rb.AddForce(jumpHeight,ForceMode2D.Impulse);
        }
        else if(curJumpTime < -1.5f)
        {
            curJumpTime = jumpTime;
            S = Random.Range(0,99);
        }
    }

    private void Ahead()
    {
        curAheadTime -= Time.fixedDeltaTime;
        if(curAheadTime < 0 && curAheadTime >= -2.5f)
        {
            rb.velocity = aheadSpeed;
        }
        else if(curAheadTime < -3.5f)
        {
            rb.velocity = Vector2.zero;
            curAheadTime = aheadTime;
            S = Random.Range(0,99);
        }
    }

    private void Drop()
    {
        curDropTime -= Time.fixedDeltaTime;
        if(curDropTime < 0 && curDropTime >= -0.25f)
        {
            if(drop)
            {
                drop = false;
                GameObject obj1 = Instantiate(bullet, (Vector2)state3.position + new Vector2(Random.Range(-21,-10),0), Quaternion.identity);
                GameObject obj2 = Instantiate(bullet, (Vector2)state3.position + new Vector2(Random.Range(-10,0),0), Quaternion.identity);
                GameObject obj3 = Instantiate(bullet, (Vector2)state3.position + new Vector2(Random.Range(0,10),0), Quaternion.identity);
                GameObject obj4 = Instantiate(bullet, (Vector2)state3.position + new Vector2(Random.Range(10,21),0), Quaternion.identity);
                GameObject obj5 = Instantiate(bullet, (Vector2)state3.position + new Vector2(Random.Range(-21,21),0), Quaternion.identity);
                GameObject obj6 = Instantiate(bullet, (Vector2)state3.position + new Vector2(Random.Range(-21,21),0), Quaternion.identity);
                GameObject obj7 = Instantiate(bullet, (Vector2)state3.position + new Vector2(Random.Range(-21,21),0), Quaternion.identity);
                qtd += 1;
            }
        }
        else if(curDropTime < -0.25f)
        {
            drop = true;
            curDropTime = dropTime;
        }
        else if(qtd > 10)
        {
            qtd = 0;
            S = Random.Range(0,99);
        }
    }

    private void switchState()
    {
        if(S < 20)
        {
            state = 0;
        }
        else if(S >= 20 && S < 50)
        {
            state = 1;
        }
        else if(S >= 50 && S < 75)
        {
            state = 2;
        }
        else if(S >= 75)
        {
            state = 3;
        }
    }

    private void detectRayCollision()
    {

    }

    private void OnDrawGizmos()
    {

    }
}
