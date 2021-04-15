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
            other.GetComponent<PlayerHabilities>().onWater = true;
            if(!other.GetComponent<PlayerHabilities>().inWaterBubble && !other.GetComponent<PlayerHabilities>().floating)
            {
                StartCoroutine(other.GetComponent<PlayerBase>()._damageTick(damage,tickRate));
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
                PlayerManager.instance.playerHabilities.onWater = false;
                PlayerManager.instance.playerHabilities.cancelBubble = false;
                PlayerManager.instance.DragChange(1f);
                PlayerManager.instance.GravityChange(1);
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
