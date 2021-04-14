using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public BuoyancyEffector2D BE2d;
    public int damage;
    public float tickRate;
    private float T;
    private bool changeFlow = true;

    private void Start()
    {
        T = Random.Range(0.4f,1.2f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<scr_player_habilities>().onWater = true;
            if(!other.GetComponent<scr_player_habilities>().inWaterBubble && !other.GetComponent<scr_player_habilities>().floating)
            {
                StartCoroutine(other.GetComponent<scr_player_base>()._damageTick(damage,tickRate));
            }
            
            if(changeFlow)
            {
                changeFlow = false;
                BE2d.flowAngle = Random.Range(30f,-210f);
                if((int)Random.Range(1f,100f) > 50)
                {
                    BE2d.flowMagnitude = Random.Range(10f,23f);
                    BE2d.flowVariation = Random.Range(10f,23f);
                }
                StartCoroutine(Change(T));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other != null)
        {
            if(other.tag == "Player")
            {
                scr_player_manager.instance.Phabilities.onWater = false;
                scr_player_manager.instance.Phabilities.cancelBubble = false;
                scr_player_manager.instance.DragChange(1f);
                scr_player_manager.instance.GravityChange(1);
            }
        }
    }

    private IEnumerator Change(float time)
    {
        yield return new WaitForSeconds(time);
        changeFlow = true;
        T = Random.Range(0.4f,1.2f);
    }
}
