using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if(item)
        {
            PlayerManager.instance.inventoryManager.syntesis.AddItem(item.item,1);
            Destroy(other.gameObject);
        }
        UI_manager.instance.DisplaySyntesis();
    }
}
