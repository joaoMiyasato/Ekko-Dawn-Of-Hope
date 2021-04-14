using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorsButton : MonoBehaviour
{
    public GameObject door1,door2;

    public void Interact()
    {
        door1.GetComponent<ElecDoors>().switchInstance();
        door2.GetComponent<ElecDoors>().switchInstance();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Spike")
        {
            Interact();
        }
    }
}
