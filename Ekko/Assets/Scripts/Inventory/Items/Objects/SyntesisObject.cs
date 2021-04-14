using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Syntesis Object", menuName = "Items/Syntesis")]
public class SyntesisObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Syntesis;
    }
}