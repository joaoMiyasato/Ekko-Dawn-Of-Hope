using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Items/Weapon")]
public class WeaponObject : ItemObject
{
    public int weaponDamage;
    public float weaponAttackRate;
    public Vector2 weaponHorizontalPosition;
    public Vector2 weaponUpPosition;
    public Vector2 weaponDownPosition;
    public float weaponRangeHorizontal0;
    public float weaponRangeHorizontal1;
    public float weaponRangeUp0;
    public float weaponRangeUp1;
    public float weaponRangeDown0;
    public float weaponRangeDown1;

    public void Awake()
    {
        type = ItemType.Weapon;
    }

}
