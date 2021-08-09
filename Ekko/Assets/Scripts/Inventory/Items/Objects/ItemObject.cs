using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Memory,
    Syntesis,
    Jewel, 
    Enemy,
    Weapon
}
public abstract class ItemObject : ScriptableObject
{
    public string Name;
    public Sprite sprite;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}