using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onAnimationControl : MonoBehaviour
{
    public string newTag;
    public int newLayer;

    public void switch_Tag()
    {
        this.gameObject.tag = newTag;
    }
    public void switch_Layer()
    {
        this.gameObject.layer = newLayer;
    }



    public float pow, tim, rot;

    public void shake()
    {
        CameraControl.instance.StartShake(pow,tim,rot);
    }



    public PhysicsMaterial2D newMaterial;

    public void changeMaterial()
    {
        GetComponent<Collider2D>().sharedMaterial = newMaterial;
    }
}
