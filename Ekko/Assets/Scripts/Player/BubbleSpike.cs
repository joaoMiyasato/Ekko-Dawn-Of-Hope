using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<scr_IA_base>().takeDamage(scr_player_manager.instance.Pattack.atkDamage);
        }
        if(other.tag == "GroundDestructable")
        {
            other.GetComponent<DestroyObject>().Interact();
        }
    }
}
