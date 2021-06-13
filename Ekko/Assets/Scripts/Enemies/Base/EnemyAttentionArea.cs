using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttentionArea : MonoBehaviour
{
    private float time;
    private bool gotAttention, inBox;
    [SerializeField]
    private bool fast;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(!fast)
        {
            if(other.tag == "Player")
            {
                gotAttention = true;
                inBox = true;
            }
        }
        else
        {
            if(other.tag == "Player")
            {
                gotAttention = true;
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
                StartCoroutine(lostAttention());
            }
        }
        else
        {
            if(other.tag == "Player")
            {
                gotAttention = false;
            }
        }
    }
    private IEnumerator lostAttention()
    {
        time = Random.Range(0.5f, 1.2f);
        yield return new WaitForSeconds(time);
        if(!inBox)
            gotAttention = false;
    }

    public void setGotAttention(bool gotAttention)
    {
        this.gotAttention = gotAttention;
    }
    public bool getGotAttention()
    {
        return this.gotAttention;
    }
}
