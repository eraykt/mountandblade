using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHorse", menuName = "Inventory/Horse")]
public class Horse : Item
{
    public int hitPoints; //þimdilik int sonrasýna 100/100 örneðin seklinde olacak
    public float speed;
    public int armor;
    public int maneuver;
    public int charge;
    public int riding;

    public override string GetItemDetails()
    {
        return base.GetItemDetails() + $"\nArmor: {armor}\nSpeed: {speed}\nManeuver: {maneuver}\nCharge: {charge}\nHit Points: {hitPoints}\nRequires Riding: {riding}";
    }
}
