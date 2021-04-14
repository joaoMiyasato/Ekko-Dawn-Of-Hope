using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_IA_1_2 : MonoBehaviour
{
    private bool inChase = false;
    private float time = 5f;
    private float curTime;
    void Start()
    {

    }
    void Update()
    {
        if(inChase)
        {
            GetComponentInParent<scr_IA_1_1>().chasing = true;
            curTime = 0f;
        }
        else if(!inChase)
        {
            curTime += Time.deltaTime;
            if(curTime > time)
            {
                GetComponentInParent<scr_IA_1_1>().chasing = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            inChase = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            inChase = false;
        }
    }
}
