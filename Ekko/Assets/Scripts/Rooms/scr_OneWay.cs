using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_OneWay : MonoBehaviour
{
    private float timer = 0.3f;
    private float curTimer;
    private bool desativado = false;
    void Update()
    {
        if(Input.GetKey(KeyCode.DownArrow))
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                desativado = true;
            }
        }
        if(desativado)
        {
            this.GetComponent<PlatformEffector2D>().surfaceArc = 0;
            curTimer += Time.deltaTime;
            if(curTimer > timer)
            {
                desativado = false;
                curTimer = 0;
            }
        }
        else
        {
            this.GetComponent<PlatformEffector2D>().surfaceArc = 178;
        }
    }
}
