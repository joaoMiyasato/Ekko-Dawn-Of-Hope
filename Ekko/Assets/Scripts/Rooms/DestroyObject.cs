using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public InteractableObj match;
    public GameObject Attach;
    public bool destroyableAnim = false;
    public bool destroyableObject = false;
    public bool onSkill = false;
    public bool Impact_Check = false;
    public int objHealth = 0;
    public int curHealth;
    public bool interacting = false;
    private Animator anim;

    private void Start()
    {
        curHealth = objHealth;
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(match.Open)
        {
            curHealth = 0;
            if(onSkill)
            {
                Impact_Check = true;
            }
        }

        destroy();
    }
    public void Interact()
    {
        if(curHealth > 0)
        {
            interacting = true;
        }
        curHealth--;
    }
    private void destroy()
    {
        if(curHealth <= 0 && !onSkill)
        {
            if(!match.Open)
            {
                CameraControl.instance.StartShake(0.1f,0.2f,2f);
            }
            match.Open = true;
            if(destroyableObject)
            {
                CameraControl.instance.StartShake(0.1f,0.2f,2f);
                if(Attach != null)
                {
                    Destroy(Attach);
                }
                Destroy(gameObject);
            }
            else if(destroyableAnim)
            {
                anim.SetTrigger("destroy");
            }
        }
        if(onSkill)
        {
            if(Impact_Check)
            {
                if(!match.Open)
                {
                    CameraControl.instance.StartShake(0.1f,0.2f,2f);
                }
                match.Open = true;
                if(Attach != null)
                {
                    Destroy(Attach);
                }
                Destroy(gameObject);
            }
        }
    }
}
