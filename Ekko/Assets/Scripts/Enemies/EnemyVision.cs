using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField]
    private LayerMask mask;
    private bool isHidding;
    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(GameObject.Find("Player (TestObject)").transform);
    }
    private void FixedUpdate()
    {
        detectRayCollision();
    }
    void detectRayCollision()
    {
        RaycastHit2D vision;

        vision = Physics2D.Raycast(transform.position, transform.forward, 1000, mask);

        if(vision.collider != null)
        {
            if(vision.collider.tag == "Player")
            {
                isHidding = false;
            }
            else
            {
                isHidding = true;
            }
        }
        else
        {
            isHidding = true;
        }
    }

    
    public bool show = true;
    private void OnDrawGizmosSelected()
    {
        if(show)
        {
            Gizmos.DrawRay(transform.position, transform.forward*1000);
        }
    }

    public bool getIsHidding()
    {
        return this.isHidding;
    }
}
