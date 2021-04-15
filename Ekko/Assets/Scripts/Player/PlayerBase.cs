using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private int maxLife = 500, curLife;
    private int maxEnergy = 300, curEnergy;
    private int powerPoints = 10000;
    private int energyStones = 0;
    private bool damageTrigger = false;
    public bool knockback = false;
    private float[] savePosition;
    
    private float iFrames; private bool mapDamage;
    private bool cantMove, cantAction, cantJump, refreshSkill;
    
    void Start()
    {
        curLife = maxLife;
        savePosition = new float[3];
    }

    void Update()
    {
        if(maxLife <= 0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if(curEnergy > maxEnergy)
        {
            curEnergy = maxEnergy;
        }
        if(iFrames > 0)
        {
            iFrames -= Time.fixedDeltaTime;
        }
    }

#region GET REGION
    public int getMaxLife()
    {
        return this.maxLife;
    }
    public int getCurrentLife()
    {
        return this.curLife;
    }
    public int getMaxEnergy()
    {
        return this.maxEnergy;
    }
    public int getCurrentEnergy()
    {
        return this.curEnergy;
    }
    public int getPowerPoints()
    {
        return this.powerPoints;
    }
    public int getEnergyStones()
    {
        return this.energyStones;
    }
    public bool getCantMove()
    {
        return this.cantMove;
    }
    public bool getCantAction()
    {
        return this.cantAction;
    }
    public bool getCantJump()
    {
        return this.cantJump;
    }
    public bool getRefreshSkill()
    {
        return this.refreshSkill;
    }

#endregion

#region SET REGION
    public void setMaxLife(int maxLife)
    {
        this.maxLife = maxLife;
    }
    public void setCurLife(int curLife)
    {
        this.curLife = curLife;
    }
    public void setMaxEnergy(int maxEnergy)
    {
        this.maxEnergy = maxEnergy;
    }
    public void setCurEnergy(int curEnergy)
    {
        this.curEnergy = curEnergy;
    }
    public void setPowerPoints(int powerPoints)
    {
        this.powerPoints = powerPoints;
    }
    public void setEnergyStones(int energyStones)
    {
        this.energyStones = energyStones;
    }
    public void setIframes(float howMuch, bool _mapDamage)
    {
        iFrames = howMuch;
        mapDamage = _mapDamage;
        if(mapDamage)
        {
            StartCoroutine(cantActionFor(iFrames*2/3));
            StartCoroutine(cantJumpFor(iFrames*2/3));
            StartCoroutine(cantMoveFor(iFrames*2/3));
        }
        else
        {
            StartCoroutine(cantActionFor(iFrames/2));
            StartCoroutine(cantJumpFor(iFrames/2));
            StartCoroutine(cantMoveFor(iFrames/2));
        }
    }
    public void setRefreshSkill(bool refreshSkill)
    {
        this.refreshSkill = refreshSkill;
    }
    public void setCantMove(bool cantMove)
    {
        this.cantMove = cantMove;
    }
    public void setCantAction(bool cantAction)
    {
        this.cantAction = cantAction;
    }
#endregion

#region DAMAGE REGION
    public void takeDamage(int damage, bool mapDamage)
    {
        if(iFrames <= 0 && !PlayerManager.instance.playerHabilities.GImpact)
        {
            CameraControl.instance.StartShake(0.2f, 0.7f, 3f);
            curLife -= damage;
            if(!mapDamage)
            {
                setIframes(0.6f, false);
                knockback = true;
            }
            else
            {
                StartCoroutine(waitToTeleport(0.75f));
                setIframes(2f, true);
            }
            cantMove = true;
            cantAction = true;
        }
    }
    public IEnumerator _damageTick(int damage, float damageTick)
    {
        if(!damageTrigger)
        {
            damageTrigger = true;
            curLife -= damage;
            yield return new WaitForSeconds(damageTick);
            damageTrigger = false;
        }

    }

#endregion

    public void addPowerPoints(int add)
    {
        this.powerPoints += add;
    }
    public void addEnergyStones(int add)
    {
        this.energyStones += add;
    }
    public void addMaxLife(int add)
    {
        this.maxLife += add;
    }
    public void addLife(int add)
    {
        this.curLife += add;
    }
    public void addMaxEnergy(int add)
    {
        this.maxEnergy += add;
    }
    public void addEnergy(int add)
    {
        this.curEnergy += add;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 99, PlayerManager.instance.playerMovement.whatIsGround);

        if(other.tag == "SavePosition")
        {
            savePosition[0] = transform.position.x;
            savePosition[1] = transform.position.y - hit.distance + 2;
            savePosition[2] = 0f;
        }
    }
    
    public IEnumerator cantMoveFor(float T)
    {
        cantMove = true;
        PlayerManager.instance.rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(T);
        cantMove = false;
    }
    public IEnumerator cantJumpFor(float T)
    {
        cantJump = true;
        yield return new WaitForSeconds(T);
        cantJump = false;
    }
    public IEnumerator cantActionFor(float T)
    {
        cantAction = true;
        yield return new WaitForSeconds(T);
        cantAction = false;
    }
    public IEnumerator refreshTime(float T)
    {
        refreshSkill = true;
        yield return new WaitForSeconds(T);
        refreshSkill = false;
    }
    private IEnumerator waitToTeleport(float Time)
    {
        PlayerManager.instance.rb.isKinematic = true;
        PlayerManager.instance.rb.velocity = Vector2.zero;
        UI_manager.instance.fadeTransition();

        yield return new WaitForSeconds(Time);
        PlayerManager.instance.rb.isKinematic = false;
        this.transform.position = new Vector3(savePosition[0], savePosition[1], savePosition[2]);
    }

    private void Die()
    {

    }

}
