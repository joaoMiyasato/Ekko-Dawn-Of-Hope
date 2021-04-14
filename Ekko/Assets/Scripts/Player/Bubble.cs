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
                if(!GetComponentInParent<scr_player_habilities>().onWater)
                {
                    GetComponentInParent<scr_player_habilities>().inWaterBubble = false;
                }
            } 
        }
        else if(!Guard)
        {
            if(other.collider.tag == "Ground" || other.collider.tag == "Wall" || other.collider.tag == "GroundDestructable")
            {
                if(GetComponentInParent<scr_player_habilities>().onWater)
                {
                    scr_player_manager.instance.Phabilities.cancelBubble = true;
                }
                GetComponentInParent<scr_player_habilities>().inWaterBubble = false;
            } 
        }
    }
}
