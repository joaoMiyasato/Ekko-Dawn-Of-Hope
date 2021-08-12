using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffensiveAttack : MonoBehaviour
{
    public WeaponObject testObject;
    public LayerMask enemyLayers;
    private float curInterval;
    private bool hitted;
    public Vector2 recoilForce;
    private Vector2 recoil;
    private float horizontalRecoil,verticalRecoil;
    private float a,b = 0.01f;
    private bool atkH = false;

    private bool animTrigger;
    private float animTime;
    public bool attacking;

    private void Start()
    {
        curInterval = 3f;
    }
    private void Update()
    {

// ARRUMAR INTERVALO
#region AttackInteval/AnimationControl
        if(curInterval < 0.25 /*   AQUI  */)
        {
            attacking = true;
            PlayerManager.instance.animator.SetBool("IsAttacking", true);
            PlayerManager.instance.animator.SetBool("Turning", false);
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

        if(curInterval < 3f) curInterval += Time.deltaTime;
#endregion


        //ARRUMAR TODA ESSA PARTE MAIS TARDE PARA INSTANCIAR DANO NA ANIMAÇÃO
        if(Input.GetKeyDown(KeyCode.X) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            if(!PlayerManager.instance.playerBase.getCantAction())
            {
                if(curInterval >= PlayerManager.instance.getOffEquipedWeapon().weaponAttackRate)
                {
                    atkH = true;
                    AttackHor();
                    if(PlayerManager.instance.playerMovement.facingRight)
                    {
                        horizontalRecoil = -1;
                    }
                    else if(!PlayerManager.instance.playerMovement.facingRight)
                    {
                        horizontalRecoil = 1;
                    }
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.UpArrow))
        {
            if(!PlayerManager.instance.playerBase.getCantAction())
            {
                if(curInterval >= PlayerManager.instance.getOffEquipedWeapon().weaponAttackRate)
                {
                    atkH = false;
                    AttackUp();

                    verticalRecoil = PlayerManager.instance.rb.velocity.y;
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.DownArrow) && !PlayerManager.instance.playerMovement.isGrounded)
        {
            if(!PlayerManager.instance.playerBase.getCantAction())
            {
                if(curInterval >= PlayerManager.instance.getOffEquipedWeapon().weaponAttackRate)
                {
                    atkH = false;
                    AttackDown();

                    verticalRecoil = 1;
                }
            }
        }

        //RECOIL HORIZONTAL E VERTICAL
        if(atkH)
        {
            recoil.y = PlayerManager.instance.rb.velocity.y;
            recoil.x = horizontalRecoil * recoilForce.x;
        }
        else
        {
            recoil.y = horizontalRecoil * recoilForce.y;
            recoil.x = PlayerManager.instance.rb.velocity.x;
        }

        if(hitted)
        {
            knockBack();
        }

        //AJEITAR MAIS TARDE
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


        Vector2 pos = PlayerManager.instance.getOffEquipedWeapon().weaponHorizontalPosition;
        if(PlayerManager.instance.playerMovement.facingRight)
        {
            pos.x = Mathf.Abs(pos.x);
        }
        else
        {
            pos.x = -Mathf.Abs(pos.x);
        }
        Collider2D[] hittedEnemies = Physics2D.OverlapBoxAll( new Vector2(gameObject.transform.position.x+pos.x, gameObject.transform.position.y+pos.y), 
                                                            new Vector2(PlayerManager.instance.getOffEquipedWeapon().weaponRangeHorizontal0, PlayerManager.instance.getOffEquipedWeapon().weaponRangeHorizontal1), 
                                                            0, 
                                                            enemyLayers);
        
        foreach(Collider2D hit in hittedEnemies)
        {
            hitted = true;
            PlayerManager.instance.rb.velocity = new Vector2(0,PlayerManager.instance.rb.velocity.y);
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Boss")
            {
                PlayerManager.instance.playerBase.addEnergy(30);
                hit.GetComponent<EnemyBase>().takeDamage(PlayerManager.instance.getOffEquipedWeapon().weaponDamage);
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


        Collider2D[] hittedEnemies = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x+PlayerManager.instance.getOffEquipedWeapon().weaponUpPosition.x, gameObject.transform.position.y+PlayerManager.instance.getOffEquipedWeapon().weaponUpPosition.y), 
                                                                new Vector2(PlayerManager.instance.getOffEquipedWeapon().weaponRangeUp0, PlayerManager.instance.getOffEquipedWeapon().weaponRangeUp1), 
                                                                0,
                                                                enemyLayers);
        foreach(Collider2D hit in hittedEnemies)
        {
            hitted = true;
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Boss")
            {
                PlayerManager.instance.playerBase.addEnergy(30);
                hit.GetComponent<EnemyBase>().takeDamage(PlayerManager.instance.getOffEquipedWeapon().weaponDamage);
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

        
        Collider2D[] hittedEnemies = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x+PlayerManager.instance.getOffEquipedWeapon().weaponDownPosition.x, gameObject.transform.position.y+PlayerManager.instance.getOffEquipedWeapon().weaponDownPosition.y), 
                                                                new Vector2(PlayerManager.instance.getOffEquipedWeapon().weaponRangeDown0, PlayerManager.instance.getOffEquipedWeapon().weaponRangeDown1), 
                                                                0,
                                                                enemyLayers);
        foreach(Collider2D hit in hittedEnemies)
        {
            if(hit.gameObject.tag != "Wall" && hit.gameObject.tag != "InteractableWall" && hit.gameObject.layer != 8)
            {
                hitted = true;
                PlayerManager.instance.rb.velocity = new Vector2(PlayerManager.instance.rb.velocity.x, 0);
            }
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Boss")
            {
                PlayerManager.instance.playerBase.addEnergy(30);
                hit.GetComponent<EnemyBase>().takeDamage(PlayerManager.instance.getOffEquipedWeapon().weaponDamage);
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
            Vector2 pos = testObject.weaponHorizontalPosition;
            // Gizmos.DrawWireCube(HPoint.position, new Vector2(attackRangeH, attackRangeH2));
            Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x+pos.x, gameObject.transform.position.y+pos.y), 
                                new Vector2(testObject.weaponRangeHorizontal0, testObject.weaponRangeHorizontal1));
                                
            Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x-pos.x, gameObject.transform.position.y+pos.y), 
                                new Vector2(testObject.weaponRangeHorizontal0, testObject.weaponRangeHorizontal1));

            // Gizmos.DrawWireSphere(VPointU.position, attackRangeU);
            Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x+testObject.weaponUpPosition.x, gameObject.transform.position.y+testObject.weaponUpPosition.y), 
                                new Vector2(testObject.weaponRangeUp0, testObject.weaponRangeUp1));

            // Gizmos.DrawWireSphere(VPointD.position, attackRangeD);
            Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x+testObject.weaponDownPosition.x, gameObject.transform.position.y+testObject.weaponDownPosition.y), 
                                new Vector2(testObject.weaponRangeDown0, testObject.weaponRangeDown1));
        }
    }
}
