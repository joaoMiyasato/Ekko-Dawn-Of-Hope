using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
                    PlayerManager.instance.animator.SetBool("LookingUp", true);
                }
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                lookDown += Time.deltaTime;
                if(lookDown > 1.5f)
                {
                    PlayerManager.instance.animator.SetBool("LookingDown", true);
                }
            }
            else
            {
                lookDown = 0;
                lookUp = 0;
                PlayerManager.instance.animator.SetBool("LookingUp", false);
                PlayerManager.instance.animator.SetBool("LookingDown", false);
            }
        }
#endregion

#region Fall
        if(PlayerManager.instance.rb.velocity.y < 0)
        {
            if(!PlayerManager.instance.playerAttack.attacking)
            {
                PlayerManager.instance.animator.SetBool("Falling", true);
            }
            else PlayerManager.instance.animator.SetBool("Falling", false);
        }
        else PlayerManager.instance.animator.SetBool("Falling", false);
#endregion

#region DropDown
        if(PlayerManager.instance.rb.velocity.y <= maxDropdownSpd)
        {
            if(!PlayerManager.instance.playerHabilities.GImpact)
            {
                PlayerManager.instance.rb.velocity = new Vector2(PlayerManager.instance.rb.velocity.x, maxDropdownSpd);
            }
            inDropdownMaxSpd += Time.deltaTime; 
            if(inDropdownMaxSpd >= 0.59)
            {
                PlayerManager.instance.playerHabilities.impactStage = 2;
            }
            else if(inDropdownMaxSpd >= 0.17 && inDropdownMaxSpd < 0.59)
            {
            PlayerManager.instance.playerHabilities.impactStage = 1;
            }
            else
            {
                PlayerManager.instance.playerHabilities.impactStage = 0;
            }
        }
        if(isJumping)
        {
            inDropdownMaxSpd = 0;
            PlayerManager.instance.playerHabilities.impactStage = 0;
        }
#endregion

        if(!PlayerManager.instance.playerHabilities.inWaterBubble)
        {
            if(!facingRight && MoveInput > 0 && !PlayerManager.instance.playerHabilities.sliding || facingRight && MoveInput > 0 && PlayerManager.instance.playerHabilities.sliding)
            {
                if(!PlayerManager.instance.playerAttack.attacking)
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
            else if(facingRight && MoveInput < 0 && !PlayerManager.instance.playerHabilities.sliding || !facingRight && MoveInput < 0 && PlayerManager.instance.playerHabilities.sliding)
            {
                if(!PlayerManager.instance.playerAttack.attacking)
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
            if(!PlayerManager.instance.cantJump && 
            !PlayerManager.instance.playerHabilities.healing &&
            !PlayerManager.instance.playerHabilities.sliding &&
            !PlayerManager.instance.playerHabilities.GImpact)
            {
                jump();
            }

            if(PlayerManager.instance.playerHabilities.wallJumping && PlayerManager.instance.Skill_DoubleJump)
            {
                jumpCount = 1;
            }
#endregion

            if(PlayerManager.instance.playerBase.Back)
            {
                Recovering();
            }

#region MaterialSwitch
            if(moving || isJumping)
            {
                PlayerManager.instance.rb.sharedMaterial = slippery;
            }
            else if(!moving)
            {
                PlayerManager.instance.rb.sharedMaterial = normal;
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
            PlayerManager.instance.animator.SetBool("IsGrounded", true);
        }
        else
        {
            isGrounded = false;
            PlayerManager.instance.animator.SetBool("IsGrounded", false);
            PlayerManager.instance.animator.SetBool("Turning", false);
        } 
#endregion

#region Move
        if(!PlayerManager.instance.cantMove && 
        !PlayerManager.instance.playerHabilities.healing &&
        !PlayerManager.instance.playerHabilities.inWaterBubble &&
        !PlayerManager.instance.playerHabilities.GImpact)
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

        if(!PlayerManager.instance.playerHabilities.wallJumping)
            PlayerManager.instance.rb.velocity = new Vector2(MoveInput * curSpeed, PlayerManager.instance.rb.velocity.y);
        
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            moving = true;
            PlayerManager.instance.animator.SetBool("Walking", true);
        }
        else
        {
            moving = false;
            PlayerManager.instance.animator.SetBool("Walking", false);
        }
    }
////////////////////////// JUMP //////////////////////////////////////////
    private void jump()
    {
        if(jumpCount > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            PlayerManager.instance.animator.SetTrigger("Jump");
            jumpCount--;
            if(!GameManager.instance.isPaused)
                isJumping = true;
            jumpTimeCounter = jumpTime;
            PlayerManager.instance.rb.velocity = Vector2.up ;
        }

        if(isJumping && Input.GetKey(KeyCode.Space))
        {
            if(jumpTimeCounter > 0)
            {
                PlayerManager.instance.rb.velocity = Vector2.up * jumpForce*3;
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
                PlayerManager.instance.rb.velocity = PlayerManager.instance.rb.velocity / 2;
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

        if(PlayerManager.instance.playerHabilities.floating)
        {
            if(!PlayerManager.instance.Skill_DoubleJump)
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
                if(!PlayerManager.instance.Skill_DoubleJump)
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
        if(inDropdownMaxSpd > 0 && !PlayerManager.instance.playerHabilities.GImpact)
        {
            if(other.collider.tag == "Ground" || other.collider.tag == "GroundDestructable" || other.collider.gameObject.layer == 13)
            {
                if(inDropdownMaxSpd <= 1 && inDropdownMaxSpd > 0.5)
                {
                    CameraControl.instance.StartShake(0.3f,0.2f,0.3f);
                    StartCoroutine(PlayerManager.instance.cantMoveFor(0.25f));
                    StartCoroutine(PlayerManager.instance.cantActionFor(0.25f));
                }
                else if(inDropdownMaxSpd > 1)
                {
                    CameraControl.instance.StartShake(0.5f,0.4f,0.5f);
                    StartCoroutine(PlayerManager.instance.cantMoveFor(0.45f));
                    StartCoroutine(PlayerManager.instance.cantActionFor(0.45f));
                }
                inDropdownMaxSpd = 0;
                PlayerManager.instance.playerHabilities.impactStage = 0;
            }
        }
        
        if(PlayerManager.instance.playerHabilities.GImpact)
        {
            if(other.collider != null)
            {
                if(other.collider.tag == "GroundDestructable")
                {
                    PlayerManager.instance.playerHabilities.destroyGround = true;
                }

                if (other.collider.tag == "Ground" && !floorHit || 
                other.collider.tag == "GroundDestructable" && !floorHit ||
                other.collider.gameObject.layer == 13 && !floorHit)
                {
                    PlayerManager.instance.playerHabilities.impactTrigger = true;
                    PlayerManager.instance.playerHabilities.GImpact = false;

                    if(PlayerManager.instance.playerHabilities.impactStage == 0)
                    {
                        CameraControl.instance.StartShake(0.3f,0.15f,2f);
                        StartCoroutine(PlayerManager.instance.cantMoveFor(0.2f));
                        StartCoroutine(PlayerManager.instance.cantJumpFor(0.2f));
                    }
                    else if(PlayerManager.instance.playerHabilities.impactStage == 1)
                    {
                        CameraControl.instance.StartShake(0.4f,0.22f,3f);
                        StartCoroutine(PlayerManager.instance.cantMoveFor(0.3f));
                        StartCoroutine(PlayerManager.instance.cantJumpFor(0.3f));
                    }
                    else if(PlayerManager.instance.playerHabilities.impactStage == 2)
                    {
                        CameraControl.instance.StartShake(0.5f,0.3f,4f);
                        StartCoroutine(PlayerManager.instance.cantMoveFor(0.4f));
                        StartCoroutine(PlayerManager.instance.cantJumpFor(0.4f));
                    }
                    inDropdownMaxSpd = 0;
                    PlayerManager.instance.playerHabilities.impactStage = 0;
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
                if(PlayerManager.instance.playerHabilities.GImpact)
                {
                    PlayerManager.instance.playerHabilities.inWaterBubble = true;
                    PlayerManager.instance.refreshSkill = false;
                    PlayerManager.instance.playerHabilities.GImpact = false;
                }
                inDropdownMaxSpd = 0;
                PlayerManager.instance.playerHabilities.impactStage = 0;
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
        PlayerManager.instance.animator.SetBool("Turning", true);
    }
    public void EndTurning()
    {
        PlayerManager.instance.animator.SetBool("Turning", false);
    }

    private void Recovering()
    {
        moving = false;
        isJumping = false;
        PlayerManager.instance.rb.sharedMaterial = normal;
        PlayerManager.instance.playerBase.Back = false;
        PlayerManager.instance.rb.velocity = Vector2.zero;
        PlayerManager.instance.rb.AddForce(Recover, ForceMode2D.Impulse);
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
