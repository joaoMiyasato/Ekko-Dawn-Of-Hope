using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_IA_1_1 : MonoBehaviour
{
    public bool chasing = false;
    public float speedX,speedY;
    private float curSX, curSY;
    private Rigidbody2D rb;
    private GameObject player;
    private bool facingRight;
    private float x,y,Rx,Ry;
    public Vector2 Recovering;
    private float distX, distY, sDistX, sDistY;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");

        x = this.transform.position.x;
        y = this.transform.position.y;

        Rx = Recovering.x;
        Ry = Recovering.y;
    }

    void Update()
    {
        distX = Mathf.Abs(this.transform.position.x) - Mathf.Abs(player.transform.position.x);
        distY = Mathf.Abs(this.transform.position.y) - Mathf.Abs(player.transform.position.y);

        sDistX = Mathf.Abs(this.transform.position.x) - Mathf.Abs(x);
        sDistY = Mathf.Abs(this.transform.position.y) - Mathf.Abs(y);
    }

    private void FixedUpdate()
    {
        if(chasing && !GetComponent<scr_IA_base>().Recover)
        {
            inChase();
        }
        else if(!chasing)
        {
            offChase();
        }
        else if(GetComponent<scr_IA_base>().Recover)
        {
            Back();
        }
    }

    private void inChase()
    {
        if(distX >= 0)
        {
            if(transform.localScale.x > 0)
            {
                FlipX();
            }
            
            curSX -= Time.fixedDeltaTime*15;
            if(curSX < speedX)
            {
                curSX = speedX;
            }
        }
        else if(distX < 0)
        {
            if(transform.localScale.x < 0)
            {
                FlipX();
            }

            curSX += Time.fixedDeltaTime*15;
            if(curSX > speedX)
            {
                curSX = speedX;
            }
        }

        if(distY <= 0)
        {
            if(transform.localScale.y > 0)
            {
                FlipY();
            }

            curSY += Time.fixedDeltaTime*15;
            if(curSY > speedY)
            {
                curSY = speedY;
            }
        }
        else if(distY >= 0)
        {
            if(transform.localScale.y < 0)
            {
                FlipY();
            }

            curSY -= Time.fixedDeltaTime*15;
            if(curSY < speedY)
            {
                curSY = speedY;
            }
        }

        rb.velocity = new Vector2(curSX,curSY);
    }

    private void offChase()
    {
        if(sDistX > 0)
        {
            if(transform.localScale.x > 0)
            {
                FlipX();
            }
            
            curSX = -1;
        }
        else if(sDistX < 0)
        {
            if(transform.localScale.x < 0)
            {
                FlipX();
            }

            curSX = 1;
        }

        if(sDistY < 0)
        {
            if(transform.localScale.y > 0)
            {
                FlipY();
            }

            curSY = 1;
        }
        else if(sDistY > 0)
        {
            if(transform.localScale.y < 0)
            {
                FlipY();
            }

            curSY = -1;
        }
    }

    private void FlipX()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        speedX *= -1;
        transform.localScale = Scaler;
    }

    private void FlipY()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.y *= -1;
        speedY *= -1;
        transform.localScale = Scaler;
    }

    private void Back()
    {
        if(GetComponent<scr_IA_base>().Back)
        {
            if(distX >= 0.5)
            {
                Rx = Recovering.x;
            }
            else if(distX <= -0.5)
            {
                Rx = -Recovering.x;
            }
            else if(distX > -0.5 && distX < 0.5)
            {
                Rx = 0f;
            }

            if(distY <= -0.5)
            {
                Ry = -Recovering.y;
            }
            else if(distY >= 0.5)
            {
                Ry = Recovering.y;
            }
            else if(distY > -0.5 && distY < 0.5)
            {
                Ry = 0f;
            }
        rb.AddForce(new Vector2(Rx,Ry),ForceMode2D.Impulse);
        curSX = 0;
        curSY = 0;
        GetComponent<scr_IA_base>().Back = false;
        }
    }
}
