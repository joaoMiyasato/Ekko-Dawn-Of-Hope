using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileTrigger : MonoBehaviour
{
    public GameObject projectile;
    public void shotTrigger()
    {
        Vector3 frente = this.transform.forward;
        Vector2 dir = new Vector2(frente.x, frente.y + 0.1f);
        GameObject stringShot = Instantiate(projectile, transform.position, projectile.transform.rotation);

        stringShot.GetComponent<EnemyProjectileBehaviour>().direction = dir;
        stringShot.GetComponent<EnemyProjectileBehaviour>().initialPosition = stringShot.transform.position;
    }
}
