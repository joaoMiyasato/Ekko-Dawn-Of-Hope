using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyBase>().takeDamage(PlayerManager.instance.playerAttack.atkDamage);
        }
        if(other.tag == "GroundDestructable")
        {
            other.GetComponent<DestroyObject>().Interact();
        }
    }
}
