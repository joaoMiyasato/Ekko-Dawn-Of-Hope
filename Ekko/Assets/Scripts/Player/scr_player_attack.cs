using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_player_attack : MonoBehaviour
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
            scr_player_manager.instance.anim.SetBool("IsAttacking", true);
            if(!scr_player_manager.instance.Pmove.isGrounded)
            {
                scr_player_manager.instance.anim.ResetTrigger("Jump");
            }
        }
        else
        {
            attacking = false;
            scr_player_manager.instance.anim.SetBool("IsAttacking", false);
        }

        curInterval += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.X) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            if(!scr_player_manager.instance.cantAction)
            {
                if(curInterval >= attackInterval)
                {
                    atkH = true;
                    AttackHor();
                    if(scr_player_manager.instance.Pmove.facingRight)
                    {
                        hor = -1;
                    }
                    else if(!scr_player_manager.instance.Pmove.facingRight)
                    {
                        hor = 1;
                    }
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.UpArrow))
        {
            if(!scr_player_manager.instance.cantAction)
            {
                if(curInterval >= attackInterval)
                {
                    atkH = false;
                    AttackUp();

                    ver = scr_player_manager.instance.rb.velocity.y;
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.DownArrow) && !scr_player_manager.instance.Pmove.isGrounded)
        {
            if(!scr_player_manager.instance.cantAction)
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
            recoil.y = scr_player_manager.instance.rb.velocity.y;
            recoil.x = hor * recoilForce.x;
        }
        else
        {
            recoil.y = ver * recoilForce.y;
            recoil.x = scr_player_manager.instance.rb.velocity.x;
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
            scr_player_manager.instance.anim.SetTrigger("AttackH0");
        }
        else
        {
            animTrigger = false;
            animTime = 0;
            scr_player_manager.instance.anim.SetTrigger("AttackH1");
        }
        curInterval = 0;
        // Collider2D[] hitEnemies1 = Physics2D.OverlapCircleAll(HPoint.position, attackRangeH, enemyLayers);
        Collider2D[] hitEnemies1 = Physics2D.OverlapBoxAll(HPoint.position, new Vector2(attackRangeH, attackRangeH2), 0,enemyLayers);
        foreach(Collider2D hit in hitEnemies1)
        {
            hitted = true;
            scr_player_manager.instance.rb.velocity = new Vector2(0,scr_player_manager.instance.rb.velocity.y);
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Boss")
            {
                scr_player_manager.instance.curEnergy += 30;
                scr_player_manager.instance.PowerPoints += 10;
                hit.GetComponent<scr_IA_base>().takeDamage(atkDamage);
            }
            else if(hit.gameObject.layer == 13)
            {
                hit.GetComponent<DestroyObject>().Interact();
            }
        }
    }

    void AttackUp()
    {
        scr_player_manager.instance.anim.SetTrigger("AttackVU");
        curInterval = 0;
        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(VPointU.position, attackRangeU, enemyLayers);
        foreach(Collider2D hit in hitEnemies2)
        {
            hitted = true;
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Boss")
            {
                scr_player_manager.instance.curEnergy += 30;
                scr_player_manager.instance.PowerPoints += 10;
                hit.GetComponent<scr_IA_base>().takeDamage(atkDamage);
            }
            else if(hit.gameObject.layer == 13)
            {
                hit.GetComponent<DestroyObject>().Interact();
            }
        }
    }

    void AttackDown()
    {
        scr_player_manager.instance.anim.SetTrigger("AttackVD");
        curInterval = 0;
        Collider2D[] hitEnemies3 = Physics2D.OverlapCircleAll(VPointD.position, attackRangeD, enemyLayers);
        foreach(Collider2D hit in hitEnemies3)
        {
            if(hit.gameObject.tag != "Wall" && hit.gameObject.tag != "InteractableWall" && hit.gameObject.layer != 8)
            {
                hitted = true;
                scr_player_manager.instance.rb.velocity = new Vector2(scr_player_manager.instance.rb.velocity.x, 0);
            }
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Boss")
            {
                scr_player_manager.instance.curEnergy += 30;
                scr_player_manager.instance.PowerPoints += 10;
                hit.GetComponent<scr_IA_base>().takeDamage(atkDamage);
            }
            else if(hit.gameObject.layer == 13)
            {
                hit.GetComponent<DestroyObject>().Interact();
            }
        }
    }

    private void knockBack()
    {
        StartCoroutine(scr_player_manager.instance.cantActionFor(0.2f));
        StartCoroutine(scr_player_manager.instance.cantMoveFor(0.2f));
        a+= Time.deltaTime;
        if(a > b)
        {
            a = 0;
            hitted = false;
        }
        scr_player_manager.instance.rb.velocity = recoil;
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
