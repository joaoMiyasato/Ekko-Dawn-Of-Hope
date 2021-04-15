using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private DestroyObject interact;
    public int chestNumber;

    public bool hasLantern;
    public int powerPoints;
    public int energyStones;
    public MemoryObject memory;
    public JewelObject jewel;
    public SyntesisObject syntesis1;
    public int synCount1;
    public SyntesisObject syntesis2;
    public int synCount2;
    public SyntesisObject syntesis3;
    public int synCount3;

    private void Start()
    {
        interact = this.gameObject.GetComponent<DestroyObject>();
    }
    
    private void Update()
    {

        if(interact.interacting)
        {
            interact.interacting = false;

            if(PlayerManager.instance.Lantern != true)
            {
                PlayerManager.instance.Lantern = hasLantern;
            }

            PlayerManager.instance.PowerPoints += powerPoints;

            PlayerManager.instance.EnergyStones += energyStones;

            if(memory != null)
            {
                PlayerManager.instance.inventoryManager.memory.AddItem(memory, 1);
            }

            if(jewel != null)
            {
                PlayerManager.instance.inventoryManager.jewel.AddItem(jewel, 1);
            }

            if(syntesis1 != null)
            {
                PlayerManager.instance.inventoryManager.syntesis.AddItem(syntesis1, synCount1);
            }

            if(syntesis2 != null)
            {
                PlayerManager.instance.inventoryManager.syntesis.AddItem(syntesis2, synCount2);
            }

            if(syntesis3 != null)
            {
                PlayerManager.instance.inventoryManager.syntesis.AddItem(syntesis3, synCount3);
            }
        }
    }
}
