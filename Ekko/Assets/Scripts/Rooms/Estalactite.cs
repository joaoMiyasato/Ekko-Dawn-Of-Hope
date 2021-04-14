using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estalactite : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "GroundDestructable")
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            StartCoroutine(die());
        }
    }
    private IEnumerator die()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
