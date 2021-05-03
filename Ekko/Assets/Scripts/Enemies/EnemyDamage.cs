using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerBase>().takeDamage(transform.parent.gameObject.GetComponent<EnemyBase>().getDamage(), false);
        }
    }
}
