using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other != null)
        {
            if(other.tag == "Player")
            {
                if(!other.GetComponent<PlayerHabilities>().inWaterBubble)
                {
                    other.GetComponent<PlayerHabilities>().floating = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other != null)
        {
            if(other.tag == "Player")
            {
                other.GetComponent<PlayerHabilities>().floating = false;
            }
        }
    }
}
