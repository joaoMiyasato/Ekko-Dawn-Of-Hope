using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform HPoint, VPointU, VPointD;
    public LayerMask enemyLayers;
    public float attackRangeH2, attackRangeH = 1.8f, attackRangeU = 1.5f, attackRangeD = 1.5f;
    public int atkDamage = 50;
    [Range(0,1)] public float attackInterval = 0.25f;
    private float curInterval;
    private bool hitted;
    public Vector2 recoilForce;
    private Vector2 recoil;
    private float hor,ver;
    private float a,b = 0.01f;
    private bool atkH = false;

    private bool animTrigger;
    private float animTime;
    public bool attacking;
    private void Start()
    {
        curInterval = attackInterval;
    }
    private void Update()
    {
        if(curInterval < attackInterval)
        {
            attacking = true;
            PlayerManager.instance.animator.SetBool("IsAttacking", true);
            if(!PlayerManager.instance.playerMovement.isGrounded)
            {
                PlayerManager.instance.animator.ResetTrigger("Jump");
            }
        }
        else
        {
            attacking = false;
            PlayerManager.instance.animator.SetBool("IsAttacking", false);
        }

        curInterval += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.X) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            if(!PlayerManager.instance.playerBase.getCantAction())
            {
                if(curInterval >= attackInterval)
                {
                    atkH = true;
                    AttackHor();
                    if(PlayerManager.instance.playerMovement.facingRight)
                    {
                        hor = -1;
                    }
                    else if(!PlayerManager.instance.playerMovement.facingRight)
                    {
                        hor = 1;
                    }
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.UpArrow))
        {
            if(!PlayerManager.instance.playerBase.getCantAction())
            {
                if(curInterval >= attackInterval)
                {
                    atkH = false;
                    AttackUp();

                    ver = PlayerManager.instance.rb.velocity.y;
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.DownArrow) && !PlayerManager.instance.playerMovement.isGrounded)
        {
            if(!PlayerManager.instance.playerBase.getCantAction())
            {
                if(curInterval >= attackInterval)
                {
                    atkH = false;
                    AttackDown();

                    ver = 1;
                }
            }
        }

        if(atkH)
        {
            recoil.y = PlayerManager.instance.rb.velocity.y;
            recoil.x = hor * recoilForce.x;
        }
        else
        {
            recoil.y = ver * recoilForce.y;
            recoil.x = PlayerManager.instance.rb.velocity.x;
        }

        if(hitted)
        {
            knockBack();
        }

        if(animTrigger)
        {
            animTime += Time.deltaTime;
            if(animTime > 1.2f)
            {
                animTime = 0;
                animTrigger = false;
            }
        }
    }
    void AttackHor()
    {
        if(!animTrigger)
        {
            animTrigger = true;
            PlayerManager.instance.animator.SetTrigger("AttackH0");
        }
        else
        {
            animTrigger = false;
            animTime = 0;
            PlayerManager.instance.animator.SetTrigger("AttackH1");
        }
        curInterval = 0;
        // Collider2D[] hitEnemies1 = Physics2D.OverlapCircleAll(HPoint.position, attackRangeH, enemyLayers);
        Collider2D[] hitEnemies1 = Physics2D.OverlapBoxAll(HPoint.position, new Vector2(attackRangeH, attackRangeH2), 0,enemyLayers);
        foreach(Collider2D hit in hitEnemies1)
        {
            hitted = true;
            PlayerManager.instance.rb.velocity = new Vector2(0,PlayerManager.instance.rb.velocity.y);
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Boss")
            {
                PlayerManager.instance.playerBase.addEnergy(30);
                hit.GetComponent<EnemyBase>().takeDamage(atkDamage);
            }
            else if(hit.gameObject.layer == 13)
            {
                hit.GetComponent<DestroyObject>().Interact();
            }
        }
    }

    void AttackUp()
    {
        PlayerManager.instance.animator.SetTrigger("AttackVU");
        curInterval = 0;
        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(VPointU.position, attackRangeU, enemyLayers);
        foreach(Collider2D hit in hitEnemies2)
        {
            hitted = true;
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Boss")
            {
                PlayerManager.instance.playerBase.addEnergy(30);
                hit.GetComponent<EnemyBase>().takeDamage(atkDamage);
            }
            else if(hit.gameObject.layer == 13)
            {
                hit.GetComponent<DestroyObject>().Interact();
            }
        }
    }

    void AttackDown()
    {
        PlayerManager.instance.animator.SetTrigger("AttackVD");
        curInterval = 0;
        Collider2D[] hitEnemies3 = Physics2D.OverlapCircleAll(VPointD.position, attackRangeD, enemyLayers);
        foreach(Collider2D hit in hitEnemies3)
        {
            if(hit.gameObject.tag != "Wall" && hit.gameObject.tag != "InteractableWall" && hit.gameObject.layer != 8)
            {
                hitted = true;
                PlayerManager.instance.rb.velocity = new Vector2(PlayerManager.instance.rb.velocity.x, 0);
            }
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Boss")
            {
                PlayerManager.instance.playerBase.addEnergy(30);
                hit.GetComponent<EnemyBase>().takeDamage(atkDamage);
            }
            else if(hit.gameObject.layer == 13)
            {
                hit.GetComponent<DestroyObject>().Interact();
            }
        }
    }

    private void knockBack()
    {
        StartCoroutine(PlayerManager.instance.playerBase.cantActionFor(0.2f));
        StartCoroutine(PlayerManager.instance.playerBase.cantMoveFor(0.2f));
        a+= Time.deltaTime;
        if(a > b)
        {
            a = 0;
            hitted = false;
        }
        PlayerManager.instance.rb.velocity = recoil;
    }

    public bool show = true;
    private void OnDrawGizmosSelected()
    {

    }
    private void OnDrawGizmos()
    {
        if(show)
        {
        if(HPoint == null)
        {
            return;
        }
        // Gizmos.DrawWireSphere(HPoint.position, attackRangeH);
        Gizmos.DrawWireCube(HPoint.position,new Vector2(attackRangeH, attackRangeH2));

        if(VPointU == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(VPointU.position, attackRangeU);

        if(VPointD == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(VPointD.position, attackRangeD);
        }
    }
}
