using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHabilities : MonoBehaviour
{
    private void Start()
    {
        jumpForce.y = wallJump.y;
    }
    private void Update()
    {
        if(PlayerManager.instance.getSkill_Impact())
            groundImpact();
        if(PlayerManager.instance.getSkill_WallJump())
            WallJump();
        if(PlayerManager.instance.getSkill_WaterBubble())
            WaterBubble();
    }
    private void FixedUpdate()
    {
        Heal();
        if(PlayerManager.instance.getSkill_WallJump() && !PlayerManager.instance.playerMovement.isJumping)
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
        if(Input.GetKey(KeyCode.Z) && PlayerManager.instance.playerBase.getCurrentLife() < PlayerManager.instance.playerBase.getMaxLife() && PlayerManager.instance.playerMovement.isGrounded && !PlayerManager.instance.playerBase.getCantAction())
        {
            healing = true;
            curHealTime += Time.fixedDeltaTime;
            if(curHealTime > (healTime/2 + 0.015f) && curHealTime < healTime)
            {
                PlayerManager.instance.playerBase.addEnergy(-2);
            }
            else if(curHealTime >= healTime && PlayerManager.instance.playerBase.getCurrentEnergy() >= 0)
            {
                healed = true;
                curHealTime = 0;
            }
            else if(curHealTime >= healTime && PlayerManager.instance.playerBase.getCurrentEnergy() < 0)
            {
                PlayerManager.instance.playerBase.setCurEnergy(0);
                curHealTime = 0;
            }
        }
        else
        {
            healing = false;
        }

        if(healed)
        {
            PlayerManager.instance.playerBase.addLife(1);
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
            PlayerManager.instance.playerBase.getCurrentEnergy() >= 100 && 
            !PlayerManager.instance.playerBase.getCantAction() && 
            !PlayerManager.instance.playerBase.getRefreshSkill() && 
            !onWater)
        {
            GImpact = true;
            PlayerManager.instance.playerBase.addEnergy(-100);
            PlayerManager.instance.GravityChange(0);
            PlayerManager.instance.rb.velocity = Vector2.zero;
            if(PlayerManager.instance.playerMovement.isGrounded)
            {
                PlayerManager.instance.rb.AddForce(Vector2.up * 1000,ForceMode2D.Impulse);
            }
            StartCoroutine(impact());
            PlayerManager.instance.playerBase.setRefreshSkill(true);
        }
        if(impactTrigger)
        {
            impactTrigger = false;
            StartCoroutine(PlayerManager.instance.playerBase.refreshTime(0.75f));
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(impactPosition.position, impactRange, PlayerManager.instance.playerOffensiveAttack.enemyLayers);
            foreach(Collider2D hit in hitEnemies)
            {
                if(hit.gameObject.tag == "Ememy")
                {
                    hit.GetComponent<EnemyBase>().takeDamage(impactDamage);
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
        PlayerManager.instance.GravityChange(1);
        PlayerManager.instance.rb.velocity = Vector2.down*50;
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

        if(!wallJumping)
        {
            if( hitD.collider != null)
            {
                Rside = true;
                if(Input.GetKey(KeyCode.RightArrow) && !PlayerManager.instance.playerBase.getCantAction())
                {
                    if( !PlayerManager.instance.playerMovement.isGrounded && hitD.collider.tag == "Wall" ||
                        !PlayerManager.instance.playerMovement.isGrounded && hitD.collider.tag == "InteractableWall" ||
                        !PlayerManager.instance.playerMovement.isGrounded && hitD.collider.tag == "Ground")
                    {
                        sliding = true;
                        if(PlayerManager.instance.rb.velocity.y < slideSpeed)
                        {
                            PlayerManager.instance.rb.velocity = new Vector2(0,slideSpeed);
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
                    if( !PlayerManager.instance.playerMovement.isGrounded && hitL.collider.tag == "Wall" ||
                        !PlayerManager.instance.playerMovement.isGrounded && hitL.collider.tag == "InteractableWall" ||
                        !PlayerManager.instance.playerMovement.isGrounded && hitL.collider.tag == "Ground")
                    {
                        sliding = true;
                        if(PlayerManager.instance.rb.velocity.y < slideSpeed)
                        {
                            PlayerManager.instance.rb.velocity = new Vector2(0,slideSpeed);
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

    }
    private void WallJump()
    {
        if(sliding)
        {
            if(Input.GetKeyDown(KeyCode.Space) && !PlayerManager.instance.playerBase.getCantAction())
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
                    PlayerManager.instance.playerMovement.jumpTimeCounter = PlayerManager.instance.playerMovement.jumpTime/3;
                }
            }
        }
        if(wallJumping)
        {
            PlayerManager.instance.playerMovement.xtraspeed = true;
            PlayerManager.instance.playerMovement.curSpeed = PlayerManager.instance.playerMovement.xtraSpeed;
            PlayerManager.instance.rb.velocity = jumpForce;
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

            PlayerManager.instance.rb.AddForce(Vector2.up*75, ForceMode2D.Force);

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

            if(PlayerManager.instance.playerMovement.facingRight)
            {
                wX = Mathf.Cos(angle * Mathf.Deg2Rad);
                wY = Mathf.Sin(angle * Mathf.Deg2Rad);
            }
            else if(!PlayerManager.instance.playerMovement.facingRight)
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
                PlayerManager.instance.rb.AddForce(new Vector2(wX*power,wY*power),ForceMode2D.Impulse);
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
        if(onWater && PlayerManager.instance.getSkill_WaterBubble())
        {
            if(inWaterBubble)
            {
                PlayerManager.instance.GravityChange(0);
                PlayerManager.instance.rb.mass = 50f;
            }
            else if(!inWaterBubble && diving)
            {
                PlayerManager.instance.GravityChange(1);
                PlayerManager.instance.rb.mass = 50f;
            }
            else if(floating)
            {
                PlayerManager.instance.GravityChange(1);
                PlayerManager.instance.rb.mass = 10f;
            }
        }
        else if(!onWater)
        {
            floating = false;
            PlayerManager.instance.rb.mass = 50f;
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
            PlayerManager.instance.DragChange(12);
        }
        if(PlayerManager.instance.rb.drag > 2.5f)
        {
            PlayerManager.instance.rb.drag -= 0.8f;
        }
        else
        {
            PlayerManager.instance.DragChange(2.5f);
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
