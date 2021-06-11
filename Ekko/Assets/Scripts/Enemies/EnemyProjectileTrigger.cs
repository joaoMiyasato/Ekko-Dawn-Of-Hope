using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileTrigger : MonoBehaviour
{
    public GameObject projectile;
    public void shotTrigger()
    {
        Vector3 frente = this.transform.forward;
        Vector2 dir = new Vector2(frente.x, frente.y);
        GameObject stringShot = Instantiate(projectile, transform.position, transform.rotation);

        stringShot.GetComponent<EnemyProjectileBehaviour>().direction = dir;
        stringShot.GetComponent<EnemyProjectileBehaviour>().initialPosition = stringShot.transform.position;
    }
}
