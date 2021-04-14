using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_player_habilities : MonoBehaviour
{
    private void Start()
    {
        jumpForce.y = wallJump.y;
    }
    private void Update()
    {
        if(scr_player_manager.instance.Skill_Impact)
            groundImpact();
        if(scr_player_manager.instance.Skill_Walljump)
            WallJump();
        if(scr_player_manager.instance.Skill_WaterBubble)
            WaterBubble();
    }
    private void FixedUpdate()
    {
        if(scr_player_manager.instance.curEnergy > scr_player_manager.instance.maxEnergy)
        {
            scr_player_manager.instance.curEnergy = scr_player_manager.instance.maxEnergy;
        }

        Heal();
        if(scr_player_manager.instance.Skill_Walljump && !scr_player_manager.instance.Pmove.isJumping)
            WallSlide();
        WaterPhysics();
    }

#region Heal
    private float healTime = 2f;
    private float curHealTime;
    public bool healing;
    private bool healed = false;

    private void Heal()
    {
        if(Input.GetKey(KeyCode.Z) && scr_player_manager.instance.curLife < scr_player_manager.instance.maxLife && scr_player_manager.instance.Pmove.isGrounded && !scr_player_manager.instance.cantAction)
        {
            healing = true;
            curHealTime += Time.fixedDeltaTime;
            if(curHealTime > (healTime/2 + 0.015f) && curHealTime < healTime)
            {
                scr_player_manager.instance.curEnergy -= 2;
            }
            else if(curHealTime >= healTime && scr_player_manager.instance.curEnergy >= 0)
            {
                healed = true;
                curHealTime = 0;
            }
            else if(curHealTime >= healTime && scr_player_manager.instance.curEnergy < 0)
            {
                scr_player_manager.instance.curEnergy = 0;
                curHealTime = 0;
            }
        }
        else
        {
            healing = false;
        }

        if(healed)
        {
            scr_player_manager.instance.curLife += 1;
            healed = false;
        }
    }

#endregion

#region GroundImpact
    public Transform impactPosition;
    public bool GImpact;
    public float impactRange;
    public int impactDamage, impactStage;
    public bool impactTrigger;
    public bool destroyGround;
    private void groundImpact()
    {
        if( Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.C) && 
            scr_player_manager.instance.curEnergy >= 100 && 
            !scr_player_manager.instance.cantAction && 
            !scr_player_manager.instance.refreshSkill && 
            !onWater)
        {
            GImpact = true;
            scr_player_manager.instance.curEnergy -= 100;
            scr_player_manager.instance.GravityChange(0);
            scr_player_manager.instance.rb.velocity = Vector2.zero;
            if(scr_player_manager.instance.Pmove.isGrounded)
            {
                scr_player_manager.instance.rb.AddForce(Vector2.up * 1000,ForceMode2D.Impulse);
            }
            StartCoroutine(impact());
            scr_player_manager.instance.refreshSkill = true;
        }
        if(impactTrigger)
        {
            impactTrigger = false;
            StartCoroutine(scr_player_manager.instance.refreshTime(0.75f));
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(impactPosition.position, impactRange, scr_player_manager.instance.Pattack.enemyLayers);
            foreach(Collider2D hit in hitEnemies)
            {
                if(hit.gameObject.tag == "Ememy")
                {
                    hit.GetComponent<scr_IA_base>().takeDamage(impactDamage);
                }
                else if(hit.gameObject.tag == "GroundDestructable" && destroyGround)
                {
                    destroyGround = false;
                    hit.GetComponent<DestroyObject>().Impact_Check = true;
                }
            }
        }
    }

    private IEnumerator impact()
    {
        yield return new WaitForSeconds(0.2f);
        scr_player_manager.instance.GravityChange(1);
        scr_player_manager.instance.rb.velocity = Vector2.down*50;
    }
#endregion

#region Walljump
    public float wallrange;
    public float slideSpeed;
    public bool sliding = false;
    public bool wallJumping = false;
    public Vector2 wallJump;
    private Vector2 jumpForce;
    public bool Rside;
    private void WallSlide()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitD = Physics2D.Raycast(transform.position,Vector2.right,wallrange);
        RaycastHit2D hitL = Physics2D.Raycast(transform.position,Vector2.left,wallrange);

        if( hitD.collider != null)
        {
            Rside = true;
            if(Input.GetKey(KeyCode.RightArrow) && !scr_player_manager.instance.cantAction)
            {
                if( !scr_player_manager.instance.Pmove.isGrounded && hitD.collider.tag == "Wall" ||
                    !scr_player_manager.instance.Pmove.isGrounded && hitD.collider.tag == "InteractableWall" ||
                    !scr_player_manager.instance.Pmove.isGrounded && hitD.collider.tag == "Ground")
                {
                    sliding = true;
                    if(scr_player_manager.instance.rb.velocity.y < slideSpeed)
                    {
                        scr_player_manager.instance.rb.velocity = new Vector2(0,slideSpeed);
                    }
                }
                else
                {
                    StartCoroutine(slidefalse());
                }
            }
        }
        else if(hitL.collider != null && Input.GetKey(KeyCode.LeftArrow))
        {
            Rside = false;
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                if( !scr_player_manager.instance.Pmove.isGrounded && hitL.collider.tag == "Wall" ||
                    !scr_player_manager.instance.Pmove.isGrounded && hitL.collider.tag == "InteractableWall" ||
                    !scr_player_manager.instance.Pmove.isGrounded && hitL.collider.tag == "Ground")
                {
                    sliding = true;
                    if(scr_player_manager.instance.rb.velocity.y < slideSpeed)
                    {
                        scr_player_manager.instance.rb.velocity = new Vector2(0,slideSpeed);
                    }
                }
                else
                {
                    StartCoroutine(slidefalse());
                }
            }
        }
        else
        {
            StartCoroutine(slidefalse());
        }

    }
    private void WallJump()
    {
        if(sliding)
        {
            if(Input.GetKeyDown(KeyCode.Space) && !scr_player_manager.instance.cantAction)
            {
                if(Rside)
                {
                    jumpForce.x = wallJump.x*Vector2.left.x;
                }
                else
                {
                    jumpForce.x = wallJump.x*Vector2.right.x;
                }
                if(!GameManager.instance.isPaused)
                {
                    wallJumping = true;
                    StartCoroutine(jump());
                    scr_player_manager.instance.Pmove.jumpTimeCounter = scr_player_manager.instance.Pmove.jumpTime/3;
                }
            }
        }
        if(wallJumping)
        {
            scr_player_manager.instance.Pmove.xtraspeed = true;
            scr_player_manager.instance.Pmove.curSpeed = scr_player_manager.instance.Pmove.xtraSpeed;
            scr_player_manager.instance.rb.velocity = jumpForce;
        }
    }
    private IEnumerator jump()
    {
        yield return new WaitForSeconds(0.15f);
        wallJumping = false;
    }
    private IEnumerator slidefalse()
    {
        yield return new WaitForSeconds(0.1f);
        sliding = false;
    }
#endregion

#region WaterBubble
    public bool onWater,diving, floating;
    public float rotSpd = 2;
    public bool inWaterBubble;
    private float changeAngle;
    private float angle;
    private float power;
    public float maxPower = 25;
    private float wX,wY;
    public bool cancelBubble;
    public GameObject theBubble;
    public SpriteRenderer normal;
    private void WaterBubble()
    {
        if(inWaterBubble)
        {
            justEntered();
            diving = true;
            if(!onWater && Input.GetKeyDown(KeyCode.C))
            {
                inWaterBubble = false;
            }

            normal.enabled = false;
            theBubble.SetActive(true);

            scr_player_manager.instance.rb.AddForce(Vector2.up*75, ForceMode2D.Force);

            changeAngle = Input.GetAxisRaw("Horizontal");
            angle += -changeAngle*rotSpd;
            if(angle > 360)
            {
                angle = 0;
            }
            else if(angle < 0)
            {
                angle = 360;
            }
            transform.rotation = Quaternion.Euler(0,0, angle);

            if(scr_player_manager.instance.Pmove.facingRight)
            {
                wX = Mathf.Cos(angle * Mathf.Deg2Rad);
                wY = Mathf.Sin(angle * Mathf.Deg2Rad);
            }
            else if(!scr_player_manager.instance.Pmove.facingRight)
            {
                wX = Mathf.Cos((angle+180) * Mathf.Deg2Rad);
                wY = Mathf.Sin((angle+180) * Mathf.Deg2Rad);
            }

            if(Input.GetKey(KeyCode.Space) && onWater)
            {
                if(power < (maxPower*100f))
                    power += 80f;
            }
            if(Input.GetKeyUp(KeyCode.Space) && onWater)
            {
                scr_player_manager.instance.rb.AddForce(new Vector2(wX*power,wY*power),ForceMode2D.Impulse);
                power = 0;
            }
        }
        else
        {
            normal.enabled = true;
            theBubble.SetActive(false);
            angle = 0;
            transform.rotation = Quaternion.Euler(0,0,0);
        }
    }
    private void WaterPhysics()
    {
        if(onWater && scr_player_manager.instance.Skill_WaterBubble)
        {
            if(inWaterBubble)
            {
                scr_player_manager.instance.GravityChange(0);
                scr_player_manager.instance.rb.mass = 50f;
            }
            else if(!inWaterBubble && diving)
            {
                scr_player_manager.instance.GravityChange(1);
                scr_player_manager.instance.rb.mass = 50f;
            }
            else if(floating)
            {
                scr_player_manager.instance.GravityChange(1);
                scr_player_manager.instance.rb.mass = 10f;
            }
        }
        else if(!onWater)
        {
            floating = false;
            scr_player_manager.instance.rb.mass = 50f;
            if(!inWaterBubble)
            {
                diving = false;
                justEnter = false;
            }
        } 
    }

    private bool justEnter;
    private void justEntered()
    {
        if(!justEnter)
        {
            justEnter = true;
            scr_player_manager.instance.DragChange(12);
        }
        if(scr_player_manager.instance.rb.drag > 2.5f)
        {
            scr_player_manager.instance.rb.drag -= 0.8f;
        }
        else
        {
            scr_player_manager.instance.DragChange(2.5f);
        }
    }
#endregion





    public bool show = true;
    private void OnDrawGizmosSelected()
    {
        if(show)
        {
            if(impactPosition == null)
            {
                return;
            }
            Gizmos.DrawWireSphere(impactPosition.position, impactRange);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position+Vector2.right*wallrange);

            Gizmos.DrawLine(transform.position, (Vector2)transform.position+(new Vector2(wX*power,wY*power)));
        }
    }
}
