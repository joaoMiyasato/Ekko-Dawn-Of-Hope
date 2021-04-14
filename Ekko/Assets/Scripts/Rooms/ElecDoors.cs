using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecDoors : MonoBehaviour
{
    public bool alreadyOpen;
    private void Start()
    {
        if(alreadyOpen)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void switchInstance()
    {
        if(this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
        else this.gameObject.SetActive(true);
    }
}
