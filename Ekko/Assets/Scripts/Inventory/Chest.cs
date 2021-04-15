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

            if(PlayerManager.instance.getHasLantern() != true)
            {
                PlayerManager.instance.setHasLantern(hasLantern);
            }

            PlayerManager.instance.playerBase.addPowerPoints(powerPoints);

            PlayerManager.instance.playerBase.addEnergyStones(energyStones);

            if(memory != null)
            {
                PlayerManager.instance.inventoryMemory.AddItem(memory, 1);
            }

            if(jewel != null)
            {
                PlayerManager.instance.inventoryJewel.AddItem(jewel, 1);
            }

            if(syntesis1 != null)
            {
                PlayerManager.instance.inventorySyntesis.AddItem(syntesis1, synCount1);
            }

            if(syntesis2 != null)
            {
                PlayerManager.instance.inventorySyntesis.AddItem(syntesis2, synCount2);
            }

            if(syntesis3 != null)
            {
                PlayerManager.instance.inventorySyntesis.AddItem(syntesis3, synCount3);
            }
        }
    }
}
