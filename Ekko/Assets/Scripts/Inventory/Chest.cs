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

            if(scr_player_manager.instance.Lantern != true)
            {
                scr_player_manager.instance.Lantern = hasLantern;
            }

            scr_player_manager.instance.PowerPoints += powerPoints;

            scr_player_manager.instance.EnergyStones += energyStones;

            if(memory != null)
            {
                scr_player_manager.instance.inventoryMemory.AddItem(memory, 1);
            }

            if(jewel != null)
            {
                scr_player_manager.instance.inventoryJewel.AddItem(jewel, 1);
            }

            if(syntesis1 != null)
            {
                scr_player_manager.instance.inventorySyntesis.AddItem(syntesis1, synCount1);
            }

            if(syntesis2 != null)
            {
                scr_player_manager.instance.inventorySyntesis.AddItem(syntesis2, synCount2);
            }

            if(syntesis3 != null)
            {
                scr_player_manager.instance.inventorySyntesis.AddItem(syntesis3, synCount3);
            }
        }
    }
}
