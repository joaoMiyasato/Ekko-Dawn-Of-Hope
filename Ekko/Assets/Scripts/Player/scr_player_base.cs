using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_player_base : MonoBehaviour
{
    private bool damageTrigger = false;
    public bool Back = false;
    private float[] savePosition;
    
    void Start()
    {
        scr_player_manager.instance.curLife = scr_player_manager.instance.maxLife;
        savePosition = new float[3];
    }

    void Update()
    {
        if(scr_player_manager.instance.maxLife <= 0)
        {
            Die();
        }
    }

    public void takeDamage(int damage, bool mapDamage)
    {
        if(scr_player_manager.instance.iFrames <= 0 && !scr_player_manager.instance.Phabilities.GImpact)
        {
            scr_camera.instance.StartShake(0.2f, 0.7f, 3f);
            scr_player_manager.instance.curLife -= damage;
            if(!mapDamage)
            {
                scr_player_manager.instance.setIframes(0.6f, false);
                Back = true;
            }
            else
            {
                StartCoroutine(waitToTeleport(0.75f));
                scr_player_manager.instance.setIframes(2f, true);
            }
            scr_player_manager.instance.cantMove = true;
            scr_player_manager.instance.cantAction = true;
        }
    }

    private void Die()
    {

    }

    private IEnumerator waitToTeleport(float Time)
    {
        scr_player_manager.instance.rb.isKinematic = true;
        scr_player_manager.instance.rb.velocity = Vector2.zero;
        UI_manager.instance.fadeTransition();

        yield return new WaitForSeconds(Time);
        scr_player_manager.instance.rb.isKinematic = false;
        this.transform.position = new Vector3(savePosition[0], savePosition[1], savePosition[2]);
    }

    public IEnumerator _damageTick(int damage, float damageTick)
    {
        if(!damageTrigger)
        {
            damageTrigger = true;
            scr_player_manager.instance.curLife -= damage;
            yield return new WaitForSeconds(damageTick);
            damageTrigger = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 99, scr_player_manager.instance.Pmove.whatIsGround);

        if(other.tag == "SavePosition")
        {
            savePosition[0] = transform.position.x;
            savePosition[1] = transform.position.y - hit.distance + 2;
            savePosition[2] = 0f;
        }
    }
}
