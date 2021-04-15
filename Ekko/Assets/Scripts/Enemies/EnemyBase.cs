using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public bool bossActivated = false;
    public int maxHealth = 10;
    public int Damage = 1;
    public int curHealth;
    public bool Recover;
    private float recovering = 0.35f;
    public bool Back = false;
    public GameObject bossArea;
    void Start()
    {
        curHealth = maxHealth;
    }

    void Update()
    {
        if(Recover == true)
        {
            recovering -= Time.deltaTime;
            if(recovering < 0)
            {
                Recover = false;
                recovering = 0.35f;
            }

        }
        if(curHealth <= 0)
        {
            if(this.gameObject.tag == "Boss")
            {
                bossArea.GetComponent<scr_bossArea>().bossDefeated = true;
            }
            Die();
        }
    }

    public void takeDamage(int damage)
    {
        curHealth -= damage;
        Back = true;
        Recover = true;
    }

    void Die()
    {
        CameraControl.instance.StartShake(0.2f,0.1f,2f);
        Destroy(gameObject);
    }
}
