using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Item currentItem;

    public void SetSlot(Item item) 
    {
        currentItem = item;
        if (icon != null && item.Itemicon != null)
        {
            icon.sprite = item.Itemicon; // ScriptableObject'ten ikonu al ve slota koy
            icon.enabled = true;
        }
        else
        {
            Debug.LogWarning("Slot veya item'de eksik alanlar var!");
        }

    }

    public void ClearSlot() 
    {
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
