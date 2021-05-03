using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private int damage = 1;
    private int curHealth;
    private bool recover;
    private float recovering = 0.35f;
    private bool back = false;
    void Start()
    {
        curHealth = maxHealth;
    }

    void Update()
    {
        if(recover == true)
        {
            recovering -= Time.deltaTime;
            if(recovering < 0)
            {
                recover = false;
                recovering = 0.35f;
            }

        }
        if(curHealth <= 0)
        {
            Die();
        }
    }

    public void takeDamage(int damage)
    {
        curHealth -= damage;
        back = true;
        recover = true;
    }

    void Die()
    {
        CameraControl.instance.StartShake(0.2f,0.1f,2f);
        Destroy(gameObject);
    }

    public void setBack(bool back)
    {
        this.back = back;
    }

    public int getDamage()
    {
        return this.damage;
    }

    public bool getBack()
    {
        return this.back;
    }

    public bool getRecover()
    {
        return this.recover;
    }

    
}
