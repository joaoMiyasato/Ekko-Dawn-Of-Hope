using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Object", menuName = "Items/Enemy")]
public class EnemyObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Enemy;
    }
}
