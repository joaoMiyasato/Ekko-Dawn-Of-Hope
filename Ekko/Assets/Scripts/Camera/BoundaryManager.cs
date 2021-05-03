using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    private BoxCollider2D managerBox;
    [SerializeField]
    private Transform player;
    public GameObject boundary;

    private void Start()
    {
        managerBox = GetComponent<BoxCollider2D>();
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Update()
    {
        ManageBoundary();
    }

    private void ManageBoundary()
    {
        if( managerBox.bounds.min.x < player.position.x && managerBox.bounds.max.x > player.position.x &&
            managerBox.bounds.min.y < player.position.y && managerBox.bounds.max.y > player.position.y)
        {
            boundary.SetActive(true);
        }
        else
        {
            boundary.SetActive(false);
        }
    }
}
