using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Inventory/Armor")]
public class Armor : Item
{
    public int defense;
    public int health;
    public ArmorType armorType;

    public enum ArmorType 
    {
        Helmet,
        Boots,
        Shields,
        Gloves,
        BodyArmor
    }

    public override string GetItemDetails()
    {
        return base.GetItemDetails() + $"\nDefense: {defense}\nHealth: {health}\nArmor Type: {armorType}";
    }
}
