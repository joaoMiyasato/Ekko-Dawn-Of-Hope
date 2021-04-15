using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDamage : MonoBehaviour
{
    public int damage;
    public bool teleport;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerBase>().takeDamage(damage, teleport);
        }
        else if(other.gameObject.layer == 10)
        {
            other.GetComponent<EnemyBase>().takeDamage(99);
        }
    }
}
