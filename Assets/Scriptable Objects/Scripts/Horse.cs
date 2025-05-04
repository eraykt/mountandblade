using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHorse", menuName = "Inventory/Horse")]
public class Horse : Item
{
    public int hitPoints; //ne kadar kullanılırsa o kadar azalır
    public float speed;
    public int armor;
    public int riding;

    public override string GetItemDetails()
    {
        return base.GetItemDetails() + $"\nArmor: {armor}\nSpeed: {speed}\nHit Points: {hitPoints}\nRequires Riding: {riding}";
    }
}
