using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionArea : MonoBehaviour
{
    private float time;
    private bool takeAction, inBox;
    [SerializeField]
    private bool fast;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(!fast)
        {
            if(other.tag == "Player")
            {
                takeAction = true;
                inBox = true;
            }
        }
        else
        {
            if(other.tag == "Player")
            {
                takeAction = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!fast)
        {
            if(other.tag == "Player")
            {
                inBox = false;
                StartCoroutine(lostAction());
            }
        }
        else
        {
            if(other.tag == "Player")
            {
                takeAction = false;
            }
        }
    }
    private IEnumerator lostAction()
    {
        time = Random.Range(0.5f, 1.2f);
        yield return new WaitForSeconds(time);
        if(!inBox)
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
