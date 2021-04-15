using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public bool Guard;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(Guard)
        {
            if(other.collider.tag == "Ground" || other.collider.tag == "Wall" || other.collider.tag == "GroundDestructable")
            {
                if(!GetComponentInParent<PlayerHabilities>().onWater)
                {
                    GetComponentInParent<PlayerHabilities>().inWaterBubble = false;
                }
            } 
        }
        else if(!Guard)
        {
            if(other.collider.tag == "Ground" || other.collider.tag == "Wall" || other.collider.tag == "GroundDestructable")
            {
                if(GetComponentInParent<PlayerHabilities>().onWater)
                {
                    PlayerManager.instance.playerHabilities.cancelBubble = true;
                }
                GetComponentInParent<PlayerHabilities>().inWaterBubble = false;
            } 
        }
    }
}
