using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform pos;
    public float damp;
    private Vector3 velocity = Vector3.zero;
    private Vector2 dir;
    private float changeT;
    private bool change;

    public float Yspeed, T;
    private float ycurSpd;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        dir.x = rb.velocity.x;
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, pos.position, ref velocity, damp);

        changeT += Time.fixedDeltaTime;

        if(changeT > T)
        {
            changeT = 0;
            Yspeed = -Yspeed;
        }
        if(ycurSpd > Yspeed)
        {
            ycurSpd += Time.fixedDeltaTime*2;
        }
        else if(ycurSpd < Yspeed)
        {
            ycurSpd -= Time.fixedDeltaTime*2;
        }
        dir.y = ycurSpd;
        rb.velocity = dir;
    }
}
