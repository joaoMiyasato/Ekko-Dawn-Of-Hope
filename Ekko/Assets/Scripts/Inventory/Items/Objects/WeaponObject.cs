using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Items/Weapon")]
public class WeaponObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Weapon;
    }
}
