using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_player_move : MonoBehaviour
{
////////////////////////// MOVIMENTAÇÃO //////////////////////////////////////////
    public float SpeedFix = 13f;
    public float curSpeed, maxDropdownSpd;
    private float inDropdownMaxSpd;
    public float xtraSpeed = 23f;
    public bool xtraspeed = false;
    public Vector2 Recover;
    public float MoveInput;
    private bool moving = false;
    public PhysicsMaterial2D slippery;
    public PhysicsMaterial2D normal;
///////////////////////// JUMP ///////////////////////////////////////////
    public float jumpForce = 8.5f;
    public float jumpTime = 0.25f;
    public float jumpTimeCounter;
    private float jumpCount;
    private float singleJump = 1, doubleJump = 2;
    public bool isJumping;
///////////////////////// CHÃO ///////////////////////////////////////////
    public Transform feetPos1;
    public Transform feetPos2;
    public Transform headPos;
    public LayerMask whatIsGround;
    public float checkRadius = 0.4f;
    public bool isGrounded;
    private bool isGrounded1, isGrounded2;
    private bool floorHit;
////////////////////////////////////////////////////////////////////
    public bool facingRight = true;
////////////////////////////////////////////////////////////////////
    private float lookUp, lookDown;
////////////////////////////////////////////////////////////////////
    void Start()
    {
        Recover.x *= -1;

        curSpeed = SpeedFix;
    }
////////////////////////////////////////////////////////////////////
    void Update()
    {
#region Look
        if(!moving && isGrounded)
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                lookUp += Time.deltaTime;
                if(lookUp > 1.5f)
                {
                    scr_player_manager.instance.anim.SetBool("LookingUp", true);
                }
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                lookDown += Time.deltaTime;
                if(lookDown > 1.5f)
                {
                    scr_player_manager.instance.anim.SetBool("LookingDown", true);
                }
            }
            else
            {
                lookDown = 0;
                lookUp = 0;
                scr_player_manager.instance.anim.SetBool("LookingUp", false);
                scr_player_manager.instance.anim.SetBool("LookingDown", false);
            }
        }
#endregion

#region Fall
        if(scr_player_manager.instance.rb.velocity.y < 0)
        {
            if(!scr_player_manager.instance.Pattack.attacking)
            {
                scr_player_manager.instance.anim.SetBool("Falling", true);
            }
            else scr_player_manager.instance.anim.SetBool("Falling", false);
        }
        else scr_player_manager.instance.anim.SetBool("Falling", false);
#endregion

#region DropDown
        if(scr_player_manager.instance.rb.velocity.y <= maxDropdownSpd)
        {
            if(!scr_player_manager.instance.Phabilities.GImpact)
            {
                scr_player_manager.instance.rb.velocity = new Vector2(scr_player_manager.instance.rb.velocity.x, maxDropdownSpd);
            }
            inDropdownMaxSpd += Time.deltaTime; 
            if(inDropdownMaxSpd >= 0.59)
            {
                scr_player_manager.instance.Phabilities.impactStage = 2;
            }
            else if(inDropdownMaxSpd >= 0.17 && inDropdownMaxSpd < 0.59)
            {
            scr_player_manager.instance.Phabilities.impactStage = 1;
            }
            else
            {
                scr_player_manager.instance.Phabilities.impactStage = 0;
            }
        }
        if(isJumping)
        {
            inDropdownMaxSpd = 0;
            scr_player_manager.instance.Phabilities.impactStage = 0;
        }
#endregion

        if(!scr_player_manager.instance.Phabilities.inWaterBubble)
        {
            if(!facingRight && MoveInput > 0 && !scr_player_manager.instance.Phabilities.sliding || facingRight && MoveInput > 0 && scr_player_manager.instance.Phabilities.sliding)
            {
                if(!scr_player_manager.instance.Pattack.attacking)
                {
                    //Direita
                    if(isGrounded)
                    {
                        LongerFlip();
                    }
                    else
                    {
                        InstantFlip();
                    }
                }
            }
            else if(facingRight && MoveInput < 0 && !scr_player_manager.instance.Phabilities.sliding || !facingRight && MoveInput < 0 && scr_player_manager.instance.Phabilities.sliding)
            {
                if(!scr_player_manager.instance.Pattack.attacking)
                {
                    //Esquerda
                    if(isGrounded)
                    {
                        LongerFlip();
                    }
                    else
                    {
                        InstantFlip();
                    }
                }
            }

#region Jump
            if(!scr_player_manager.instance.cantJump && 
            !scr_player_manager.instance.Phabilities.healing &&
            !scr_player_manager.instance.Phabilities.sliding &&
            !scr_player_manager.instance.Phabilities.GImpact)
            {
                jump();
            }

            if(scr_player_manager.instance.Phabilities.wallJumping && scr_player_manager.instance.Skill_DoubleJump)
            {
                jumpCount = 1;
            }
#endregion

            if(scr_player_manager.instance.Pbase.Back)
            {
                Recovering();
            }

#region MaterialSwitch
            if(moving || isJumping)
            {
                scr_player_manager.instance.rb.sharedMaterial = slippery;
            }
            else if(!moving)
            {
                scr_player_manager.instance.rb.sharedMaterial = normal;
            }
#endregion
        }
    }
////////////////////////////////////////////////////////////////////
        private void FixedUpdate() 
    {
#region Checagem
        isGrounded1 = Physics2D.OverlapCircle(feetPos1.position, checkRadius, whatIsGround);
        isGrounded2 = Physics2D.OverlapCircle(feetPos2.position, checkRadius, whatIsGround);
        floorHit = Physics2D.OverlapCircle(headPos.position, checkRadius, whatIsGround);

        if(isGrounded1 || isGrounded2)
        {
            isGrounded = true;
            scr_player_manager.instance.anim.SetBool("IsGrounded", true);
        }
        else
        {
            isGrounded = false;
            scr_player_manager.instance.anim.SetBool("IsGrounded", false);
            scr_player_manager.instance.anim.SetBool("Turning", false);
        } 
#endregion

#region Move
        if(!scr_player_manager.instance.cantMove && 
        !scr_player_manager.instance.Phabilities.healing &&
        !scr_player_manager.instance.Phabilities.inWaterBubble &&
        !scr_player_manager.instance.Phabilities.GImpact)
            move();
#endregion

    }
////////////////////////// MOVIMENTAÇÃO //////////////////////////////////////////
    private void move()
    {
        MoveInput = Input.GetAxisRaw("Horizontal");

        if(xtraspeed)
        {
            curSpeed -= 0.3f;
            if(curSpeed <= SpeedFix)
            {
                curSpeed = SpeedFix;
                xtraspeed = false;
            }
        }

        if(!scr_player_manager.instance.Phabilities.wallJumping)
            scr_player_manager.instance.rb.velocity = new Vector2(MoveInput * curSpeed, scr_player_manager.instance.rb.velocity.y);
        
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            moving = true;
            scr_player_manager.instance.anim.SetBool("Walking", true);
        }
        else
        {
            moving = false;
            scr_player_manager.instance.anim.SetBool("Walking", false);
        }
    }
////////////////////////// JUMP //////////////////////////////////////////
    private void jump()
    {
        if(jumpCount > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            scr_player_manager.instance.anim.SetTrigger("Jump");
            jumpCount--;
            if(!GameManager.instance.isPaused)
                isJumping = true;
            jumpTimeCounter = jumpTime;
            scr_player_manager.instance.rb.velocity = Vector2.up ;
        }

        if(isJumping && Input.GetKey(KeyCode.Space))
        {
            if(jumpTimeCounter > 0)
            {
                scr_player_manager.instance.rb.velocity = Vector2.up * jumpForce*3;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if(isJumping)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                scr_player_manager.instance.rb.velocity = scr_player_manager.instance.rb.velocity / 2;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        if(floorHit)
        {
            isJumping = false;
        }

        if(scr_player_manager.instance.Phabilities.floating)
        {
            if(!scr_player_manager.instance.Skill_DoubleJump)
            {
                jumpCount = singleJump;
            }
            else jumpCount = doubleJump;
        }
    }
    private IEnumerator jumpLess()
    {
        yield return new WaitForSeconds(0.075f);
        jumpCount--;
    }
///////////////////////////// COLISÕES ///////////////////////////////////////
    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.collider.gameObject.tag == "Ground" || other.collider.gameObject.tag == "Chest" || other.collider.gameObject.tag == "GroundDestructable")
        {
            if(!floorHit)
            {
                StartCoroutine(jumpLess());
            }
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.collider.gameObject.tag == "Ground" || other.collider.gameObject.tag == "Chest" || other.collider.gameObject.tag == "GroundDestructable")
        {  
            if(!floorHit && isGrounded)
            {
                if(!scr_player_manager.instance.Skill_DoubleJump)
                {
                    jumpCount = singleJump;
                }
                else jumpCount = doubleJump;
            }

            // if(!scr_player_manager.instance.Phabilities.onWater)
            // {
                // scr_player_manager.instance.Phabilities.cancelBubble = false;
            // }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(inDropdownMaxSpd > 0 && !scr_player_manager.instance.Phabilities.GImpact)
        {
            if(other.collider.tag == "Ground" || other.collider.tag == "GroundDestructable" || other.collider.gameObject.layer == 13)
            {
                if(inDropdownMaxSpd <= 1 && inDropdownMaxSpd > 0.5)
                {
                    scr_camera.instance.StartShake(0.3f,0.2f,0.3f);
                    StartCoroutine(scr_player_manager.instance.cantMoveFor(0.25f));
                    StartCoroutine(scr_player_manager.instance.cantActionFor(0.25f));
                }
                else if(inDropdownMaxSpd > 1)
                {
                    scr_camera.instance.StartShake(0.5f,0.4f,0.5f);
                    StartCoroutine(scr_player_manager.instance.cantMoveFor(0.45f));
                    StartCoroutine(scr_player_manager.instance.cantActionFor(0.45f));
                }
                inDropdownMaxSpd = 0;
                scr_player_manager.instance.Phabilities.impactStage = 0;
            }
        }
        
        if(scr_player_manager.instance.Phabilities.GImpact)
        {
            if(other.collider != null)
            {
                if(other.collider.tag == "GroundDestructable")
                {
                    scr_player_manager.instance.Phabilities.destroyGround = true;
                }

                if (other.collider.tag == "Ground" && !floorHit || 
                other.collider.tag == "GroundDestructable" && !floorHit ||
                other.collider.gameObject.layer == 13 && !floorHit)
                {
                    scr_player_manager.instance.Phabilities.impactTrigger = true;
                    scr_player_manager.instance.Phabilities.GImpact = false;

                    if(scr_player_manager.instance.Phabilities.impactStage == 0)
                    {
                        scr_camera.instance.StartShake(0.3f,0.15f,2f);
                        StartCoroutine(scr_player_manager.instance.cantMoveFor(0.2f));
                        StartCoroutine(scr_player_manager.instance.cantJumpFor(0.2f));
                    }
                    else if(scr_player_manager.instance.Phabilities.impactStage == 1)
                    {
                        scr_camera.instance.StartShake(0.4f,0.22f,3f);
                        StartCoroutine(scr_player_manager.instance.cantMoveFor(0.3f));
                        StartCoroutine(scr_player_manager.instance.cantJumpFor(0.3f));
                    }
                    else if(scr_player_manager.instance.Phabilities.impactStage == 2)
                    {
                        scr_camera.instance.StartShake(0.5f,0.3f,4f);
                        StartCoroutine(scr_player_manager.instance.cantMoveFor(0.4f));
                        StartCoroutine(scr_player_manager.instance.cantJumpFor(0.4f));
                    }
                    inDropdownMaxSpd = 0;
                    scr_player_manager.instance.Phabilities.impactStage = 0;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other != null)
        {
            if(other.tag == "Water")
            {
                if(scr_player_manager.instance.Phabilities.GImpact)
                {
                    scr_player_manager.instance.Phabilities.inWaterBubble = true;
                    scr_player_manager.instance.refreshSkill = false;
                    scr_player_manager.instance.Phabilities.GImpact = false;
                }
                inDropdownMaxSpd = 0;
                scr_player_manager.instance.Phabilities.impactStage = 0;
            }
        }
    }


////////////////////////// FLIP //////////////////////////////////////////
    public void InstantFlip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        Recover.x *= -1;
        transform.localScale = Scaler;
    }

    private void LongerFlip()
    {
        scr_player_manager.instance.anim.SetBool("Turning", true);
    }
    public void EndTurning()
    {
        scr_player_manager.instance.anim.SetBool("Turning", false);
    }

    private void Recovering()
    {
        moving = false;
        isJumping = false;
        scr_player_manager.instance.rb.sharedMaterial = normal;
        scr_player_manager.instance.Pbase.Back = false;
        scr_player_manager.instance.rb.velocity = Vector2.zero;
        scr_player_manager.instance.rb.AddForce(Recover, ForceMode2D.Impulse);
    }

    public bool show = true;
    private void OnDrawGizmosSelected()
    {
        if(show)
        {
        if(feetPos1 == null)
        {
            return;
        }
        if(feetPos2 == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(feetPos1.position, checkRadius);
        Gizmos.DrawWireSphere(feetPos2.position, checkRadius);
        }
    }
}
