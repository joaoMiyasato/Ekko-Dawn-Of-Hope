using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinasAbandonadas_Manager : MonoBehaviour
{
    public static MinasAbandonadas_Manager instance;
    public bool Energy;
    public List<GameObject> elecDoors;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }
private void Update()
{
    if(Input.GetKeyDown(KeyCode.V)) SwitchEnergy();
}
    public void SwitchEnergy()
    {
        Energy = !Energy;

        for (int i = 0; i < elecDoors.Count; i++)
        {
            elecDoors[i].GetComponent<ElecDoors>().switchInstance();
        }
    }
}
