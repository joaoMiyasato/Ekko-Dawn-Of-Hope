using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Jewel Object", menuName = "Items/Jewel")]
public class JewelObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Jewel;
    }
}