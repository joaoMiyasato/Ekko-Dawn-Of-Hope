using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_IA_damage : MonoBehaviour
{
    // public int Damage = 1;
    private int damage;
    void Start()
    {
        
    }

    void Update()
    {
        damage = this.GetComponentInParent<scr_IA_base>().Damage;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<scr_player_base>().takeDamage(damage, false);
        }
    }
}
