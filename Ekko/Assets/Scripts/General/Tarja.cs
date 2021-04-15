using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarja : MonoBehaviour
{
    public GameObject tarja;
    private void Start()
    {
        tarja.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            tarja.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            tarja.SetActive(false);
        }
    }
}
