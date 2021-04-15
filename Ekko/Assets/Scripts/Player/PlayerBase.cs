using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private bool damageTrigger = false;
    public bool Back = false;
    private float[] savePosition;
    
    void Start()
    {
        PlayerManager.instance.curLife = PlayerManager.instance.maxLife;
        savePosition = new float[3];
    }

    void Update()
    {
        if(PlayerManager.instance.maxLife <= 0)
        {
            Die();
        }
    }

    public void takeDamage(int damage, bool mapDamage)
    {
        if(PlayerManager.instance.iFrames <= 0 && !PlayerManager.instance.playerHabilities.GImpact)
        {
            CameraControl.instance.StartShake(0.2f, 0.7f, 3f);
            PlayerManager.instance.curLife -= damage;
            if(!mapDamage)
            {
                PlayerManager.instance.setIframes(0.6f, false);
                Back = true;
            }
            else
            {
                StartCoroutine(waitToTeleport(0.75f));
                PlayerManager.instance.setIframes(2f, true);
            }
            PlayerManager.instance.cantMove = true;
            PlayerManager.instance.cantAction = true;
        }
    }

    private void Die()
    {

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

    public IEnumerator _damageTick(int damage, float damageTick)
    {
        if(!damageTrigger)
        {
            damageTrigger = true;
            PlayerManager.instance.curLife -= damage;
            yield return new WaitForSeconds(damageTick);
            damageTrigger = false;
        }

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
}
