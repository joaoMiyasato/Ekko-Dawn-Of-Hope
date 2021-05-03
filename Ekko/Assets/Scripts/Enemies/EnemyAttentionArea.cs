using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttentionArea : MonoBehaviour
{
    private float time;
    private bool gotAttention, inBox;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            gotAttention = true;
            inBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            inBox = false;
            StartCoroutine(lostAttention());
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
