using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeapon", menuName = "Inventory/Weapon")]
public class Weapon : Item
{
    public int attackDamage;
    public float attackSpeed;
    public WeaponType weaponType;

    public enum WeaponType
    {
        OneHanded,
        TwoHanded,
        TwoAndOneHanded,
        Polearm,
        PolearmTwoHanded,
        Bow,
        Crossbow,
        ThrownWeapon,
        Firearm
    }

    public override string GetItemDetails()
    {
        return base.GetItemDetails() + $"\nAttack Damage: {attackDamage}\nAttack Speed: {attackSpeed} seconds\nWeapon Type: {weaponType}";
    }

}
