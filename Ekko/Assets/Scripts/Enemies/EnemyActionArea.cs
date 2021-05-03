using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionArea : MonoBehaviour
{
    private float time;
    private bool takeAction;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            takeAction = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(lostAction());
        }
    }

    private IEnumerator lostAction()
    {
        time = Random.Range(0.5f, 1.2f);
        yield return new WaitForSeconds(time);
        takeAction = false;
    }

    public void setTakeAction(bool takeAction)
    {
        this.takeAction = takeAction;
    }
    public bool getTakeAction()
    {
        return this.takeAction;
    }
}
