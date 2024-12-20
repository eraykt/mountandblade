using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Inventory/Food")]
public class Food : Item
{
    public int moraleBonus;

    public override string GetItemDetails()
    {
        return base.GetItemDetails() + $"\nMorale Bonus: {moraleBonus}";
    }
}
