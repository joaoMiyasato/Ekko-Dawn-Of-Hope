using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Memory Object", menuName = "Items/Memory")]
public class MemoryObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Memory;
    }
}