using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // public int Damage = 1;
    private int damage;
    void Start()
    {
        
    }

    void Update()
    {
        damage = this.GetComponentInParent<EnemyBase>().Damage;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerBase>().takeDamage(damage, false);
        }
    }
}
