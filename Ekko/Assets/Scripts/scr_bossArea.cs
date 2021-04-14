using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_bossArea : MonoBehaviour
{
    private bool once = true;
    private bool activate = false;
    public bool bossDefeated = false;
    public GameObject boss;
    public GameObject Door1, Door2, Door3;
    public GameObject platform1, platform2, platform3, platform4, platform5;
    void Update()
    {
        if(!bossDefeated)
        {
            if(activate)
            {
                if(Door1 != null)
                {
                    Door1.SetActive(true);
                }
                if(Door2 != null)
                {
                    Door2.SetActive(true);
                }
                if(Door3 != null)
                {
                    Door3.SetActive(true);
                }

                boss.GetComponent<scr_IA_base>().bossActivated = true;
            }
        }
        else
        {
            if(Door1 != null)
            {
                Door1.SetActive(false);
            }
            if(Door2 != null)
            {
                Door2.SetActive(false);
            }
            if(Door3 != null)
            {
                Door3.SetActive(false);
            }

            if(platform1 != null)
            {
                platform1.SetActive(true);
            }
            if(platform2 != null)
            {
                platform2.SetActive(true);
            }
            if(platform3 != null)
            {
                platform3.SetActive(true);
            }
            if(platform4 != null)
            {
                platform4.SetActive(true);
            }
            if(platform5 != null)
            {
                platform5.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(once)
        {
            if(other.tag == "Player")
            {
                activate = true;
                once = false;
            }
        }
    }
}
