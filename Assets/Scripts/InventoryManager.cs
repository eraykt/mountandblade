using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform content;
    public GameObject slotPrefab;
    public List<Item> items;
    public int totalSlot = 21;

    private void Start() 
    {
        PopulateInventory();
        AddEmptySlots();
    }

    private void PopulateInventory() 
    {

        foreach (var item in items) 
        {
            GameObject slotObject = Instantiate(slotPrefab, content);
            InventorySlot slot = slotObject.GetComponent <InventorySlot>();
            if (slot != null)
            {
                slot.SetSlot(item); // Item'i slota baðla
            }
            else
            {
                Debug.LogError("InventorySlot script'i slot prefab'ýnda bulunamadý!");
            }
        }
    }


    private void AddEmptySlots() 
    {
        int currentSlotCount = content.childCount;
        int emptySlotsToAdd = totalSlot - currentSlotCount;

        for (int i = 0; i < emptySlotsToAdd; i++) 
        {
            Instantiate(slotPrefab, content); //boþ slot oluþturma
        }
    }
}
